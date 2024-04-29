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
        Destroy(transform.GetComponent<Rigidbody>());
        if(other.tag == "Dragon")
        {
            other.GetComponent<Dragon>().TakeDamage(DamageAmount);
        }
    }
}
