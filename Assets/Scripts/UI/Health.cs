using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public Image ringHealthBar;
    float health, maxHealth = 100;
    float lerpSpeed;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health > maxHealth) health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
    }

    void HealthBarFiller()
    {
        ringHealthBar.fillAmount = Mathf.Lerp(ringHealthBar.fillAmount, (health / maxHealth), lerpSpeed);
    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        ringHealthBar.color = healthColor;
    }

    public void ApplyDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
    }
}
