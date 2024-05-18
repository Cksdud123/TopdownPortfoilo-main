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
    [SerializeField] float fireRate; // �߻� �ӵ� (�ʴ� �߻� ������ Ƚ��)
    [SerializeField] bool semiAuto; // ���ڵ� �߻� ����
    float fireRateTimer; // �߻� �ӵ� Ÿ�̸�

    [Header("Bullet Properties")]
    [SerializeField] Transform barrelPos; // �ѱ� ��ġ
    [SerializeField] float bulletVelocity; // �Ѿ� �߻� �ӵ�
    [SerializeField] int bulletsPerShot; // �߻��� �Ѿ� ����
    private float damage = 0;

    [SerializeField] public WeaponAmmo ammo; // �Ѿ� �ܿ��� ���� Ŭ����

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
        fireRateTimer += Time.deltaTime; // �߻� �ӵ� Ÿ�̸� ����
        if (fireRateTimer < fireRate) return false; // �߻� �ӵ� Ÿ�̸Ӱ� �߻� �ӵ����� ������ �߻��� �� ����
        if (ammo.currentAmmo == 0) return false; // �Ѿ��� ������ �߻��� �� ����
        if(actions.currentState == actions.Default) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true; // ���ڵ� ��忡�� ���콺 ���� ��ư�� ������ �߻� ����
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true; // ���ڵ� ��忡�� ���콺 ���� ��ư�� ���� ������ ��� �߻�
        return false; // �߻� ������ �ƴϸ� �߻����� ����
    }
    void Fire()
    {
        fireRateTimer = 0; // �߻� �ӵ� Ÿ�̸� �ʱ�ȭ
        ammo.currentAmmo--; // �Ѿ� �Ҹ�

        AmmoUI.instance.UpdateAmmoText(ammo.currentAmmo);
        AmmoUI.instance.UpdateMagText(ammo.extraAmmo);
        //AmmoUI.instance.AmmoBarFilter(ammo.currentAmmo, ammo.clipSize);

        if(weaponState == Weapon.Rifle)
            audioSource.PlayOneShot(gunShot);
        else if(weaponState == Weapon.Pistol)
            audioSource.PlayOneShot(pistolShot);

        TriggerMuzzleFlash();
        recoil.TriggerRecoil();

        for (int i = 0; i < bulletsPerShot; i++) // �߻��� �Ѿ� ������ŭ �ݺ�
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

        bulletGo.transform.position = barrelPos.transform.position; // �Ѿ��� ��ġ�� �ѱ��� ��ġ�� ����
        bulletGo.transform.rotation = barrelPos.transform.rotation; // �Ѿ��� ȸ���� �ѱ��� ȸ������ ����

        TrailRenderer trailRenderer = bulletGo.GetComponent<TrailRenderer>();

        // �Ѿ��� Rigidbody �ʱ�ȭ
        Rigidbody rb = bulletGo.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(barrelPos.transform.forward * bulletVelocity, ForceMode.Impulse); // �Ѿ˿� ���� ���� �߻�
        trailRenderer.Clear();
    }
}


