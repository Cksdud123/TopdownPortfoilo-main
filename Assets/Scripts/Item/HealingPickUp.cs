using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPickUp : MonoBehaviour
{
    public float RotationSpeed = 50.0f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Health playerHealth = other.GetComponent<Health>();
        playerHealth.health += Random.Range(1.0f, 20.0f);

        gameObject.SetActive(false);
        // ¶Ç´Â Destroy(gameObject);
    }
}
