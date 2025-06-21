using UnityEngine;
using System.Collections;
public class CarMovement : MonoBehaviour
{
    [Header("İleri (Z ↓) Hareket")]
    public float forwardSpeed = 10f;

    [Header("Sağa-Sola Hareket")]
    public float horizontalSpeed = 1f;
    public float maxX = 4f;

    private float targetX;
    private Quaternion startRotation;
    private float currentTiltZ = 0f;

    [Header("Eğim Ayarları")]
    public float tiltAmount = 15f;
    public float tiltSmooth = 5f;

    [Header("Hızlanma Ayarları")]
    public float boostedSpeed = 20f;
    public float liftAmount = 15f;
    private bool isAccelerating = false;

    // --- Yön Kontrol Bayrakları ---
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    void Awake()
    {
        targetX = transform.position.x;
        startRotation = transform.rotation;
    }

    void Update()
    {
        MoveForward();
        HandleHorizontalInput();
        ApplyHorizontalMovement();
        ApplyTiltEffect();
    }

    void MoveForward()
    {
        float speed = isAccelerating ? boostedSpeed : forwardSpeed;
        transform.position += new Vector3(0f, 0f, -speed * Time.deltaTime);
    }

    void HandleHorizontalInput()
    {
        if (isMovingLeft)
            targetX = Mathf.Clamp(targetX - horizontalSpeed * Time.deltaTime * 50f, -maxX, maxX);

        if (isMovingRight)
            targetX = Mathf.Clamp(targetX + horizontalSpeed * Time.deltaTime * 50f, -maxX, maxX);
    }

    void ApplyHorizontalMovement()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Lerp(currentPosition.x, targetX, 10f * Time.deltaTime);
        transform.position = currentPosition;
    }

    void ApplyTiltEffect()
    {
        float delta = targetX - transform.position.x;

        float targetTiltZ = Mathf.Clamp(delta * tiltAmount, -tiltAmount, tiltAmount);
        currentTiltZ = Mathf.Lerp(currentTiltZ, targetTiltZ, Time.deltaTime * tiltSmooth);

        float targetTiltX = isAccelerating ? -liftAmount : 0f;

        Quaternion tiltRotation = Quaternion.Euler(currentTiltZ, 0f, 0f);
        Quaternion liftRotation = Quaternion.Euler(targetTiltX, 0f, 0f);

        Quaternion combinedRotation = startRotation * tiltRotation * liftRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, combinedRotation, Time.deltaTime * tiltSmooth);
    }

    // --- UI Butonlar için kontrol fonksiyonları ---

    public void MoveLeft()
    {
        targetX = Mathf.Clamp(targetX - horizontalSpeed * 10f, -maxX, maxX);
    }

    public void MoveRight()
    {
        targetX = Mathf.Clamp(targetX + horizontalSpeed * 10f, -maxX, maxX);
    }

    public void Boost()
    {
        if (!isAccelerating) // zaten hızlanıyorsa tekrar başlatma
            StartCoroutine(TemporaryBoost());
    }

    private IEnumerator TemporaryBoost()
    {
        isAccelerating = true;

        float boostTime = 1.5f; // Kaç saniye hızlanacak
        float elapsed = 0f;

        float startTiltX = 0f;
        float endTiltX = -liftAmount; // Yukarı kalkma miktarı (X ekseninde)

        while (elapsed < boostTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / boostTime;

            // Yavaş yavaş ön kaldırma (başta kaldır, sonda indir)
            float tiltX = Mathf.Lerp(endTiltX, startTiltX, t);
            ApplyLiftRotation(tiltX);

            yield return null;
        }

        isAccelerating = false;
        ApplyLiftRotation(0f); // Düz konuma getir
    }
    private void ApplyLiftRotation(float tiltX)
    {
        // Z ekseni (yan eğim) yine delta'ya bağlı
        float delta = targetX - transform.position.x;
        float targetTiltZ = Mathf.Clamp(delta * tiltAmount, -tiltAmount, tiltAmount);

        Quaternion targetRotation = startRotation * Quaternion.Euler(tiltX, 0f, targetTiltZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * tiltSmooth);
    }
}
