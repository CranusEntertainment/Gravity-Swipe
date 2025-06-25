using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Game Over Paneli")]
    public GameObject gameOverPanel;

    private void Start()
    {
        // Oyun baþýnda panel kapalý olsun
        gameOverPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCar"))
        {
            Debug.Log("Çarpýþma oldu! Oyun bitiyor...");
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f; // Oyunu durdur
        gameOverPanel.SetActive(true); // Paneli göster
    }

    // Bu fonksiyonu butonla çaðýracaðýz
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Zamaný sýfýrdan çýkar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
