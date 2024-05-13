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
        // ���Կ� �������� ���� ��� �巡�׸� �����մϴ�.
        if (transform.GetComponent<Slot>().ItemInSlot == null)
            return;

        // �巡�� ������ �α׿� ����մϴ�.
        print("Begin Drag");

        // �巡�� ���� �������� �θ� �����մϴ�.
        draggedItemParent = transform;

        // �巡�� ���� �������� Transform�� ������ �����մϴ�.
        draggedItem = draggedItemParent.GetComponentInChildren<RawImage>().transform;

        // �巡�� ���� �������� ĵ������ �ڽ����� �����մϴ�.
        draggedItem.SetParent(FindObjectOfType<Canvas>().transform, worldPositionStays: false);

        // �巡�� ���� �������� �ؽ�Ʈ�� ��Ȱ��ȭ�մϴ�.
        draggedItemParent.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
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
        // �巡�� ���� �������� ���� ������ �ڽ����� �ٽ� �����մϴ�.
        draggedItem.SetParent(draggedItemParent);

        // �巡�� ���� �������� ���� ��ġ�� �ʱ�ȭ�մϴ�.
        draggedItem.localPosition = Vector3.zero;

        // �巡�� ���� �������� RawImage ������Ʈ�� �ٽ� ����ĳ��Ʈ ������� �����մϴ�.
        draggedItem.GetComponent<RawImage>().raycastTarget = true;

        // ������ �ؽ�Ʈ�� �ٽ� Ȱ��ȭ�մϴ�.
        draggedItemParent.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

        // ������ UI�� ������Ʈ�մϴ�.
        draggedItemParent.GetComponent<Slot>().SetStats();
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