using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeallingToInfection : MonoBehaviour
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

        if (playerHealth.infection == playerHealth.maxinfection)
        {
            return;
        }
        else
        {
            playerHealth.infection -= Random.Range(2.0f, 7.0f);

            gameObject.SetActive(false);
        }
    }
}
