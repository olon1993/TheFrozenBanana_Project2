using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

public class AltHealth : MonoBehaviour
{ 
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [SerializeField] Image HealthBar;
    [SerializeField] AudioClip HurtSoundEffect;
    [SerializeField] AudioClip DieSoundEffect;
    [SerializeField] List<Damage> DamageMultipliers;

    AudioManager audioManager;

    AltLocomotion2D locomotion2D;
    [SerializeField] float damageForce = 500;

    [SerializeField] float invincibleTime = 2f;
    bool invincible;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            Debug.Log("AudioManager found");
        }
        else
        {
            Debug.Log("No AudioManager in scene");
        }

        locomotion2D = GetComponent<AltLocomotion2D>();
    }

    // Returns true if the Die method is called
    public bool TakeDamage(Damage damage, float direction)
    {
        Debug.Log("TakingDamage: " + gameObject.name);
        if (invincible)
        {
            Debug.Log("You can't touch me! : " + gameObject.name + " is invincible!");
            return false;
        }

        StartCoroutine(InvincibleCountdown());

        if (locomotion2D != null)
            StartCoroutine(locomotion2D.ApplyDamageForce(damageForce, direction, invincibleTime));

        CurrentHealth -= damage.DamageAmount;

        if (HealthBar != null)
        {
            HealthBar.fillAmount = (float)CurrentHealth / MaxHealth;
        }

        if (CurrentHealth <= 0)
        {
            Die();
            return true;
        }
        else if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            if (audioManager != null)
            {
                Debug.Log("Playing hurt sound on: " + gameObject.name);
                audioManager.PlayClip(HurtSoundEffect);
            }
            else
            {
                Debug.Log("No audioManager found in scene");
            }
        }

        return false;
    }

    private void Die()
    {
        if (_showDebugLog)
        {
            Debug.Log(gameObject.name + " has died!");
        }

        if (audioManager != null)
        {
            audioManager.PlayClip(DieSoundEffect);
        }

        if (gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (audioManager != null)
            {
                Destroy(gameObject, DieSoundEffect.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator InvincibleCountdown()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
}

