using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CarMovement : MonoBehaviour
{
    [Header("İleri Gitme (Z ekseni azalacak)")]
    public float forwardSpeed = 10f;
    public float boostedSpeed = 20f;
    public float boostLerpSpeed = 5f;
    public float currentSpeed;

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
    private ChromaticAberration chromaticAberration;
    private MotionBlur motionBlur;

    [Header("Ses Ayarları")]
    public AudioSource engineAudioSource;
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;

    [Header("Trail Ayarları")]
    public TrailRenderer rightBoostTrail;
    public TrailRenderer leftBoostTrail;
    public float defaultTrailTime = 0.2f;
    public float boostedTrailTime = 1.5f;
    public float trailLerpSpeed = 5f;

    private int moveDirection = 0;
    private bool isBoosting = false;

    void Start()
    {
        currentSpeed = forwardSpeed;

        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out lensDistortion);
            globalVolume.profile.TryGet(out chromaticAberration);
            globalVolume.profile.TryGet(out motionBlur);

            if (lensDistortion != null) lensDistortion.intensity.value = 0f;
            if (motionBlur != null) motionBlur.intensity.value = 0f;
            if (chromaticAberration != null) chromaticAberration.intensity.value = 0f;
        }

        if (engineAudioSource != null)
            engineAudioSource.Play();

        if (rightBoostTrail != null)
            rightBoostTrail.time = defaultTrailTime;
        if (leftBoostTrail != null)
            leftBoostTrail.time = defaultTrailTime;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        float speedRatio = Mathf.InverseLerp(forwardSpeed, boostedSpeed, currentSpeed);
        float targetSpeed = isBoosting ? boostedSpeed : forwardSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, boostLerpSpeed * delta);

        // Motor sesi
        if (engineAudioSource != null)
            engineAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);

        // Hareket
        Vector3 pos = transform.position;
        pos.z -= currentSpeed * delta;

        if (moveDirection != 0)
        {
            pos.x += moveDirection * sideSpeed * delta;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }
        transform.position = pos;

        // Görsel efektler
        if (lensDistortion != null)
            lensDistortion.intensity.value = Mathf.Lerp(0f, -0.5f, speedRatio);

        if (motionBlur != null)
            motionBlur.intensity.value = Mathf.Lerp(0f, 0.5f, speedRatio);

        // Yatış efekti
        float targetZRotation = moveDirection == 1 ? -tiltAmount : moveDirection == -1 ? tiltAmount : 0f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetZRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * delta);

        // Trail boylarını boost durumuna göre güncelle
        float targetTrailTime = isBoosting ? boostedTrailTime : defaultTrailTime;

        if (rightBoostTrail != null)
            rightBoostTrail.time = Mathf.Lerp(rightBoostTrail.time, targetTrailTime, trailLerpSpeed * delta);

        if (leftBoostTrail != null)
            leftBoostTrail.time = Mathf.Lerp(leftBoostTrail.time, targetTrailTime, trailLerpSpeed * delta);
    }

    // UI butonları
    public void MoveRightStart() => moveDirection = 1;
    public void MoveLeftStart() => moveDirection = -1;
    public void StopMove() => moveDirection = 0;

    public void StartBoost() => isBoosting = true;

    public void StopBoost() => isBoosting = false;
}
