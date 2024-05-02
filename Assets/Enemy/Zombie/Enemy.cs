using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolAble
{
    public int HP = 100;
    public Animator animator;
    RagdollManager ragdollManager;
    NavMeshAgent navMeshAgent;

    private Rigidbody rb;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdollManager = GetComponent<RagdollManager>();
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
            // RagdollManager가 존재할 때만 Ragdoll 활성화
            ragdollManager.setRigidbodyState(false);
        }
        navMeshAgent.enabled = false;
        animator.enabled = false;
    }
    public void ActiveEnemy()
    {
        HP = 100;

        if (ragdollManager != null)
        {
            // RagdollManager가 존재할 때만 Ragdoll 활성화
            ragdollManager.setRigidbodyState(true);
        }
        navMeshAgent.enabled = true;
        animator.enabled = true;
    }
}
