using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = 
    "ScriptableObjects/CollectableType")]
public class CollectableTypeSO : ScriptableObject
{
    public string collectableName;
    public Transform prefab;
    public int amountSpawned;
}
