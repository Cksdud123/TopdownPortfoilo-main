using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotsHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    public List<SlotClass> items = new List<SlotClass>();

    private GameObject[] slots;
    public void Start()
    {
        slots = new GameObject[slotsHolder.transform.childCount];
        
        // 슬롯 초기화
        for(int i = 0; i < slotsHolder.transform.childCount; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
        Add(itemToAdd);
        Remove(itemToRemove);
    }

    public void RefreshUI()
    {
        for(int i = 0;i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if(items[i].GetItem().isStackable)
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity().ToString() + "";
                else
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }

    public bool Add(ItemClass item)
    {
        //items.Add(item);


        SlotClass slot = Contains(item);
        // 수량체크를해서 isStackable이 true일때는 1개만 쌓고
        if (slot != null && slot.GetItem().isStackable) slot.AddQuantity(1);
        // 그렇지 않으면 계산해서 쌓음
        else
        {
            if (items.Count < slots.Length)
                items.Add(new SlotClass(item, 1));
            else
                return false;
        }
        RefreshUI();
        return true;
    }
    public bool Remove(ItemClass item)
    {
        //items.Remove(item);
        SlotClass slotToRemove = new SlotClass();

        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if(temp.GetQuantity() > 1)
                temp.SubQuantity(1);
            else
            {
                foreach (SlotClass slot in items)
                {
                    if (slot.GetItem() == item)
                    {
                        slotToRemove = slot;
                        break;
                    }
                }
                items.Remove(slotToRemove);

            }
        }
        else
        {
            return false;
        }
       
        RefreshUI();
        return true;
    }
    public SlotClass Contains(ItemClass item)
    {
        foreach(SlotClass slot in items)
        {
            if(slot.GetItem() == item) return slot;
        }
        return null;
    }
}
