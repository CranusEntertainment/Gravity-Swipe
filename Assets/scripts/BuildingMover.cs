using UnityEngine;

public class BuildingMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
    }
}
