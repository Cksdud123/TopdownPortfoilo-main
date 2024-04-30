using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy; // ���� �� ���� �ð��� ���� �� �ڵ����� �ı��� �ð�
    [HideInInspector] public WeaponManager weapon; // �ѱ� ������ ������ �ִ� WeaponManager ��ü
    [HideInInspector] public Vector3 dir; // �Ѿ��� �߻�� ���� ����

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

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(dir * weapon.enemyKickbackForce, ForceMode.Impulse);
        }
    }
}
