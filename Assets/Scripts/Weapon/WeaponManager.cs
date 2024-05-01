using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Burst.Intrinsics;


public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate; // 발사 속도 (초당 발사 가능한 횟수)
    [SerializeField] bool semiAuto; // 반자동 발사 여부
    float fireRateTimer; // 발사 속도 타이머

    [Header("Bullet Properties")]
    [SerializeField] GameObject bulletPrefabs; // 발사할 총알 프리팹
    [SerializeField] Transform barrelPos; // 총구 위치
    [SerializeField] float bulletVelocity; // 총알 발사 속도
    [SerializeField] int bulletsPerShot; // 발사할 총알 개수
    public float damage = 20; // 총알 데미지

    [SerializeField] public WeaponAmmo ammo; // 총알 잔여량 관리 클래스

    ParticleSystem muzzleFlash;

    private ActionStateManager actions;
    WeaponClassManager weaponClass;
    WeaponRecoil recoil;

    public Transform IHandTarget;
    public Transform RHandTarget;

    private IObjectPool<Bullet> bulletPool;

    // Start is called before the first frame update
    private void Awake()
    {
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        actions = GetComponentInParent<ActionStateManager>();
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 15);
    }
    void Start()
    {
        fireRateTimer = fireRate;
    }
    private void OnEnable()
    {
        if (weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            ammo = GetComponent<WeaponAmmo>();
            recoil = GetComponent<WeaponRecoil>();
            recoil.recoilFollowPos = weaponClass.recoilFollowPos;
        }
        weaponClass.SetCurrentWeapon(this);
    }
    // Update is called once per frame
    void Update()
    {

        if (ShouldFire())
        {
            Fire();
        }
    }
    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime; // 발사 속도 타이머 갱신
        if (fireRateTimer < fireRate) return false; // 발사 속도 타이머가 발사 속도보다 작으면 발사할 수 없음
        if (ammo.currentAmmo == 0) return false; // 총알이 없으면 발사할 수 없음
        if(actions.currentState == actions.Default) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true; // 반자동 모드에서 마우스 왼쪽 버튼이 눌리면 발사 가능
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true; // 전자동 모드에서 마우스 왼쪽 버튼이 눌려 있으면 계속 발사
        return false; // 발사 조건이 아니면 발사하지 않음
    }
    void Fire()
    {
        fireRateTimer = 0; // 발사 속도 타이머 초기화
        ammo.currentAmmo--; // 총알 소모

        AmmoUI.instance.UpdateAmmoText(ammo.currentAmmo);
        AmmoUI.instance.UpdateMagText(ammo.extraAmmo);

        TriggerMuzzleFlash(); // 총구 화염 효과 실행
        recoil.TriggerRecoil();

        for (int i = 0; i < bulletsPerShot; i++) // 발사할 총알 개수만큼 반복
        {
            //GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation); // 총알 생성
            var bullet = bulletPool.Get();

            Bullet bulletScript = bullet.GetComponent<Bullet>(); // 총알 스크립트 가져오기
            bulletScript.weapon = this; // 총기 정보 설정

            bulletScript.dir = barrelPos.transform.forward; // 총알 방향 설정

            Rigidbody rb = bullet.GetComponent<Rigidbody>(); // 총알 Rigidbody 컴포넌트 가져오기
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse); // 총알에 힘을 가해 발사
            Debug.Log("bullet 위치" + bullet.transform.position);
        }
    }
    private void TriggerMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    // 오브젝트 풀
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefabs,barrelPos.position,barrelPos.rotation).GetComponent<Bullet>();
        bullet.SetManagedPool(bulletPool);
        return bullet;
    }
    private void OnGetBullet(Bullet bullet)
    {
        bullet.transform.SetParent(null); // 이전의 부모 계층에서 분리
        bullet.transform.position = barrelPos.position; // 총알의 위치를 총구의 위치로 설정
        bullet.transform.rotation = barrelPos.rotation; // 총알의 회전을 총구의 회전으로 설정

        // 총알의 Rigidbody 초기화
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false); // 게임 오브젝트 비활성화
    }
    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
