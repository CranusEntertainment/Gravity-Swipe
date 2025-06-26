using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [Header("Spawner Ayarları")]
    public GameObject[] buildingPrefabs;   // Farklı bina prefabları
    public Transform spawnPoint;           // Nereden spawnlanacak
    public float spawnRate = 1.5f;         // Ne sıklıkla spawnlansın (saniye)

    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;           // Z yönündeki hareket hızı

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnBuilding();
            timer = 0f;
        }
    }

    void SpawnBuilding()
    {
        int randomIndex = Random.Range(0, buildingPrefabs.Length);
        GameObject building = Instantiate(buildingPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        building.AddComponent<BuildingMover>().moveSpeed = moveSpeed;
    }
}
