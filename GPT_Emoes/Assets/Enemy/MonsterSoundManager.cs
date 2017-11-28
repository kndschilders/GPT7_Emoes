using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterSoundManager : MonoBehaviour {

    public AudioClip RoamSound;
    public AudioClip InvestigateSound;
    public AudioClip ChaseSound;
    public AudioClip WalkSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

        
}
