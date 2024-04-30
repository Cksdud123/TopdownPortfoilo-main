using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;
    public CapsuleCollider capsuleCollider; // ĸ�� �ݶ��̴� ����

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
        // �ִϸ��̼� Ʈ���� ����
        animator.SetTrigger("Die");

        // ĸ�� �ݶ��̴� ��Ȱ��ȭ
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }

        // ������Ʈ ���� �ð� �� ����
        StartCoroutine(DestroyAfterDelay(10f)); // 3�� �Ŀ� ����
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
