using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    public Transform car;
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    public float followSmoothTime = 0.2f;  // Pozisyon yumuþatma süresi (kýsa süre daha hýzlý takip)
    public float rotationSmoothTime = 0.1f; // Dönüþ yumuþatma süresi

    private Vector3 currentVelocity = Vector3.zero;
    private float currentRotationVelocity;

    void LateUpdate()
    {
        if (car == null)
            return;

        // Hedef pozisyon: araba + offset (arabanýn arkasýnda)
        Vector3 desiredPosition = car.position + car.rotation * offset;

        // Pozisyonu SmoothDamp ile yumuþak takip et
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, followSmoothTime);

        // Hedef bakýþ rotasyonu: arabanýn pozisyonuna bak
        Vector3 directionToCar = car.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(directionToCar.normalized, Vector3.up);

        // Kameranýn açýsýný SmoothDamp benzeri þekilde yumuþatmak için açýyý ayarla
        float angle = Quaternion.Angle(transform.rotation, desiredRotation);
        float smoothFactor = Mathf.SmoothDampAngle(0, angle, ref currentRotationVelocity, rotationSmoothTime);

        // Ýleri doðru yumuþak dönüþ (slerp, ama yavaþ yavaþ hýzlanarak)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, smoothFactor * Time.deltaTime * 1000f);
    }
}
