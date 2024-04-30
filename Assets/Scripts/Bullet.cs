using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy; // ���� �� ���� �ð��� ���� �� �ڵ����� �ı��� �ð�
    [HideInInspector] public WeaponManager weapon; // �ѱ� ������ ������ �ִ� WeaponManager ��ü
    [HideInInspector] public Vector3 dir; // �Ѿ��� �߻�� ���� ����

    public GameObject bloodEffect;

    public int DamageAmount = 20;

    // Start is called before the first frame update
    void Start()
    {
        // ���� �ð��� ���� �� �ڵ����� �ı��ǵ��� ����
        Destroy(this.gameObject, timeToDestroy);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Enemy>())
        {
            Enemy enemyHealth = other.gameObject.GetComponentInParent<Enemy>();
            enemyHealth.TakeDamage(DamageAmount);

            // ���� ��ġ�� ���� ����Ʈ ����
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
