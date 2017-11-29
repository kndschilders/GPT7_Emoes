using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterSoundManager : MonoBehaviour
{
    public enum SoundCues
    {
        Roaming,
        Investigating,
        Chasing
    }

    public AudioClip[] RoamSounds;
    public AudioClip[] InvestigateSounds;
    public AudioClip[] ChaseSounds;
    public float AudioCueCooldownTime = 1f;
    public float MaxRandomCooldownDelay = 1f;
    public AudioClip WalkSoundLoop;

    private AudioSource audioSource;
    private bool cooldownActive = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundCue(SoundCues cue)
    {
        if (cooldownActive)
            return;

        AudioClip[] sounds = null;

        switch (cue)
        {
            case SoundCues.Roaming:
                sounds = RoamSounds;
                break;
            case SoundCues.Investigating:
                sounds = InvestigateSounds;
                break;
            case SoundCues.Chasing:
                sounds = ChaseSounds;
                break;
        }

        if (sounds == null || sounds.Length == 0)
            return;

        audioSource.PlayOneShot(RandomUtil.RandomElement(sounds));
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        cooldownActive = true;
        yield return new WaitForSeconds(AudioCueCooldownTime + Random.Range(0,MaxRandomCooldownDelay));
        cooldownActive = false;
    }


}
