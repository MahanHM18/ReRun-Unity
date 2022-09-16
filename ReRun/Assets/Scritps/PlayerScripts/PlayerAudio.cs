using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource MovementAudio;
    public AudioSource SwordAudio;
    public AudioSource HitEnemyAudio;
    public AudioSource HitPlayerAudio;

    [SerializeField] private AudioClip[] FootStepsClips;
    [SerializeField] private AudioClip StartSlideClip;
    [SerializeField] private AudioClip JumpClip;
    [SerializeField] private AudioClip LandClip;
    [SerializeField] private AudioClip PickUpClip;
    [SerializeField] private AudioClip[] AttackSwordClip;
    [SerializeField] private AudioClip[] HitEnemyClip;
    

    public void PlayFootStepsAudio()
    {
        MovementAudio.clip = FootStepsClips[Random.Range(0, FootStepsClips.Length)];
        MovementAudio.Play();
    }

    public void PlayStartSlideAudio()
    {
        MovementAudio.clip = StartSlideClip;
        MovementAudio.Play();
    }
    
    public void PlayJumpAudio()
    {
        MovementAudio.clip = JumpClip;
        MovementAudio.Play();
    }
    
    public void PlayLandAudio()
    {
        MovementAudio.clip = LandClip;
        MovementAudio.Play();
    }

    public void PlayPickUpClip()
    {
        MovementAudio.clip = PickUpClip;
        MovementAudio.Play();
    }

    public void PlayAttackClip()
    {
        SwordAudio.clip = AttackSwordClip[Random.Range(0, AttackSwordClip.Length)];
        SwordAudio.Play();
    }

    public void PlayHitEnemyClip()
    {
        HitEnemyAudio.clip = HitEnemyClip[Random.Range(0, HitEnemyClip.Length)];
        HitEnemyAudio.Play();
    }

    public void PlayHitPlayerClip()
    {
        HitPlayerAudio.Play();
    }
}