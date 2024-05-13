using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create new Item")]
[System.Serializable]
public class Items : ScriptableObject
{
    public int id;
    public string itemName;

    [TextArea(3, 3)] public string description;

    public enum ItemsTypes
    {
        Ammo,
        Healing
    }

    public GameObject prefab;
    public Texture icon;

    public ItemsTypes types;
    public int maxStack;
    public float weight;
    public int baseValue;
}
