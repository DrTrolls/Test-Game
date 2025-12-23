using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tracks player health and provides simple damage/heal controls.
/// Later systems (enemies, UI) can subscribe to events for updates.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invulnerabilityTime = 0f;

    public UnityEvent<float, float> OnHealthChanged; // current, max
    public UnityEvent OnDeath;

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }

    private float _invulnerabilityTimer;

    private void Awake()
    {
        ResetHealth();
    }

    private void Update()
    {
        if (_invulnerabilityTimer > 0f)
        {
            _invulnerabilityTimer -= Time.deltaTime;
        }
    }

    [ContextMenu("Take 10 Damage")]
    public void DebugDamage10()
    {
        TakeDamage(10f);
    }

    [ContextMenu("Reset Health")]
    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        _invulnerabilityTimer = 0f;
        NotifyHealthChanged();
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f)
            return;

        if (_invulnerabilityTimer > 0f)
            return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        _invulnerabilityTimer = invulnerabilityTime;
        NotifyHealthChanged();

        if (CurrentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        NotifyHealthChanged();
    }

    private void HandleDeath()
    {
        OnDeath?.Invoke();
        // Additional death handling (disable input, play VFX) can be added later.
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}
