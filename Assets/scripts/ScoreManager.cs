using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public CarMovement carMovement; // Arabadan h�z bilgisini alaca��z

    public float baseScoreIncreaseSpeed = 30f; // Normal skor art�� h�z�
    public float lerpSpeed = 5f;

    public float scaleUpAmount = 1.2f;
    public float scaleSpeed = 10f;

    // Arcade renk paleti (iste�e ba�l�)
    private Color[] arcadeColors = new Color[]
    {
        new Color(1f, 0.1f, 0.1f),
        new Color(1f, 0.6f, 0.1f),
        new Color(1f, 1f, 0.2f),
        new Color(0.1f, 1f, 0.4f),
        new Color(0.1f, 0.8f, 1f),
        new Color(0.7f, 0.2f, 1f),
    };

    private float currentScore = 0f;
    private float displayedScore = 0f;

    private bool scaleUp = false;
    private int lastColorChangeScore = 0;

    // Ate� animasyonu i�in
    private bool isBoostFireActive = false;
    private float fireAnimTimer = 0f;
    private float fireAnimDuration = 1f;

    void Update()
    {
        if (carMovement == null)
            return;

        // Arabadan h�z� al, normalize et (0-1 aras�)
        float speedRatio = Mathf.InverseLerp(carMovement.forwardSpeed, carMovement.boostedSpeed, carMovement.currentSpeed);

        // Skor art�� h�z�n� h�z oran�na g�re ayarla
        float scoreIncreaseSpeed = Mathf.Lerp(baseScoreIncreaseSpeed, baseScoreIncreaseSpeed * 3f, speedRatio);
        // Boosttayken 3 kat h�zland�rd�k (diledi�in gibi de�i�tir)

        // Skoru s�rekli artt�r
        currentScore += scoreIncreaseSpeed * Time.deltaTime;

        // Skoru yumu�ak�a g�ster
        displayedScore = Mathf.Lerp(displayedScore, currentScore, lerpSpeed * Time.deltaTime);
        int displayInt = Mathf.FloorToInt(displayedScore);
        scoreText.text = displayInt.ToString();

        // Animasyon tetikleme
        if (!scaleUp && Mathf.Abs(currentScore - displayedScore) > 1f)
        {
            scaleUp = true;
        }

        // Scale animasyonu
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

        // Her 100 puanda renk de�i�tir (opsiyonel)
        if (displayInt / 100 > lastColorChangeScore / 100)
        {
            ChangeToRandomArcadeColor();
            lastColorChangeScore = displayInt;
        }

        // H�z boosttayken ate� animasyonu uygula
        if (speedRatio > 0.9f) // %90 ve �st� h�zda ate� animasyonu
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

        // Basit yan�p s�nen k�rm�z�-sar� renk animasyonu
        float t = Mathf.PingPong(fireAnimTimer * 5f, 1f); // 5 kez/sn yan�p s�ner
        Color fireColor = Color.Lerp(Color.red, Color.yellow, t);
        scoreText.color = fireColor;
    }

    void ResetFireAnimation()
    {
        // Rengi varsay�lan arcade renklerinden rastgele atayal�m
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
