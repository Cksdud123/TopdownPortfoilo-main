using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy; // 생성 후 일정 시간이 지난 후 자동으로 파괴될 시간
    [HideInInspector] public WeaponManager weapon; // 총기 정보를 가지고 있는 WeaponManager 객체
    [HideInInspector] public Vector3 dir; // 총알이 발사된 방향 벡터

    // Start is called before the first frame update
    void Start()
    {
        // 일정 시간이 지난 후 자동으로 파괴되도록 설정
        Destroy(this.gameObject, timeToDestroy);
    }
}
