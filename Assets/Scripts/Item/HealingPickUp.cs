using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPickUp : PoolAble
{
    public float RotationSpeed = 50.0f;
    // Update is called once per frame
    private void Start()
    {
        Invoke("DestroyHealing", Random.Range(5.0f, 15.0f));
    }
    public void DestroyHealing()
    {
        ReleaseObject();
    }

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

        if (playerHealth.health == playerHealth.maxHealth)
        {
            return;
        }
        else
        {
            playerHealth.health += Random.Range(1.0f, 20.0f);

            gameObject.SetActive(false);
        }
        // ¶Ç´Â Destroy(gameObject);
    }
}
