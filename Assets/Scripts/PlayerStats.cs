using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]

    [SerializeField]
    private float maxHealth = 100;
    private float currentHealth;

    [SerializeField]
    private Image HealthBarFill;

    [SerializeField]
    private float healthDecreaseRateForHungerAndThirst;

    [Header("Hunger")]

    [SerializeField]
    private float maxHunger = 100;
    private float currentHunger;

    [SerializeField]
    private Image HungerBarFill;

    [SerializeField]
    private float hungerDecreaseRate;

    [Header("Thirst")]

    [SerializeField]
    private float maxThirst = 100;
    private float currentThirst;

    [SerializeField]
    private Image thirstBarFill;

    [SerializeField]
    private float thirstDecreaseRate;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        UpdateHealthBarFill();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHungerAndThirstBarFill();
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(15f);
        }
    }

    void TakeDamage(float damage, bool overTime = false)
    {
        if (overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        }
        else
        {
            currentHealth -= damage;
        }
        
        if(currentHealth <= 0)
        {
            Debug.Log("Player Died");
        }

        UpdateHealthBarFill();
    }
     void UpdateHealthBarFill()
    {
        HealthBarFill.fillAmount = currentHealth / maxHealth;
    }

    void UpdateHungerAndThirstBarFill()
    {
        //on diminue la faim / la soif au fil du temps
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;

        //on evite les valeur négatives
        currentHunger = currentHunger < 0 ? 0 : currentHunger;
        currentThirst = currentThirst < 0 ? 0 : currentThirst;

        //on met à jour les visuels
        HungerBarFill.fillAmount = currentHunger / maxHunger;
        thirstBarFill.fillAmount = currentThirst / maxThirst;

        //Si la faim ou la soif est à 0 => le joueur prend des degats
        if(currentThirst <= 0 || currentHunger <=0)
        {
            TakeDamage((currentHunger <=0 &&currentThirst <=0 ? healthDecreaseRateForHungerAndThirst * 2 : healthDecreaseRateForHungerAndThirst), true);
        }
    }

}
