using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public Image ringHealthBar;
    public Image ringStaminaBar;
    public Image ringInfectionBar;

    [HideInInspector] public float health, maxHealth = 100.0f;
    [HideInInspector] public float stamina, maxStamina = 100.0f;
    [HideInInspector] public float infection, maxinfection = 100.0f;
    float lerpSpeed;

    public GameOverScreen gameOverScreen;

    public TextMeshProUGUI MaxHPText;
    public TextMeshProUGUI MaxStaminaText;

    private void OnEnable()
    {
        health = maxHealth;
        stamina = maxStamina;
        infection = 0;

        MaxHPText.text = "MaxHP : " + maxHealth.ToString();
        MaxStaminaText.text = "MaxStamina : " + maxStamina.ToString();
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

            if (health <= 0 || infection >= maxinfection)
            {
                // 게임 오버
                gameOverScreen.SetUp();
            }
        }
    }
    public void SubStamina(float DecreaseStamina)
    {
        if (stamina > 0)
        {
            stamina -= DecreaseStamina;
        }
    }
    public void AddStamina(float IncreaseStamina)
    {
        stamina += IncreaseStamina;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }
}
