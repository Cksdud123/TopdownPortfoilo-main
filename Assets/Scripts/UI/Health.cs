using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public Image ringHealthBar;
    public Image ringStaminaBar;
    public Image ringInfectionBar;

    [HideInInspector] public float health, maxHealth = 100;
    [HideInInspector] public float stamina, maxStamina = 100;
    [HideInInspector] public float infection, maxinfection = 100;
    float lerpSpeed;

    private void Start()
    {
        health = maxHealth;
        stamina = maxStamina;
        infection = 0;
    }

    private void Update()
    {
        if (health > maxHealth) health = maxHealth;
        if (stamina > maxStamina) stamina = maxStamina;
        if (infection > maxinfection) infection = maxinfection;

        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
    }

    void HealthBarFiller()
    {
        ringHealthBar.fillAmount = Mathf.Lerp(ringHealthBar.fillAmount, (health / maxHealth), lerpSpeed);
        ringStaminaBar.fillAmount = Mathf.Lerp(ringStaminaBar.fillAmount, (stamina / maxStamina), lerpSpeed);
        ringInfectionBar.fillAmount = Mathf.Lerp(ringInfectionBar.fillAmount, (infection / maxinfection), lerpSpeed);
    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        ringHealthBar.color = healthColor;
    }

    public void ApplyDamage(float damage,float infectionRate)
    {
        if (health > 0)
        {
            health -= damage;
            infection += infectionRate;
        }
    }
    public void SubStamina(float DecreaseStamina)
    {
        if (stamina > 0)
        {
            stamina -= DecreaseStamina;
        }
    }
    public void AddStamina(float DecreaseStamina)
    {
        if (stamina > 0)
        {
            stamina += DecreaseStamina;
        }
    }
}
