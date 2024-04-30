using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    Rigidbody[] ragdollRigs;
    Collider[] colliders;

    // Start is called before the first frame update
    void Awake()
    {
        ragdollRigs = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }
    public void setRigidbodyState(bool state)
    {
        foreach (Rigidbody rb in ragdollRigs)
        {
            rb.angularDrag = 0.5f; // ȸ�� ���� ����
            rb.drag = 0.5f; // �̵� ���� ����
            rb.isKinematic = state;
            rb.detectCollisions = !state; // �浹 ���� Ȱ��ȭ/��Ȱ��ȭ
        }
    }

    public void setColliderState(bool state)
    {
        foreach (Collider col in colliders)
        {
            col.enabled = state;
        }
    }
}
