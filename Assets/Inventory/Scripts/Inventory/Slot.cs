using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public Items ItemInSlot;
    public int AmountInSlot;

    RawImage icon;
    TextMeshProUGUI txt_amount;

    public void SetStats()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();

        if (ItemInSlot == null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            return;
        }


        icon.texture = ItemInSlot.icon;
        txt_amount.text = $"{AmountInSlot}x";

        // 텍스트가 아이콘 위에 렌더링되도록 설정
        txt_amount.transform.SetAsLastSibling();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryUIInteraction draggableItem = dropped.GetComponent<InventoryUIInteraction>();
        Slot slot = draggableItem.draggedItemParent.GetComponent<Slot>();

        if (slot == this)
            return;

        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;

        slot.ItemInSlot = null;
        slot.AmountInSlot = 0;

        SetStats();
    }
}
