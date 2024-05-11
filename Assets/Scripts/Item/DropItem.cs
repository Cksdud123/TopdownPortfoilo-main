using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DropItem : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public ItemData(GameObject item, float dropRate)
        {
            this.Item = item;
            this.DropRate = dropRate;
        }
        public GameObject Item;
        [Range(0, 1)]
        public float DropRate; // ���� Ȯ�� (0���� 1 ������ ��)
    }
    [SerializeField] private List<ItemData> _ItemDropData;

    private float _sumOfDropChances; // ��� �� ���� Ȯ���� ��
    [SerializeField][Range(0, 1)] private float _notDropProbability = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _sumOfDropChances = 1f - _notDropProbability;

        // ��� �� ���� Ȯ���� ���� ����ϰ� ����ȭ�մϴ�.
        foreach (ItemData item in _ItemDropData)
        {
            _sumOfDropChances += item.DropRate;
        }
        NormalizeSpawnRates(); // ���� Ȯ���� ����ȭ�մϴ�.
    }
    public void Item()
    {
        float randomValue = Random.value;

        if (randomValue <= _notDropProbability)
        {
            Debug.Log("�������� ������� �ʾҽ��ϴ�.");
            return;
        }

        float adjustedRandomValue = (randomValue - _notDropProbability) / (1f - _notDropProbability); // ������ ��� Ȯ���� ����ȭ

        float cumulativeProbability = 0f;

        // ������ Ȯ���� ���� �������� ����մϴ�.
        foreach (ItemData item in _ItemDropData)
        {
            cumulativeProbability += item.DropRate;

            if (adjustedRandomValue <= cumulativeProbability)
            {
                // �������� ����մϴ�.
                ItemSpawn(item.Item);
                Debug.Log("�������� ����߽��ϴ�! ������ �̸�: " + item.Item.name);
                return;
            }
        }
    }

    public void ItemSpawn(GameObject Item)
    {
        var ItemGo = ObjectPoolingManager.instance.GetGo(Item.name);
        ItemGo.transform.position = transform.position;
        ItemGo.transform.rotation = Quaternion.identity;
    }
    private void NormalizeSpawnRates()
    {
        if (_sumOfDropChances == 0)
        {
            return;
        }
        foreach (ItemData item in _ItemDropData)
        {
            item.DropRate /= _sumOfDropChances;
        }
    }
}
