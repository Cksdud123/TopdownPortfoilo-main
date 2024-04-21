using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int clipSize; // 장전할 탄창의 크기
    public int extraAmmo; // 추가 탄약
    [HideInInspector] public int currentAmmo; // 현재 탄약 수

    //public AudioClip magInSound; // 탄창 장전 사운드
    //public AudioClip magOutSound; // 탄창 탈착 사운드
    //public AudioClip releaseSlideSound; // 슬라이드 해제 사운드

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = clipSize; // 시작할 때 현재 탄약을 탄창 크기로 초기화
    }

    // 탄약을 재장전하는 메서드
    public void Reload()
    {
        if (extraAmmo >= clipSize) // 추가 탄약이 탄창 크기 이상이면
        {
            int ammoToReload = clipSize - currentAmmo; // 재장전할 탄약 계산
            extraAmmo -= ammoToReload; // 추가 탄약에서 재장전한 탄약 수 차감
            currentAmmo += ammoToReload; // 현재 탄약에 재장전한 탄약 수 추가
        }
        else if (extraAmmo > 0) // 추가 탄약이 있고 탄창 크기보다 작으면
        {
            if (extraAmmo + currentAmmo > clipSize) // 추가 탄약과 현재 탄약을 합친 수가 탄창 크기를 초과하면
            {
                int leftOverAmmo = extraAmmo + currentAmmo - clipSize; // 초과된 탄약 수 계산
                extraAmmo = leftOverAmmo; // 추가 탄약에 초과된 탄약 수 할당
                currentAmmo = clipSize; // 현재 탄약을 탄창 크기로 설정
            }
            else // 추가 탄약이 탄창 크기 이하이면
            {
                currentAmmo += extraAmmo; // 현재 탄약에 추가 탄약 전부 추가
                extraAmmo = 0; // 추가 탄약 초기화
            }
        }
    }
}
