using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectableSpawnManager : MonoBehaviour
{

    [SerializeField] private int sphereAmount;
    [SerializeField] private int cubeAmount;
    [SerializeField] private int capsuleAmount;

    [SerializeField] private List<Transform> spawnTransformList;

    private List<Vector3> spawnPositionsList;
    private List<CollectableTypeSO> collectableTypeList;

    private void Awake()
    {
        collectableTypeList = Resources.Load<CollectableTypeListSO>("CollectableTypeListSO").list;
        spawnPositionsList = new List<Vector3>();

        foreach (Transform spawnTransform in spawnTransformList)
        {
            spawnPositionsList.Add(spawnTransform.position);
        }

        // overwrites the value in the SO
        collectableTypeList[0].amountSpawned = sphereAmount;
        collectableTypeList[1].amountSpawned = cubeAmount;
        collectableTypeList[2].amountSpawned = capsuleAmount;

        // randomize placement of collectables
        List<Vector3> shuffledSpawnPositions = spawnPositionsList.OrderBy(x => Random.value).ToList();

        int spawnIndex = 0;
        foreach (var item in collectableTypeList)
        {
            for (int i = 0; i < item.amountSpawned; i++)
            {
                if (spawnIndex >= shuffledSpawnPositions.Count)
                    break;

                Transform collectableTransform = Instantiate(item.prefab, shuffledSpawnPositions[spawnIndex], Quaternion.identity);
                spawnIndex++;
            }
        }
        CollectableCounter.Instance.SetCollectablesTotalAmount(spawnIndex);
    }
}
