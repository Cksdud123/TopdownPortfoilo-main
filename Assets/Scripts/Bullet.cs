using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy; // ���� �� ���� �ð��� ���� �� �ڵ����� �ı��� �ð�
    [HideInInspector] public WeaponManager weapon; // �ѱ� ������ ������ �ִ� WeaponManager ��ü
    [HideInInspector] public Vector3 dir; // �Ѿ��� �߻�� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        // ���� �ð��� ���� �� �ڵ����� �ı��ǵ��� ����
        Destroy(this.gameObject, timeToDestroy);
    }
}
