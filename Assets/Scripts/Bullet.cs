using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy; // 생성 후 일정 시간이 지난 후 자동으로 파괴될 시간
    [HideInInspector] public WeaponManager weapon; // 총기 정보를 가지고 있는 WeaponManager 객체
    [HideInInspector] public Vector3 dir; // 총알이 발사된 방향 벡터

    public GameObject bloodEffect;

    public int DamageAmount = 20;

    private IObjectPool<Bullet> ManagePool;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this.gameObject, timeToDestroy);
        Invoke("DestroyBullet", 3f);
    }
    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        ManagePool = pool;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Enemy 태그를 가진 오브젝트인지 확인
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 충돌한 오브젝트가 Enemy 스크립트를 가지고 있는지 확인
            Enemy enemyHealth = other.GetComponentInParent<Enemy>();

            if (enemyHealth != null)
            {
                // Enemy 스크립트가 있는 경우, 데미지를 주고 블러드 이펙트 생성
                enemyHealth.TakeDamage(DamageAmount);

                if (bloodEffect != null)
                {
                    // 충돌 지점과 방향 설정
                    Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
                    Vector3 hitNormal = (hitPoint - enemyHealth.transform.position).normalized;

                    // 블러드 이펙트 생성
                    GameObject bloodEffectGo = Instantiate(bloodEffect, hitPoint, Quaternion.LookRotation(hitNormal));
                    Destroy(bloodEffectGo, 1f); // 일정 시간 후에 블러드 이펙트 제거
                }
            }
        }
    }

    public void DestroyBullet()
    {
        ManagePool.Release(this);
    }
}
