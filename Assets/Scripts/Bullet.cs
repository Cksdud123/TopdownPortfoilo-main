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
    public TrailRenderer trail;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this.gameObject, timeToDestroy);
        Invoke("DestroyBullet", 1f);
    }
    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        ManagePool = pool;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Enemy>())
        {
            Enemy enemyHealth = other.gameObject.GetComponentInParent<Enemy>();
            enemyHealth.TakeDamage(DamageAmount);

            // 적의 위치에 블러드 이펙트 생성
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
    public void DestroyBullet()
    {
        ManagePool.Release(this);
    }
}
