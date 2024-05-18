using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeallingToStamina : MonoBehaviour
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

        if (playerHealth.stamina == playerHealth.maxStamina)
        {
            return;
        }
        else
        {
            playerHealth.stamina += Random.Range(10.0f, 30.0f);

            gameObject.SetActive(false);
        }
    }
}
