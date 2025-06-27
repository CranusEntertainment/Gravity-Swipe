using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText; // Game Over ekranı için
    public Text highScoreDisplayText; // Oyun içi sürekli görünen

    public CarMovement carMovement;

    public float baseScoreIncreaseSpeed = 30f;
    public float lerpSpeed = 5f;

    public float scaleUpAmount = 1.2f;
    public float scaleSpeed = 10f;

    private int highScore = 0;
    private float currentScore = 0f;
    private float displayedScore = 0f;
    private bool scaleUp = false;
    private int lastColorChangeScore = 0;

    private bool isBoostFireActive = false;
    private float fireAnimTimer = 0f;

    private Color[] arcadeColors = new Color[]
    {
        new Color(1f, 0.1f, 0.1f),
        new Color(1f, 0.6f, 0.1f),
        new Color(1f, 1f, 0.2f),
        new Color(0.1f, 1f, 0.4f),
        new Color(0.1f, 0.8f, 1f),
        new Color(0.7f, 0.2f, 1f),
    };

    void Start()
    {
        // Varsayılan text referansı
        if (scoreText == null)
            scoreText = GetComponent<Text>();

        // HighScore'u yükle
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScoreDisplayText != null)
            highScoreDisplayText.text = "BEST: " + highScore.ToString();

        if (highScoreText != null)
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();

        // Başlangıç rengi
        scoreText.color = Color.white;
    }

    void Update()
    {
        if (carMovement == null)
            return;

        float speedRatio = Mathf.InverseLerp(carMovement.forwardSpeed, carMovement.boostedSpeed, carMovement.currentSpeed);
        float scoreIncreaseSpeed = Mathf.Lerp(baseScoreIncreaseSpeed, baseScoreIncreaseSpeed * 3f, speedRatio);

        currentScore += scoreIncreaseSpeed * Time.deltaTime;
        displayedScore = Mathf.Lerp(displayedScore, currentScore, lerpSpeed * Time.deltaTime);

        int displayInt = Mathf.FloorToInt(displayedScore); // Bunu yukarı taşıdık
        scoreText.text = displayInt.ToString();

        // High Score kontrolü
        if (displayInt > highScore)
        {
            highScore = displayInt;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            if (highScoreDisplayText != null)
                highScoreDisplayText.text = "BEST: " + highScore.ToString();

            if (highScoreText != null)
                highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        }

        // Skor font büyüme animasyonu
        if (!scaleUp && Mathf.Abs(currentScore - displayedScore) > 1f)
            scaleUp = true;

        if (scaleUp)
        {
            scoreText.transform.localScale = Vector3.Lerp(scoreText.transform.localScale, Vector3.one * scaleUpAmount, scaleSpeed * Time.deltaTime);
            if (Vector3.Distance(scoreText.transform.localScale, Vector3.one * scaleUpAmount) < 0.01f)
                scaleUp = false;
        }
        else
        {
            scoreText.transform.localScale = Vector3.Lerp(scoreText.transform.localScale, Vector3.one, scaleSpeed * Time.deltaTime);
        }

        // Renk değiştirme
        if (displayInt / 100 > lastColorChangeScore / 100)
        {
            ChangeToRandomArcadeColor();
            lastColorChangeScore = displayInt;
        }

        // Ateş animasyonu
        if (speedRatio > 0.9f)
        {
            if (!isBoostFireActive)
            {
                isBoostFireActive = true;
                fireAnimTimer = 0f;
            }
            PlayFireAnimation();
        }
        else
        {
            if (isBoostFireActive)
            {
                isBoostFireActive = false;
                ResetFireAnimation();
            }
        }
    }

    void PlayFireAnimation()
    {
        fireAnimTimer += Time.deltaTime;
        float t = Mathf.PingPong(fireAnimTimer * 5f, 1f);
        Color fireColor = Color.Lerp(Color.red, Color.yellow, t);
        scoreText.color = fireColor;
    }

    void ResetFireAnimation()
    {
        ChangeToRandomArcadeColor();
    }

    void ChangeToRandomArcadeColor()
    {
        Color newColor;
        do
        {
            newColor = arcadeColors[Random.Range(0, arcadeColors.Length)];
        } while (newColor == scoreText.color);

        scoreText.color = newColor;
    }
}
