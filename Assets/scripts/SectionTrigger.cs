using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject[] roadPrefabs; // Birden fazla prefab atanabilir

    private static Vector3 spawnPosition = new Vector3(43, 291, -1407);
    private static float zOffset = -842f;
    private static bool isInitialized = false;

    private void Awake()
    {
        if (!isInitialized)
        {
            spawnPosition = new Vector3(43, 291, -1407);
            isInitialized = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameObject selectedPrefab = RoadManager.Instance.GetRandomRoad();

        GameObject newRoad = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        newRoad.name = "Road";

        spawnPosition.z += zOffset;
        Destroy(newRoad, 50f);
        Destroy(gameObject);
    }


    public static void ResetStatics()
    {
        spawnPosition = new Vector3(43, 291, -1407);
        isInitialized = false;
    }
}
