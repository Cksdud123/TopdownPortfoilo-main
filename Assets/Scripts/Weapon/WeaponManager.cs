using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate; // 발사 속도 (초당 발사 가능한 횟수)
    [SerializeField] bool semiAuto; // 반자동 발사 여부
    float fireRateTimer; // 발사 속도 타이머

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet; // 발사할 총알 프리팹
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

        if (ammo.currentAmmo == 0)
        {
            Debug.Log("현재 남은 총알이 없습니다.");
        }
        Debug.Log(ammo.currentAmmo + "발사!!");

        AmmoUI.instance.UpdateAmmoText(ammo.currentAmmo);
        AmmoUI.instance.UpdateMagText(ammo.extraAmmo);

        TriggerMuzzleFlash(); // 총구 화염 효과 실행
        recoil.TriggerRecoil();

        for (int i = 0; i < bulletsPerShot; i++) // 발사할 총알 개수만큼 반복
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation); // 총알 생성

            Bullet bulletScript = currentBullet.GetComponent<Bullet>(); // 총알 스크립트 가져오기
            bulletScript.weapon = this; // 총기 정보 설정

            bulletScript.dir = barrelPos.transform.forward; // 총알 방향 설정

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>(); // 총알 Rigidbody 컴포넌트 가져오기
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse); // 총알에 힘을 가해 발사
        }
    }
    private void TriggerMuzzleFlash()
    {
        muzzleFlash.Play();
    }
}
