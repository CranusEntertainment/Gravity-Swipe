using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;  // Directional light referansý
    public float dayDuration = 24f; // Bir günün süresi (saniye cinsinden)
    private float timeOfDay = 0f;   // Günün saati (0-24 arasý)

    void Update()
    {
        // Zamaný ilerlet
        timeOfDay += Time.deltaTime / dayDuration * 24f;  // 24 saatlik döngü
        if (timeOfDay >= 24f)
        {
            timeOfDay = 0f;  // Zaman 24 saatlik döngüyü geçtiðinde sýfýrla
        }

        // Directional light'ýn dönüþünü ayarla
        float rotationAngle = timeOfDay / 24f * 360f;  // 24 saatte 360 derece dönmeli
        directionalLight.transform.rotation = Quaternion.Euler(rotationAngle - 18.872f, 0f, 0f);  // Dönüþ açýsýný ayarla
    }
}
