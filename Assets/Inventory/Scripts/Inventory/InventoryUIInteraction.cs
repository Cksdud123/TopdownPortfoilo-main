using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIInteraction : MonoBehaviour ,IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] GameObject ClickedItemUI; // 클릭된 아이템 정보 UI GameObject

    public Transform draggedItemParent; // 드래그 중인 아이템의 부모 Transform
    public Transform draggedItem; // 드래그 중인 아이템의 Transform

    // 드래그 시작 시 호출되는 메서드, IBeginDragHandler 
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.GetComponent<Slot>().ItemInSlot == null)
            return;
        print("Begin Drag");
        draggedItemParent = transform;
        draggedItem = draggedItemParent.GetComponentInChildren<RawImage>().transform;
        draggedItemParent.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        draggedItem.parent = FindObjectOfType<Canvas>().transform;
        draggedItem.SetAsLastSibling();
    }

    // 드래그 중일 때 호출되는 메서드, IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        draggedItem.position = Input.mousePosition;
        draggedItem.GetComponent<RawImage>().raycastTarget = false;
    }

    // 드래그 종료 시 호출되는 메서드, IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        draggedItem.parent = draggedItemParent;
        draggedItem.localPosition = new Vector3(0, 0, 0);
        draggedItem.GetComponent<RawImage>().raycastTarget = true;
        draggedItemParent.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        draggedItemParent.GetComponent<Slot>().SetStats();
        draggedItem = null;
        draggedItemParent = null;
        print("End Drag");
    }

    // 마우스 클릭 시 호출되는 메서드, IPointerClickHandler 
    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭된 슬롯에 아이템이 없거나 이미 클릭된 아이템 정보 UI가 활성화된 경우 처리를 종료합니다.
        if (eventData.pointerClick.GetComponent<Slot>().ItemInSlot == null || ClickedItemUI.activeInHierarchy)
            return;

        // 클릭된 아이템 정보 UI의 위치를 마우스 위치로 설정합니다.
        ClickedItemUI.transform.position = Input.mousePosition;
        /*+ new Vector3(ClickedItemUI.GetComponent<RectTransform>().rect.width * 1.5f / 2 + 1, 
                    -(ClickedItemUI.GetComponent<RectTransform>().rect.height * 1.5f / 2 - 1),
                      0);*/

        // 클릭된 아이템 정보 UI에 클릭된 슬롯 정보를 전달합니다.
        //ClickedItemUI.GetComponent<ClickedItem>().clickedSlot = eventData.pointerClick.GetComponent<Slot>();

        // 클릭된 아이템 정보 UI를 활성화합니다.
        //ClickedItemUI.SetActive(true);
    }

    // 마우스가 슬롯에서 벗어날 때 호출되는 메서드, IPointerExitHandler 
    public void OnPointerExit(PointerEventData eventData)
    {
        ClickedItemUI.SetActive(false);
    }
}