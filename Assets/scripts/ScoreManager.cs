using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    public Text highScoreDisplayText;

    public CarMovement carMovement;

    public float baseScoreIncreaseSpeed = 30f;
    public float lerpSpeed = 5f;

    public float scaleUpAmount = 1.2f;
    public float scaleSpeed = 10f;

    private int highScore = 0;
    private float currentScore = 0f;
    private float displayedScore = 0f;
    private int displayedInt = 0;

    private bool scaleUp = false;
    private int lastColorChangeScore = 0;

    private bool isBoostFireActive = false;
    private float fireAnimTimer = 0f;

    private Vector3 targetScaleUp, targetScaleNormal;

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
        if (scoreText == null)
            scoreText = GetComponent<Text>();

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        string highScoreStr = highScore.ToString();

        if (highScoreDisplayText != null)
            highScoreDisplayText.text = "BEST: " + highScoreStr;

        if (highScoreText != null)
            highScoreText.text = "HIGH SCORE: " + highScoreStr;

        scoreText.color = Color.white;

        targetScaleUp = Vector3.one * scaleUpAmount;
        targetScaleNormal = Vector3.one;
    }

    void Update()
    {
        if (carMovement == null) return;

        float delta = Time.deltaTime;

        float speedRatio = Mathf.InverseLerp(carMovement.forwardSpeed, carMovement.boostedSpeed, carMovement.currentSpeed);
        float scoreIncreaseSpeed = Mathf.Lerp(baseScoreIncreaseSpeed, baseScoreIncreaseSpeed * 3f, speedRatio);

        currentScore += scoreIncreaseSpeed * delta;
        displayedScore = Mathf.Lerp(displayedScore, currentScore, lerpSpeed * delta);

        int newDisplayInt = Mathf.FloorToInt(displayedScore);
        if (newDisplayInt != displayedInt)
        {
            displayedInt = newDisplayInt;
            scoreText.text = displayedInt.ToString();

            if (displayedInt > highScore)
            {
                highScore = displayedInt;
                string highStr = highScore.ToString();

                if (highScoreDisplayText != null)
                    highScoreDisplayText.text = "BEST: " + highStr;

                if (highScoreText != null)
                    highScoreText.text = "HIGH SCORE: " + highStr;

                PlayerPrefs.SetInt("HighScore", highScore); // Disk'e yaz
                PlayerPrefs.Save(); // Buraya kadar sadece bir kez çalışır
            }

            // Yeni skor renk kontrolü (her 100 puanda bir)
            if (displayedInt / 100 > lastColorChangeScore / 100)
            {
                ChangeToRandomArcadeColor();
                lastColorChangeScore = displayedInt;
            }
        }

        // Skor yazı büyütme
        Vector3 currentScale = scoreText.transform.localScale;
        Vector3 target = scaleUp ? targetScaleUp : targetScaleNormal;
        scoreText.transform.localScale = Vector3.Lerp(currentScale, target, scaleSpeed * delta);

        if (scaleUp && Vector3.Distance(currentScale, targetScaleUp) < 0.01f)
            scaleUp = false;
        else if (!scaleUp && Mathf.Abs(currentScore - displayedScore) > 1f)
            scaleUp = true;

        // Boost animasyonu
        if (speedRatio > 0.9f)
        {
            if (!isBoostFireActive)
            {
                isBoostFireActive = true;
                fireAnimTimer = 0f;
            }
            PlayFireAnimation(delta);
        }
        else if (isBoostFireActive)
        {
            isBoostFireActive = false;
            ResetFireAnimation();
        }
    }

    void PlayFireAnimation(float delta)
    {
        fireAnimTimer += delta;
        float t = Mathf.PingPong(fireAnimTimer * 5f, 1f);
        scoreText.color = Color.Lerp(Color.red, Color.yellow, t);
    }

    void ResetFireAnimation()
    {
        ChangeToRandomArcadeColor();
    }

    void ChangeToRandomArcadeColor()
    {
        Color current = scoreText.color;
        Color newColor = arcadeColors[Random.Range(0, arcadeColors.Length)];

        // Direkt karşılaştırma yerine tekrar denemek GC üretir; bu risk düşük, sadece aynı renkse bir kez tekrar denenir.
        if (newColor == current)
        {
            newColor = arcadeColors[(Random.Range(0, arcadeColors.Length - 1) + 1) % arcadeColors.Length];
        }

        scoreText.color = newColor;
    }
}
