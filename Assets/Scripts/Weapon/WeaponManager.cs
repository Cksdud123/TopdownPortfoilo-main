using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using UnityEditor.PackageManager;
using Unity.Burst.Intrinsics;

public enum Weapon
{
    Rifle,
    Pistol,
    Knife
}
public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate; // 발사 속도 (초당 발사 가능한 횟수)
    [SerializeField] bool semiAuto; // 반자동 발사 여부
    float fireRateTimer; // 발사 속도 타이머

    [Header("Bullet Properties")]
    [SerializeField] Transform barrelPos; // 총구 위치
    [SerializeField] float bulletVelocity; // 총알 발사 속도
    [SerializeField] int bulletsPerShot; // 발사할 총알 개수
    private float damage = 0;

    [SerializeField] public WeaponAmmo ammo; // 총알 잔여량 관리 클래스

    ParticleSystem muzzleFlash;

    private ActionStateManager actions;
    WeaponClassManager weaponClass;
    WeaponBloom bloom;
    WeaponRecoil recoil;

    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip pistolShot;
    [HideInInspector] public AudioSource audioSource;

    public Transform IHandTarget;
    public Transform RHandTarget;

    public float shootingRange = 100f;

    public Weapon weaponState;

    private Animator anim;

    // Start is called before the first frame update
    private void Awake()
    {
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        actions = GetComponentInParent<ActionStateManager>();
        anim = GetComponentInParent<Animator>();
        bloom = GetComponent<WeaponBloom>();
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
            audioSource = GetComponent<AudioSource>();
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
        else if (weaponState == Weapon.Knife)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
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
        //AmmoUI.instance.AmmoBarFilter(ammo.currentAmmo, ammo.clipSize);

        if(weaponState == Weapon.Rifle)
            audioSource.PlayOneShot(gunShot);
        else if(weaponState == Weapon.Pistol)
            audioSource.PlayOneShot(pistolShot);

        TriggerMuzzleFlash();
        recoil.TriggerRecoil();

        for (int i = 0; i < bulletsPerShot; i++) // 발사할 총알 개수만큼 반복
        {
            Bullet();
        }
    }
    void Attack()
    {
        anim.SetTrigger("Attacking");
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
            hitBullet();
            EnemyHealth enemy = hitInfo.transform.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage + Random.Range(15, 30));

                EnemySoundManager enemySound = enemy.GetComponent<EnemySoundManager>();
                enemySound.Play_getHitSound();

                var bloodEffectGo = ObjectPoolingManager.instance.GetGo("BloodEffect");
                bloodEffectGo.transform.position = hitInfo.point;
                bloodEffectGo.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            }
        }
    }
    public void hitBullet()
    {
        var bulletGo = ObjectPoolingManager.instance.GetGo("Bullet");

        bulletGo.transform.position = barrelPos.transform.position; // 총알의 위치를 총구의 위치로 설정
        bulletGo.transform.rotation = barrelPos.transform.rotation; // 총알의 회전을 총구의 회전으로 설정

        TrailRenderer trailRenderer = bulletGo.GetComponent<TrailRenderer>();

        // 총알의 Rigidbody 초기화
        Rigidbody rb = bulletGo.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(barrelPos.transform.forward * bulletVelocity, ForceMode.Impulse); // 총알에 힘을 가해 발사
        trailRenderer.Clear();
    }
}


