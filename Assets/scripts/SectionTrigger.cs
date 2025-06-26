using UnityEngine;

// Bu bileşen, her yol prefab'ının sonundaki bir tetikleyici objeye yerleştirilmelidir.
[RequireComponent(typeof(Collider))]
public class SectionTrigger : MonoBehaviour
{
    private bool hasBeenTriggered = false;

    private void Awake()
    {
        // Collider'ın bir tetikleyici olduğundan emin ol
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sadece bir kez ve sadece "Player" tarafından tetiklenmesini sağla
        if (hasBeenTriggered || !other.CompareTag("Player")) return;

        hasBeenTriggered = true;
        RoadManager.Instance.SpawnNextRoad();
    }
}
