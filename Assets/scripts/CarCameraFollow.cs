using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    public Transform car;
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    public float followSmoothTime = 0.2f;  // Pozisyon yumu�atma s�resi (k�sa s�re daha h�zl� takip)
    public float rotationSmoothTime = 0.1f; // D�n�� yumu�atma s�resi

    private Vector3 currentVelocity = Vector3.zero;
    private float currentRotationVelocity;

    void LateUpdate()
    {
        if (car == null)
            return;

        // Hedef pozisyon: araba + offset (araban�n arkas�nda)
        Vector3 desiredPosition = car.position + car.rotation * offset;

        // Pozisyonu SmoothDamp ile yumu�ak takip et
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, followSmoothTime);

        // Hedef bak�� rotasyonu: araban�n pozisyonuna bak
        Vector3 directionToCar = car.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(directionToCar.normalized, Vector3.up);

        // Kameran�n a��s�n� SmoothDamp benzeri �ekilde yumu�atmak i�in a��y� ayarla
        float angle = Quaternion.Angle(transform.rotation, desiredRotation);
        float smoothFactor = Mathf.SmoothDampAngle(0, angle, ref currentRotationVelocity, rotationSmoothTime);

        // �leri do�ru yumu�ak d�n�� (slerp, ama yava� yava� h�zlanarak)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, smoothFactor * Time.deltaTime * 1000f);
    }
}
