using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        inventory.PickUpItem(other.GetComponent<ItemObject>());
        other.gameObject.SetActive(false);
    }
}
