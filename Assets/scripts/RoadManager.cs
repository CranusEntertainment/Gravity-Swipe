// RoadManager.cs
using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance;

    public GameObject[] roadPrefabs;
    public GameObject initialRoad; // Sahnedeki ilk yol parçası
    public Transform player; // Oyuncunun pozisyonunu takip etmek için
    public float destroyDistanceBehindPlayer = 150f; // Arkada kalan yolların silinme mesafesi

    private Transform lastSpawnedRoadEnd;
    private readonly Queue<GameObject> activeRoads = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (initialRoad == null)
        {
            Debug.LogError("RoadManager: Initial Road atanmamış!");
            return;
        }

        // Başlangıç yolunun bitiş noktasını bul. Her yol prefab'ında bu isimde bir obje olmalı.
        lastSpawnedRoadEnd = initialRoad.transform.Find("EndPoint");
        if (lastSpawnedRoadEnd == null)
        {
            Debug.LogError($"'{initialRoad.name}' prefab'ı içinde 'EndPoint' adında bir alt obje bulunamadı.");
        }
        activeRoads.Enqueue(initialRoad);
    }

    private void Update()
    {
        // Oyuncunun gerisinde kalan yolları temizle
        if (activeRoads.Count > 2) // Sahnede en az 2 yol kalsın
        {
            GameObject oldestRoad = activeRoads.Peek();
            // Oyuncunun Z pozisyonu, en eski yolun Z pozisyonundan belirli bir miktar ilerideyse (yani yol arkada kaldıysa)
            if (player.position.z < oldestRoad.transform.position.z - destroyDistanceBehindPlayer)
            {
                Destroy(activeRoads.Dequeue());
            }
        }
    }

    public void SpawnNextRoad()
    {
        GameObject roadToSpawn = GetRandomRoad();
        // Yeni yolu, bir önceki yolun bittiği yerden başlat
        GameObject newRoad = Instantiate(roadToSpawn, lastSpawnedRoadEnd.position, lastSpawnedRoadEnd.rotation);

        // Bir sonraki yolun spawn olacağı yeni bitiş noktasını güncelle
        lastSpawnedRoadEnd = newRoad.transform.Find("EndPoint");
        if (lastSpawnedRoadEnd == null)
        {
            // Bu hata, bir sonraki yolun nerede spawn olacağını bilemeyeceğimiz için kritik.
            Debug.LogError($"'{newRoad.name.Replace("(Clone)", "")}' prefab'ı içinde 'EndPoint' adında bir alt obje bulunamadı! Yol üretimi duracak.", newRoad);
            enabled = false; // RoadManager'ı durdurarak daha fazla hata oluşmasını engelle.
            return;
        }
        activeRoads.Enqueue(newRoad);
    }

    private GameObject GetRandomRoad()
    {
        if (roadPrefabs == null || roadPrefabs.Length == 0) return null;
        int index = Random.Range(0, roadPrefabs.Length);
        return roadPrefabs[index];
    }
}
