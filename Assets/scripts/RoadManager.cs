// RoadManager.cs
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance;

    public GameObject[] roadPrefabs;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetRandomRoad()
    {
        int index = Random.Range(0, roadPrefabs.Length);
        return roadPrefabs[index];
    }
}
