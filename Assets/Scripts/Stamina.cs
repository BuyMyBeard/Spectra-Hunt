using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    Slider display;
    [SerializeField] int maxStamina = 100;
    [SerializeField] float currentStamina;
    [SerializeField] float staminaRegenRate = 100f;
    [SerializeField] int staminaRequiredToRun = 20;
    [SerializeField] float timeBeforeHidden = 2;

    FoxMovement foxMovement;
    Coroutine regenCooldown;
    public bool CanRun { get; private set; } = true;
    bool canRegen = true;
    public bool IsExhausted { get => currentStamina <= 0; }
    public float Value => maxStamina;
    float hideTimer = 0;
    public void StopRegen()
    {
        if (regenCooldown != null)
            StopCoroutine(regenCooldown);
        canRegen = false;
    }
    public void StartRegenAfterCooldown(float cooldown = .7f)
    {
        if (regenCooldown != null)
            StopCoroutine(regenCooldown);
        regenCooldown = StartCoroutine(RegenCooldown(cooldown));
    }
    IEnumerator RegenCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canRegen = true;
    }

    private void Awake()
    {
        display = GameObject.FindWithTag("Stamina").GetComponent<Slider>();
        foxMovement = GetComponent<FoxMovement>();
    }
    private void Start()
    {
        display.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ResetStamina();
    }
    private void Update()
    {
        hideTimer += Time.deltaTime;
        if (IsExhausted && CanRun)
            StartCoroutine(ExhaustionCooldown());
        if (canRegen && currentStamina < maxStamina && !foxMovement.IsRunning)
        {
            if (!display.gameObject.activeSelf) display.enabled = true;
            hideTimer = 0;
            float staminaToRegen = staminaRegenRate * Time.deltaTime;
            currentStamina += staminaToRegen;
            display.value = currentStamina / maxStamina;
            if (currentStamina > maxStamina)
                ResetStamina();
        }
        if (hideTimer > timeBeforeHidden) display.gameObject.SetActive(false);
    }
    private void ResetStamina()
    {
        currentStamina = maxStamina;
    }

    public void Remove(float value)
    {
        hideTimer = 0;
        if (!display.gameObject.activeSelf) display.gameObject.SetActive(true);
        if (!IsExhausted)
        {
            currentStamina -= value;
            if (currentStamina < 0)
                currentStamina = 0;
            display.value = currentStamina / maxStamina;
        }
    }
    private IEnumerator ExhaustionCooldown()
    {
        CanRun = false;
        yield return new WaitUntil(() => currentStamina >= staminaRequiredToRun);
        CanRun = true;
    }
}
