using UnityEngine;
public class SpeedLineEffect : MonoBehaviour
{
    public Material speedLineMaterial;
    public Rigidbody carRigidbody;

    void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        speedLineMaterial.SetFloat("_Speed", speed);
        speedLineMaterial.SetFloat("_BlurAmount", Mathf.Clamp01(speed / 50f));
        speedLineMaterial.SetFloat("_Opacity", Mathf.Clamp01(speed / 100f));
    }
}
