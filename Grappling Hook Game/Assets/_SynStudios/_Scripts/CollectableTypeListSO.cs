using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/CollectableTypeList")]
public class CollectableTypeListSO : ScriptableObject
{
    public List<CollectableTypeSO> list;
}
