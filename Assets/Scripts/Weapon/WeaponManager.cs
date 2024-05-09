using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using UnityEditor.PackageManager;

public enum Weapon
{
    Rifle,
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
    public float damage = 20; // �Ѿ� ������

    [SerializeField] public WeaponAmmo ammo; // �Ѿ� �ܿ��� ���� Ŭ����

    ParticleSystem muzzleFlash;

    private ActionStateManager actions;
    WeaponClassManager weaponClass;
    WeaponRecoil recoil;

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

        if (weaponState == Weapon.Rifle)
        {
            if (ShouldFire()) Fire();
        }
        else if(weaponState == Weapon.Knife)
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

        TriggerMuzzleFlash(); // �ѱ� ȭ�� ȿ�� ����
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
            Enemy enemy = hitInfo.transform.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);

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
