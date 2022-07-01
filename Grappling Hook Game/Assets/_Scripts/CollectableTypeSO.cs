using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = 
    "ScriptableObjects/CollectableType")]
public class CollectableTypeSO : ScriptableObject
{
    public string collectableName; // to present to the player upon collection of the item
    public int idNumber; // this will be used to determine if the player has all the collectables and which ones they are missing
}
