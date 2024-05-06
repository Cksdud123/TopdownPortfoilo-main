using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolAble
{
    public int HP = 100;
    public Animator animator;
    [HideInInspector] public RagdollManager ragdollManager;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdollManager = GetComponent<RagdollManager>();
    }
    private void Start()
    {
        navMeshAgent.enabled = true;
        animator.enabled = true;
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        Debug.Log("총알 맞음!! 현재 체력" + HP);
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    private void Die()
    {
        DeactiveEnemy();
        StartCoroutine(ReleaseZombieAfterDelay(5f));
    }

    private IEnumerator ReleaseZombieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseObject();
    }

    public void DeactiveEnemy()
    {
        if (ragdollManager != null)
        {
            ragdollManager.setRigidbodyState(false);
        }
        navMeshAgent.enabled = false;
        animator.enabled = false;
    }
}
