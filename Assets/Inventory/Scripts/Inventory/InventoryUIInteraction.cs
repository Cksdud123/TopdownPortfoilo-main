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
        // 슬롯에 아이템이 없는 경우 드래그를 중지합니다.
        if (transform.GetComponent<Slot>().ItemInSlot == null)
            return;

        // 드래그 시작을 로그에 출력합니다.
        print("Begin Drag");

        // 드래그 중인 아이템의 부모를 설정합니다.
        draggedItemParent = transform;

        // 드래그 중인 아이템의 Transform을 가져와 저장합니다.
        draggedItem = draggedItemParent.GetComponentInChildren<RawImage>().transform;

        // 드래그 중인 아이템을 캔버스의 자식으로 설정합니다.
        draggedItem.SetParent(FindObjectOfType<Canvas>().transform, worldPositionStays: false);

        // 드래그 중인 아이템의 텍스트를 비활성화합니다.
        draggedItemParent.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
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
        // 드래그 중인 아이템을 원래 슬롯의 자식으로 다시 설정합니다.
        draggedItem.SetParent(draggedItemParent);

        // 드래그 중인 아이템의 로컬 위치를 초기화합니다.
        draggedItem.localPosition = Vector3.zero;

        // 드래그 중인 아이템의 RawImage 컴포넌트를 다시 레이캐스트 대상으로 설정합니다.
        draggedItem.GetComponent<RawImage>().raycastTarget = true;

        // 슬롯의 텍스트를 다시 활성화합니다.
        draggedItemParent.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

        // 슬롯의 UI를 업데이트합니다.
        draggedItemParent.GetComponent<Slot>().SetStats();
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