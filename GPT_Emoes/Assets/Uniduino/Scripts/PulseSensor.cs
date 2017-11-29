using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uniduino;
using DG.Tweening;
using System.IO.Ports;

public class PulseSensor : MonoBehaviour {

    public FloatVariable stresslevel;
    public FloatVariable BPMlevel;

    public Arduino arduino;
    public int pin = 0;
    public int pinValue;
    int calibration = 0;

    private bool coroutine = false;

    volatile int BPM;
    volatile int Signal;
    volatile int IBI = 500;
    volatile int LastIBI = 0;
    volatile bool Pulse = false;
    volatile bool QS = false;

    volatile int[] rate = new int[10];
    long sampleCounter = 0;
    long lastBeatTime = 0;
    long lastTime = 0, N;
    volatile int P = 512;
    volatile int T = 512;
    volatile int thresh = 525;
    volatile int amp = 100;
    volatile bool firstBeat = true;
    volatile bool secondBeat = false;

    GameObject emptyGO;


    // Use this for initialization
    void Start () {
        arduino = Arduino.global;
        arduino.Setup(ConfigurePins);
        emptyGO = new GameObject();
        emptyGO.name = "stressfloat";
        emptyGO.transform.position.Set(16, 0, 0);
	}

    void ConfigurePins()
    {
        arduino.pinMode(0, PinMode.ANALOG);
        arduino.pinMode(13, PinMode.OUTPUT);
        arduino.reportAnalog(0, 1);
    }



    // Update is called once per frame
    void Update()
    {
        pinValue = arduino.analogRead(0);
        if(QS == true)
        {
            if (!coroutine)
            {
                coroutine = true;
                StartCoroutine(PulseFound());
            }
            StartCoroutine(Delay());
        }else if(Time.realtimeSinceStartup*1000 >= (lastTime + 20))
        {
            //print(lastTime.ToString());
            lastTime = (long)(Time.realtimeSinceStartup * 1000);
            readPulse();
            
        }
    }

    void readPulse()
    {
        Signal = arduino.analogRead(0);
        Mathf.Clamp(Signal, 350, 800);
        sampleCounter += 20;
        int N = (int)(sampleCounter - lastBeatTime);

        detectSetHighLow();

        if (N > 250)
        {
            if ((Signal > thresh) && (Pulse == false) && (N > (IBI / 5) * 3))
            {
                pulseDetected();
            }
            if(Signal < thresh && Pulse == true)
            {
                Pulse = false;
                amp = P - T;
                thresh = ((int)(amp *0.75)) + T;
                P = thresh;
                T = thresh;
            }
            if(N > 2500)
            {
                thresh = 512;
                P = 512;
                T = 512;
                lastBeatTime = sampleCounter;
                firstBeat = true;
                secondBeat = false;
            }
        }

    }

    void detectSetHighLow()
    {
        if(Signal < thresh && N > (IBI / 5) * 3)
        {
            if (Signal < T)
            {
                T = Signal;
            }
        }
        if(Signal > thresh && Signal > P)
        {
            P = Signal;
        }
    }

    void pulseDetected()
    {
        //print("pulse");
        Pulse = true;
        LastIBI = IBI;
        IBI = (int)(sampleCounter - lastBeatTime);
        print(IBI);
        if ((IBI - (sampleCounter - lastBeatTime)) > 300)
        {
            IBI = LastIBI - 150;
        }
        else if((sampleCounter - lastBeatTime) - IBI > 300)
        {
            IBI = LastIBI + 150;
        }
        else
        {
            IBI = (int)(sampleCounter - lastBeatTime);
        }
        lastBeatTime = sampleCounter;

        int[] lastRate = new int[10];
        lastRate = rate;

        if (secondBeat)
        {
            secondBeat = false;
        }
        if (firstBeat)
        {
            firstBeat = false;
            secondBeat = true;

            return;
        }
        if(calibration < 10 && !secondBeat && !firstBeat)
        {
            rate[calibration] = IBI;
            calibration++;
        }
        else if (calibration == 10)
        {
            int runningTotal = CalculateRunningTotal(rate);

            if ((60000 / runningTotal) > 150 || (60000 / runningTotal) < 35)
            {
                runningTotal = CalculateRunningTotal(lastRate);
                BPM = 60000 / runningTotal;
                Mathf.Clamp(BPM, 35, 150);
            }
            else
            {
                BPM = 60000 / runningTotal;
            }
            QS = true;
        }
        
    }

    int CalculateRunningTotal(int[] rateArray)
    {
        int[] rateCalculate = new int[10];
        rateCalculate = rateArray;
        int runningTotal = 0;

        for (int i = 0; i <= 8; i++)
        {
            rateCalculate[i] = rateCalculate[i+1];
            runningTotal += rateCalculate[i];
        }
        
        rateCalculate[9] = IBI;

        runningTotal += rateCalculate[9];
        runningTotal /= 10;

        return runningTotal;
    }

    void CalculateStressLevel()
    {
        float lastStress = stresslevel.Value;
        Transform t = emptyGO.transform;
        t.position.Set(lastStress, 0, 0);
        t.DOMoveX((float)BPM / (150f - 35f), 1).OnUpdate(() => {
            stresslevel.SetValue(t.position.x);
            lastStress = stresslevel.Value;
        });
    }

    IEnumerator PulseFound()
    {
        while (coroutine)
        {
            yield return new WaitForSeconds(1f);
            print("BPM: " + BPM.ToString());
            BPMlevel.SetValue(BPM);
            CalculateStressLevel();
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.025f);
        QS = false;
        StopCoroutine(Delay());
    }
}
