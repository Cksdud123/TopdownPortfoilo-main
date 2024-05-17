using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AttackScripts : MonoBehaviour
{
    public float damage = 0f;
    public float infection = 0f;
    public float radius = 1f;
    public LayerMask layerMask;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
            hits[0].gameObject.GetComponent<Health>().ApplyDamage(damage + Random.Range(1.0f, 5.0f), infection + Random.Range(1.0f, 15.0f));

            gameObject.SetActive(false);

        }

    }
}
