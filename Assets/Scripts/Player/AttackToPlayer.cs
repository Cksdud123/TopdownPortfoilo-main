using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class AttackToPlayer : MonoBehaviour
{
    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {

            hits[0].gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(damage);

            var bloodEffectGo = ObjectPoolingManager.instance.GetGo("BloodEffect");
            bloodEffectGo.transform.position = hits[0].transform.position;
            bloodEffectGo.transform.rotation = Quaternion.identity;

            gameObject.SetActive(false);

        }

    }
}
