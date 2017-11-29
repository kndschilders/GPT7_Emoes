using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterSoundManager : MonoBehaviour {

    public AudioClip[] RoamSounds;
    public AudioClip[] InvestigateSounds;
    public AudioClip[] ChaseSounds;
    public AudioClip WalkSoundLoop;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    public void PlayChaseCue()
    {
        audioSource.PlayOneShot(RandomUtil.RandomElement(ChaseSounds));
    }

    public void PlayInvestigateCue()
    {
        audioSource.PlayOneShot(RandomUtil.RandomElement(InvestigateSounds));
    }

    public void PlayRoamCue()
    {
        audioSource.PlayOneShot(RandomUtil.RandomElement(RoamSounds));
    }


}
