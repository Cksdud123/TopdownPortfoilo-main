using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public Transform Player;

    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Player == null)
            return;

        // 마우스 입력값을 받아와 사용자의 위치를 월드내에서 좌표로 변환후 LookAt2D함수를 실행
        transform.position = Input.mousePosition;
        Vector3 playerPos2D = Camera.main.WorldToScreenPoint(Player.position);
        LookAt2D(playerPos2D);
    }
    // CrossHair가 입력된 마우스값을 따라다닐 수 있게 설정함
    private void LookAt2D(Vector3 lookAtPosition)
    {
        Vector3 dir = lookAtPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}
