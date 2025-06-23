using UnityEngine;
using System.Collections.Generic;

public class BinaSpawner : MonoBehaviour
{
    [Header("Bina Ayarları")]
    public GameObject binaPrefab;
    public float spawnSpacing = 20f;
    public float destroyDistance = 30f;
    public int initialSpawnCount = 10;      // EDITÖRDEN AYARLANIR


    [Header("Spawn Hassasiyeti")]
    public float spawnAheadDistance = 30f;

    [Header("Yerleşim")]
    public float xOffset = 6f;

    [Header("Araba Takibi")]
    public Transform car;

    private float nextSpawnZ;

    private List<GameObject> activeBuildingsLeft = new List<GameObject>();
    private List<GameObject> activeBuildingsRight = new List<GameObject>();

    void Start()
    {
        // Arabanın Z'sinden başlama — onun önünden başla
        nextSpawnZ = car.position.z - spawnSpacing; // 1 spacing önünden başla

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnNext();
        }
    }

    void Update()
    {
        // Araba bu kadar yaklaşmadan önce spawn yapılır
        if (car.position.z < nextSpawnZ + spawnAheadDistance)
        {
            SpawnNext();
        }

        CleanupBuildings(activeBuildingsLeft);
        CleanupBuildings(activeBuildingsRight);
    }

    void SpawnNext()
    {
        Vector3 leftPos = new Vector3(-xOffset, 0f, nextSpawnZ);
        Vector3 rightPos = new Vector3(xOffset, 0f, nextSpawnZ);

        GameObject left = Instantiate(binaPrefab, leftPos, Quaternion.identity);
        GameObject right = Instantiate(binaPrefab, rightPos, Quaternion.identity);

        activeBuildingsLeft.Add(left);
        activeBuildingsRight.Add(right);

        nextSpawnZ -= spawnSpacing; // Z ↓ yönünde spawn devam eder
    }

    void CleanupBuildings(List<GameObject> buildingList)
    {
        for (int i = buildingList.Count - 1; i >= 0; i--)
        {
            float distance = Mathf.Abs(car.position.z - buildingList[i].transform.position.z);
            if (distance > destroyDistance)
            {
                Destroy(buildingList[i]);
                buildingList.RemoveAt(i);
            }
        }
    }
}
