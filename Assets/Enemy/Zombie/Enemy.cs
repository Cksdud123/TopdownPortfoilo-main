using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;
    public CapsuleCollider capsuleCollider; // 캡슐 콜라이더 참조

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            Die(); // HP가 0 이하이면 Die 함수 호출
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    private void Die()
    {
        // 애니메이션 트리거 설정
        animator.SetTrigger("Die");

        // 캡슐 콜라이더 비활성화
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }

        // 오브젝트 일정 시간 후 제거
        StartCoroutine(DestroyAfterDelay(10f)); // 3초 후에 제거
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
