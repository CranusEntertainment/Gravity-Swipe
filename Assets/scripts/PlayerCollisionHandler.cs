using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Game Over Paneli")]
    public GameObject gameOverPanel;

    private void Start()
    {
        // Oyun ba��nda panel kapal� olsun
        gameOverPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCar"))
        {
            Debug.Log("�arp��ma oldu! Oyun bitiyor...");
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f; // Oyunu durdur
        gameOverPanel.SetActive(true); // Paneli g�ster
    }

    // Bu fonksiyonu butonla �a��raca��z
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Zaman� s�f�rdan ��kar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
