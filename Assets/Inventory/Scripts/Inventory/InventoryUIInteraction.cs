using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIInteraction : MonoBehaviour ,IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] GameObject ClickedItemUI; // Ŭ���� ������ ���� UI GameObject

    public Transform draggedItemParent; // �巡�� ���� �������� �θ� Transform
    public Transform draggedItem; // �巡�� ���� �������� Transform

    // �巡�� ���� �� ȣ��Ǵ� �޼���, IBeginDragHandler 
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

    // �巡�� ���� �� ȣ��Ǵ� �޼���, IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        draggedItem.position = Input.mousePosition;
        draggedItem.GetComponent<RawImage>().raycastTarget = false;
    }

    // �巡�� ���� �� ȣ��Ǵ� �޼���, IEndDragHandler
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

    // ���콺 Ŭ�� �� ȣ��Ǵ� �޼���, IPointerClickHandler 
    public void OnPointerClick(PointerEventData eventData)
    {
        // Ŭ���� ���Կ� �������� ���ų� �̹� Ŭ���� ������ ���� UI�� Ȱ��ȭ�� ��� ó���� �����մϴ�.
        if (eventData.pointerClick.GetComponent<Slot>().ItemInSlot == null || ClickedItemUI.activeInHierarchy)
            return;

        // Ŭ���� ������ ���� UI�� ��ġ�� ���콺 ��ġ�� �����մϴ�.
        ClickedItemUI.transform.position = Input.mousePosition;
        /*+ new Vector3(ClickedItemUI.GetComponent<RectTransform>().rect.width * 1.5f / 2 + 1, 
                    -(ClickedItemUI.GetComponent<RectTransform>().rect.height * 1.5f / 2 - 1),
                      0);*/

        // Ŭ���� ������ ���� UI�� Ŭ���� ���� ������ �����մϴ�.
        //ClickedItemUI.GetComponent<ClickedItem>().clickedSlot = eventData.pointerClick.GetComponent<Slot>();

        // Ŭ���� ������ ���� UI�� Ȱ��ȭ�մϴ�.
        //ClickedItemUI.SetActive(true);
    }

    // ���콺�� ���Կ��� ��� �� ȣ��Ǵ� �޼���, IPointerExitHandler 
    public void OnPointerExit(PointerEventData eventData)
    {
        ClickedItemUI.SetActive(false);
    }
}