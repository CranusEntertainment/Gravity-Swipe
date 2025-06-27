using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Game Over Paneli")]
    public GameObject gameOverPanel;
    public Text highScoreText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCar"))
        {
            Debug.Log("Çarpışma oldu! Oyun bitiyor...");
            GameOver();
        }
    }

    void GameOver()
    {

        
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
