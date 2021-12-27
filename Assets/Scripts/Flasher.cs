using UnityEngine;

public class Flasher : MonoBehaviour
{
    private Material eye;
    public Color color;
    public float intensity;
    public Light spotLight;

    private float _spotLightIntensity;

    void Start()
    {
        eye = GetComponent<Renderer>().sharedMaterial;
        _spotLightIntensity = spotLight.intensity;
    }

    void Update()
    {
        eye.SetVector("_EmissionColor", color * Mathf.Abs(Mathf.Sin(Time.time)) * intensity);
        spotLight.intensity = Mathf.Abs(Mathf.Sin(Time.time)) * _spotLightIntensity;
    }
}