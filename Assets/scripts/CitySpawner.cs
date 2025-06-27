using System.Collections.Generic;
using UnityEngine;

public class CitySpawner : MonoBehaviour
{
    public Transform player;
    public GameObject[] segmentPrefabs; // Artık dizi oldu
    public int visibleSegments = 5;
    public float segmentLength = 100f;

    private float spawnZ = 0f;
    private List<GameObject> spawnedSegments = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < visibleSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        if (player.position.z < spawnZ + visibleSegments * segmentLength)
        {
            SpawnSegment();
        }

        if (spawnedSegments.Count > 0)
        {
            GameObject firstSegment = spawnedSegments[0];
            float firstSegmentEndZ = firstSegment.transform.position.z - segmentLength;

            if (player.position.z < firstSegmentEndZ)
            {
                RemoveOldestSegment();
            }
        }
    }

    void SpawnSegment()
    {
        // RANDOM prefab seç
        int index = Random.Range(0, segmentPrefabs.Length);
        GameObject prefabToSpawn = segmentPrefabs[index];

        GameObject segment = Instantiate(prefabToSpawn, new Vector3(0, 0, spawnZ), Quaternion.identity);
        spawnedSegments.Add(segment);
        spawnZ -= segmentLength;
    }

    void RemoveOldestSegment()
    {
        Destroy(spawnedSegments[0]);
        spawnedSegments.RemoveAt(0);
    }
}
