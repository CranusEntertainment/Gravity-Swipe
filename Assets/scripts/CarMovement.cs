using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("İleri (Z ↓) Hareket")]
    public float forwardSpeed = 10f;

    [Header("Sağa-Sola Hareket")]
    public float horizontalSpeed = 0.1f;
    public float maxX = 4f;

    private Vector2 touchStartPosition;
    private bool isTouching = false;
    private float targetX;
    private Quaternion startRotation;
    private float initialTargetX;
    private float currentTiltZ = 0f;
    private bool isSwipeDetected = false;
    private float swipeThreshold = 0.02f; // Ekran genişliğine göre minimum swipe mesafesi


    [Header("Eğim Ayarları")]
    public float tiltAmount = 15f;         // Maksimum eğim derecesi
    public float tiltSmooth = 5f;          // Eğimin dönüş hızı

    [Header("Hızlanma Ayarları")]
    public float boostedSpeed = 20f;     // Hızlanınca çıkacağı hız
    public float liftAmount = 15f;       // Ön kaldırma açısı
    private bool isAccelerating = false; // Basılı tutuluyor mu


    void Awake()
    {
        startRotation = transform.rotation;
        targetX = transform.position.x;
    }

    void Update()
    {
        bool touchHeld = Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && Input.GetTouch(0).phase != TouchPhase.Canceled;

        // Sadece dokunuluyorsa VE swipe yapılmıyorsa hızlan
        isAccelerating = touchHeld && !isSwipeDetected;

        MoveForward();
        HandleSwipe();
        ApplyHorizontalMovement();
        ApplyTiltEffect();
    }
    void MoveForward()
    {
        float speed = isAccelerating ? boostedSpeed : forwardSpeed;
        transform.position += new Vector3(0f, 0f, -speed * Time.deltaTime);
    }
    void HandleSwipe()
    {
        if (Input.touchCount == 0)
        {
            isTouching = false;
            isSwipeDetected = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStartPosition = touch.position;
            isTouching = true;
            isSwipeDetected = false;
            initialTargetX = targetX;
        }
        else if (touch.phase == TouchPhase.Moved && isTouching)
        {
            float swipeDelta = (touch.position.x - touchStartPosition.x) / Screen.width;

            // Eğer belli bir eşiği geçerse swipe say
            if (Mathf.Abs(swipeDelta) > swipeThreshold)
            {
                isSwipeDetected = true;
                targetX = Mathf.Clamp(initialTargetX + swipeDelta * horizontalSpeed * 10f, -maxX, maxX);
            }
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isTouching = false;
            isSwipeDetected = false;
        }
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

        float targetTiltZ = 0f;
        if (Mathf.Abs(delta) > 0.01f)
            targetTiltZ = Mathf.Clamp(delta * tiltAmount, -tiltAmount, tiltAmount);

        currentTiltZ = Mathf.Lerp(currentTiltZ, targetTiltZ, Time.deltaTime * tiltSmooth);

        float targetTiltX = isAccelerating ? -liftAmount : 0f;

        // Ekstra smooth dönüş için X ekseni tilt'ini ayrı yumuşatabiliriz (opsiyonel)
        Quaternion targetRotation = startRotation * Quaternion.Euler(currentTiltZ, 0f, targetTiltZ);
        Quaternion liftRotation = Quaternion.Euler(targetTiltX, 0f, 0f);

        // İki rotasyonu birleştir
        Quaternion combinedRotation = targetRotation * liftRotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, combinedRotation, Time.deltaTime * tiltSmooth);
    }

}
