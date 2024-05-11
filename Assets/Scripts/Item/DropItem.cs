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
        public float DropRate; // 스폰 확률 (0부터 1 사이의 값)
    }
    [SerializeField] private List<ItemData> _ItemDropData;

    private float _sumOfDropChances; // 모든 적 스폰 확률의 합
    [SerializeField][Range(0, 1)] private float _notDropProbability = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _sumOfDropChances = 1f - _notDropProbability;

        // 모든 적 스폰 확률의 합을 계산하고 정규화합니다.
        foreach (ItemData item in _ItemDropData)
        {
            _sumOfDropChances += item.DropRate;
        }
        NormalizeSpawnRates(); // 스폰 확률을 정규화합니다.
    }
    public void Item()
    {
        float randomValue = Random.value;

        if (randomValue <= _notDropProbability)
        {
            Debug.Log("아이템을 드롭하지 않았습니다.");
            return;
        }

        float adjustedRandomValue = (randomValue - _notDropProbability) / (1f - _notDropProbability); // 아이템 드롭 확률을 정규화

        float cumulativeProbability = 0f;

        // 조정된 확률에 따라 아이템을 드롭합니다.
        foreach (ItemData item in _ItemDropData)
        {
            cumulativeProbability += item.DropRate;

            if (adjustedRandomValue <= cumulativeProbability)
            {
                // 아이템을 드롭합니다.
                ItemSpawn(item.Item);
                Debug.Log("아이템을 드롭했습니다! 아이템 이름: " + item.Item.name);
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
