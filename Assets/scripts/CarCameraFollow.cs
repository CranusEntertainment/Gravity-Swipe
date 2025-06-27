using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    public Transform car;
    public Vector3 offset = new Vector3(0f, 5f, -10f);
    public float followSpeed = 5f; // Pozisyon takip hizi

    void LateUpdate()
    {
        if (car == null)
            return;

        
        // Hedef pozisyonu hesapla (aracin konumuna offset uygula)
        Vector3 targetPosition = car.position + car.rotation * offset;

        // Sadece pozisyonu Lerp ile takip et
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // ACIYI DEGISTIRME — sadece pozisyon takip edilir
    }
}
