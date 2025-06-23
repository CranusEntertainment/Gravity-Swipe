using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject roadPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Transform thisEndPoint = transform.parent.Find("EndPoint");

        if (thisEndPoint == null)
        {
            Debug.LogError("EndPoint bulunamad�!");
            return;
        }

        GameObject newRoad = Instantiate(roadPrefab);

        Transform newStartPoint = newRoad.transform.Find("StartPoint");

        if (newStartPoint == null)
        {
            Debug.LogError("StartPoint bulunamad�!");
            return;
        }

        // HESAP: Yeni yolun tamam�, StartPoint EndPoint�e yerle�ecek �ekilde kayd�r�l�r
        Vector3 difference = newRoad.transform.position - newStartPoint.position;
        newRoad.transform.position = thisEndPoint.position + difference;

        Destroy(gameObject);
    }
}
