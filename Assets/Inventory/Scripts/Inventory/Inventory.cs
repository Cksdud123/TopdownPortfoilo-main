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
        // ���� Ȱ��ȭ �������� ���θ�ŭ ��Ȱ��ȭ �Ѵ�.
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
        // i�� ������ �κ��丮�� ���� ũ�ν��� ��Ȱ��ȭ
        if (!InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.I))
        {
            CrossHair.SetActive(false);
            Cursor.visible = true;
            InventoryUI.SetActive(true);
        }
        // i�� ������ �κ��丮�� �ݰ� ũ�ν��� Ȱ��ȭ
        else if (InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            CrossHair.SetActive(true);
            Cursor.visible = false;
            InventoryUI.SetActive(false);
        }
    }
    public void PickUpItem(ItemObject obj)
    {
        // ������ ���̸�ŭ
        for (int i = 0; i < slots.Length; i++)
        {
            // ���� �ƴϰ� ������ ���̵�� ������Ʈ�� ���̵� ���� �ִ�����뷮�� �ƴҶ�
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
