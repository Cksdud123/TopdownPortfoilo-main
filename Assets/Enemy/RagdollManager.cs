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
            rb.isKinematic = state;
            rb.detectCollisions = !state; // 충돌 감지 활성화/비활성화
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
