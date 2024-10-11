using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _sliderHealth;

    private Enemy enemy;
    [SerializeField] private Camera camera;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemy.HealthChanged += OnHealthChanged;
        camera = Camera.main;
    }

    private void OnDestroy()
    {
        enemy.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float value)
    {
        _sliderHealth.value = value;
    }

    private void LateUpdate()
    {
        _sliderHealth.transform.LookAt(camera.transform.position);
        _sliderHealth.transform.Rotate(0, 180, 0);
    }

    public void SetPlayerCamera(Camera camera)
    {
        this.camera = camera;
    }
}