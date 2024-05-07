using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public GameObject zombie;

    // Start is called before the first frame update
    private void Start()
    {
        setRigidbodyState(true);
        setColliderState(true);
    }
    public void setRigidbodyState(bool state)
    {
        Rigidbody[] ragdollRigs = zombie.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in ragdollRigs)
        {
            rb.isKinematic = state;
        }
    }
    public void setColliderState(bool state)
    {
        Collider [] ragdollCols = zombie.GetComponentsInChildren<Collider>();
        foreach (Collider col in ragdollCols)
        {
            col.enabled = state;
        }
    }
}
