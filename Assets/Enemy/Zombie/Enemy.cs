using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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
        if (ragdollManager != null)
        {
            // RagdollManager가 존재할 때만 Ragdoll 활성화
            ragdollManager.setRigidbodyState(false);
            ragdollManager.setRigidbodyState(false);
        }

        navMeshAgent.enabled = false;

        // 애니메이션 비활성화
        animator.enabled = false;

        // 오브젝트 일정 시간 후 제거
        StartCoroutine(DestroyAfterDelay(10f)); // 10초 후에 제거
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
