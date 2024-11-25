using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Image fillImage;

    public void SetHealth(int currentHealth, int maxHealth) {
        float healthPercentage = (float)currentHealth / maxHealth;
        fillImage.fillAmount = healthPercentage;
    }
}
