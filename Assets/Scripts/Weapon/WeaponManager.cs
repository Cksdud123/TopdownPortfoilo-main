using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Pool;


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

    public float shootingRange = 100f;
    public GameObject bloodEffect;

    // Start is called before the first frame update
    private void Awake()
    {
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        actions = GetComponentInParent<ActionStateManager>();
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
            Bullet();
        }
    }

    private void TriggerMuzzleFlash()
    {
        muzzleFlash.Play();
    }
    public void Bullet()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(barrelPos.transform.position, barrelPos.transform.forward, out hitInfo, shootingRange))
        {
            Enemy enemy = hitInfo.transform.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(20);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
        }
    }
}
