using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Burst.Intrinsics;


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
            //GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation); // �Ѿ� ����
            var bullet = bulletPool.Get();

            Bullet bulletScript = bullet.GetComponent<Bullet>(); // �Ѿ� ��ũ��Ʈ ��������
            bulletScript.weapon = this; // �ѱ� ���� ����

            bulletScript.dir = barrelPos.transform.forward; // �Ѿ� ���� ����

            Rigidbody rb = bullet.GetComponent<Rigidbody>(); // �Ѿ� Rigidbody ������Ʈ ��������
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse); // �Ѿ˿� ���� ���� �߻�
            Debug.Log("bullet ��ġ" + bullet.transform.position);
        }
    }
    private void TriggerMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    // ������Ʈ Ǯ
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefabs,barrelPos.position,barrelPos.rotation).GetComponent<Bullet>();
        bullet.SetManagedPool(bulletPool);
        return bullet;
    }
    private void OnGetBullet(Bullet bullet)
    {
        bullet.transform.SetParent(null); // ������ �θ� �������� �и�
        bullet.transform.position = barrelPos.position; // �Ѿ��� ��ġ�� �ѱ��� ��ġ�� ����
        bullet.transform.rotation = barrelPos.rotation; // �Ѿ��� ȸ���� �ѱ��� ȸ������ ����

        // �Ѿ��� Rigidbody �ʱ�ȭ
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
        bullet.gameObject.SetActive(false); // ���� ������Ʈ ��Ȱ��ȭ
    }
    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
