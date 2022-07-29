using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectableSpawnManager : MonoBehaviour
{
	[SerializeField] private List<Transform> spawnTransformList;

	private List<Vector3> spawnPositionsList;


	private void Awake()
    {        
        spawnPositionsList = new List<Vector3>();
        foreach (Transform spawnTransform in spawnTransformList)
        {
            spawnPositionsList.Add(spawnTransform.position);
        }

        List<Vector3> shuffledSpawnPositions = ShufflePositionsList(spawnPositionsList);
        SpawnCollectables(shuffledSpawnPositions);
    }


    private List<Vector3> ShufflePositionsList(List<Vector3> spawnPositionsList)
    {
        return spawnPositionsList.OrderBy(x => Random.value).ToList();
    }


    private void SpawnCollectables(List<Vector3> shuffledSpawnPositions)
    {
        int spawnIndex = 0;
        List<CollectableTypeSO> collectableTypeList = Resources.Load<CollectableTypeListSO>("CollectableTypeListSO").list;
        
        foreach (CollectableTypeSO item in collectableTypeList)
        {
            for (int i = 0; i < item.amountSpawned; i++)
            {
                if (spawnIndex >= shuffledSpawnPositions.Count)
                {
                    Debug.Log("Not enough spawn points");
                    break;
                }

                Transform collectableTransform = Instantiate(item.prefab, shuffledSpawnPositions[spawnIndex], Quaternion.identity);
                spawnIndex++;
            }
        }
        CollectableCounter.Instance.CollectablesTotalAmount = spawnIndex;
    }    
}
