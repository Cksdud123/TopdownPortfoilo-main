using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int clipSize; // ������ źâ�� ũ��
    public int extraAmmo; // �߰� ź��
    [HideInInspector] public int currentAmmo; // ���� ź�� ��

    //public AudioClip magInSound; // źâ ���� ����
    //public AudioClip magOutSound; // źâ Ż�� ����
    //public AudioClip releaseSlideSound; // �����̵� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = clipSize; // ������ �� ���� ź���� źâ ũ��� �ʱ�ȭ
    }

    // ź���� �������ϴ� �޼���
    public void Reload()
    {
        if (extraAmmo >= clipSize) // �߰� ź���� źâ ũ�� �̻��̸�
        {
            int ammoToReload = clipSize - currentAmmo; // �������� ź�� ���
            extraAmmo -= ammoToReload; // �߰� ź�࿡�� �������� ź�� �� ����
            currentAmmo += ammoToReload; // ���� ź�࿡ �������� ź�� �� �߰�
        }
        else if (extraAmmo > 0) // �߰� ź���� �ְ� źâ ũ�⺸�� ������
        {
            if (extraAmmo + currentAmmo > clipSize) // �߰� ź��� ���� ź���� ��ģ ���� źâ ũ�⸦ �ʰ��ϸ�
            {
                int leftOverAmmo = extraAmmo + currentAmmo - clipSize; // �ʰ��� ź�� �� ���
                extraAmmo = leftOverAmmo; // �߰� ź�࿡ �ʰ��� ź�� �� �Ҵ�
                currentAmmo = clipSize; // ���� ź���� źâ ũ��� ����
            }
            else // �߰� ź���� źâ ũ�� �����̸�
            {
                currentAmmo += extraAmmo; // ���� ź�࿡ �߰� ź�� ���� �߰�
                extraAmmo = 0; // �߰� ź�� �ʱ�ȭ
            }
        }
    }
}
