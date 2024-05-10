using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip IdleState, ChaseState, AttackState, PatrolState, getHitState;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play_IdleSound()
    {
        audioSource.clip = IdleState;
        audioSource.Play();
    }
    public void Play_ChaseSound()
    {
        audioSource.clip = ChaseState;
        audioSource.Play();
    }
    public void Play_AttackSound()
    {
        audioSource.clip = AttackState;
        audioSource.Play();
    }
    public void Play_PatrolSound()
    {
        audioSource.clip = PatrolState;
        audioSource.Play();
    }
    public void Play_getHitSound()
    {
        audioSource.clip = getHitState;
        audioSource.Play();
    }
}
