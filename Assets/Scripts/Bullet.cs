using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : PoolAble
{
    [HideInInspector] public WeaponManager weapon; // �ѱ� ������ ������ �ִ� WeaponManager ��ü
    [HideInInspector] public Vector3 dir; // �Ѿ��� �߻�� ���� ����

    public GameObject bloodEffect;

    public int DamageAmount = 20;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� Enemy �±׸� ���� ������Ʈ���� Ȯ��
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemyHealth = other.GetComponentInParent<Enemy>();

            if (enemyHealth != null)
            {
                // Enemy ��ũ��Ʈ�� �ִ� ���, �������� �ְ� ���� ����Ʈ ����
                enemyHealth.TakeDamage(DamageAmount);

                if (bloodEffect != null)
                {
                    // �浹 ������ ���� ����
                    Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
                    Vector3 hitNormal = (hitPoint - enemyHealth.transform.position).normalized;

                    // ���� ����Ʈ ����
                    GameObject bloodEffectGo = Instantiate(bloodEffect, hitPoint, Quaternion.LookRotation(hitNormal));
                    Destroy(bloodEffectGo, 1f); // ���� �ð� �Ŀ� ���� ����Ʈ ����
                }
            }
        }
    }
    public void DestroyBullet()
    {
        ReleaseObject();
    }
}
