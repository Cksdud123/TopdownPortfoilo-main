using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Burst.Intrinsics;
using UnityEngine.UIElements;


public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate; // �߻� �ӵ� (�ʴ� �߻� ������ Ƚ��)
    [SerializeField] bool semiAuto; // ���ڵ� �߻� ����
    float fireRateTimer; // �߻� �ӵ� Ÿ�̸�

    [Header("Bullet Properties")]
    [SerializeField] GameObject bulletPrefabs; // �߻��� �Ѿ� ������
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

        TriggerMuzzleFlash(); // �ѱ� ȭ�� ȿ�� ����
        recoil.TriggerRecoil();

        for (int i = 0; i < bulletsPerShot; i++) // �߻��� �Ѿ� ������ŭ �ݺ�
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
        //GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation); // �Ѿ� ����

        var bulletGo = ObjectPoolingManager.instance.GetGo("Bullet");

        bulletGo.transform.position = barrelPos.position; // �Ѿ��� ��ġ�� �ѱ��� ��ġ�� ����
        bulletGo.transform.rotation = barrelPos.rotation; // �Ѿ��� ȸ���� �ѱ��� ȸ������ ����

        // �Ѿ��� Rigidbody �ʱ�ȭ
        Rigidbody rb = bulletGo.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Bullet bulletScript = bulletGo.GetComponent<Bullet>(); // �Ѿ� ��ũ��Ʈ ��������
        bulletScript.weapon = this; // �ѱ� ���� ����

        bulletScript.dir = barrelPos.transform.forward; // �Ѿ� ���� ����

        rb = bulletGo.GetComponent<Rigidbody>(); // �Ѿ� Rigidbody ������Ʈ ��������
        rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse); // �Ѿ˿� ���� ���� �߻�
    }
}
