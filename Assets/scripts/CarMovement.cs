using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CarMovement : MonoBehaviour
{
    [Header("İleri Gitme (Z ekseni azalacak)")]
    public float forwardSpeed = 10f;
    public float boostedSpeed = 20f; // Boost aktifkenki hız
    public float boostLerpSpeed = 5f; // Hız geçiş yumuşaklığı

    private float currentSpeed; // Gerçek zamanlı ileri hızı

    [Header("Sağa–Sola Hareket")]
    public float sideSpeed = 5f;
    public float maxX = 4f;
    public float minX = -4f;

    [Header("Yatış Efekti")]
    public float tiltAmount = 10f;
    public float tiltSpeed = 5f;

    [Header("Post-Processing Ayarları")]
    public Volume globalVolume;
    private LensDistortion lensDistortion;


    private int moveDirection = 0; // -1: sola, 1: sağa, 0: düz
    private bool isBoosting = false;

    void Start()
    {
        currentSpeed = forwardSpeed;
        // LensDistortion bileşenini Volume içinden al
        if (globalVolume != null && globalVolume.profile.TryGet(out lensDistortion))
        {
            lensDistortion.intensity.value = 0f; // Başlangıçta bozulma yok
        }
    }

    void Update()
    {
        // Boost kontrolü (yumuşak geçişli hız değişimi)
        float targetSpeed = isBoosting ? boostedSpeed : forwardSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, boostLerpSpeed * Time.deltaTime);

        // Sürekli ileri hareket (Z ↓)
        transform.Translate(Vector3.back * currentSpeed * Time.deltaTime);

        // Sağa–sola hareket
        if (moveDirection != 0)
        {
            float xMove = moveDirection * sideSpeed * Time.deltaTime;
            Vector3 newPos = transform.position + new Vector3(xMove, 0f, 0f);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            transform.position = newPos;
        }

        if (lensDistortion != null)
        {
            // Hızı normalize et (0 = normal hız, 1 = boost hızı)
            float speedRatio = Mathf.InverseLerp(forwardSpeed, boostedSpeed, currentSpeed);

            // Hıza bağlı olarak intensity'yi ayarla (0 → -0.5)
            float targetDistortion = Mathf.Lerp(0f, -0.5f, speedRatio);

            // Uygula
            lensDistortion.intensity.value = targetDistortion;
        }
        // Yatış açısını belirle
        float targetZRotation = 0f;
        if (moveDirection == 1)
            targetZRotation = -tiltAmount;
        else if (moveDirection == -1)
            targetZRotation = tiltAmount;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetZRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }

    // UI butonları
    public void MoveRightStart() => moveDirection = 1;
    public void MoveLeftStart() => moveDirection = -1;
    public void StopMove() => moveDirection = 0;

    // BOOST buton fonksiyonları
    public void StartBoost() => isBoosting = true;
    public void StopBoost() => isBoosting = false;
}
