using System.Collections.Generic;
using UnityEngine;

public class CitySpawner : MonoBehaviour
{
    public Transform player;
    public GameObject segmentPrefab;
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
        // Yeni segment spawn etme kontrolü
        if (player.position.z < spawnZ + visibleSegments * segmentLength)
        {
            SpawnSegment();
        }

        // Segment silme kontrolü (ilk segmentin bittiğinden emin ol)
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
        GameObject segment = Instantiate(segmentPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        spawnedSegments.Add(segment);
        spawnZ -= segmentLength;
    }

    void RemoveOldestSegment()
    {
        Destroy(spawnedSegments[0]);
        spawnedSegments.RemoveAt(0);
    }
}
