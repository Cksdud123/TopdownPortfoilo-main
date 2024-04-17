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

        // ���콺 �Է°��� �޾ƿ� ������� ��ġ�� ���峻���� ��ǥ�� ��ȯ�� LookAt2D�Լ��� ����
        transform.position = Input.mousePosition;
        Vector3 playerPos2D = Camera.main.WorldToScreenPoint(Player.position);
        LookAt2D(playerPos2D);
    }
    // CrossHair�� �Էµ� ���콺���� ����ٴ� �� �ְ� ������
    private void LookAt2D(Vector3 lookAtPosition)
    {
        Vector3 dir = lookAtPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}
