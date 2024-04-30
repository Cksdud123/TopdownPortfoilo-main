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
        if (HP <= 0)
        {
            Die(); // HP�� 0 �����̸� Die �Լ� ȣ��
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
            // RagdollManager�� ������ ���� Ragdoll Ȱ��ȭ
            ragdollManager.setRigidbodyState(false);
            ragdollManager.setColliderState(true);
        }

        navMeshAgent.enabled = false;

        // �ִϸ��̼� ��Ȱ��ȭ
        animator.enabled = false;

        // ������Ʈ ���� �ð� �� ����
        StartCoroutine(DestroyAfterDelay(10f)); // 10�� �Ŀ� ����
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
