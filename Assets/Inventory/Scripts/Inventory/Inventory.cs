using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public Slot[] slots = new Slot[40];
    [SerializeField] GameObject InventoryUI;

    public GameObject CrossHair;

    // Start is called before the first frame update
    void Start()
    {
        // 슬롯 활성화 아이템이 널인만큼 비활성화 한다.
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot == null)
            {
                for (int k = 0; k < slots[i].transform.childCount; k++)
                {
                    slots[i].transform.GetChild(k).gameObject.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // i를 누르면 인벤토리를 열고 크로스헤어를 비활성화
        if (!InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.I))
        {
            CrossHair.SetActive(false);
            Cursor.visible = true;
            InventoryUI.SetActive(true);
        }
        // i를 누르면 인벤토리를 닫고 크로스헤어를 활성화
        else if (InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            CrossHair.SetActive(true);
            Cursor.visible = false;
            InventoryUI.SetActive(false);
        }
    }
    public void PickUpItem(ItemObject obj)
    {
        // 슬롯의 길이만큼
        for (int i = 0; i < slots.Length; i++)
        {
            // 널이 아니고 슬롯의 아이디와 오브젝트의 아이디가 같고 최대저장용량이 아닐때
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.id == obj.itemStats.id && slots[i].AmountInSlot != slots[i].ItemInSlot.maxStack)
            {
                // 
                if (!WillHitMaxStack(i, obj.amount))
                {
                    slots[i].AmountInSlot += obj.amount;
                    Destroy(obj.gameObject);
                    slots[i].SetStats();
                    return;
                }
                else
                {
                    int result = NeededToFill(i);
                    obj.amount = RemainingAmount(i, obj.amount);
                    slots[i].AmountInSlot += result;
                    slots[i].SetStats();
                    PickUpItem(obj);
                    return;
                }
            }
            else if (slots[i].ItemInSlot == null)
            {
                slots[i].ItemInSlot = obj.itemStats;
                slots[i].AmountInSlot += obj.amount;
                Destroy(obj.gameObject);
                slots[i].SetStats();
                return;
            }
        }
    }

    bool WillHitMaxStack(int index, int amount)
    {
        if (slots[index].ItemInSlot.maxStack <= slots[index].AmountInSlot + amount)
            return true;
        else
            return false;
    }

    int NeededToFill(int index)
    {
        return slots[index].ItemInSlot.maxStack - slots[index].AmountInSlot;
    }
    int RemainingAmount(int index, int amount)
    {
        return (slots[index].AmountInSlot + amount) - slots[index].ItemInSlot.maxStack;
    }
}
