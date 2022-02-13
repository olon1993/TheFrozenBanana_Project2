using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IHealth
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

    protected AudioSource audioSource;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource = null)
        {
            Debug.Log("No audioSource attached to " + gameObject.name);
        }
    }

    // Returns true if the Die method is called
    public virtual bool TakeDamage(Damage damage)
    {
        CurrentHealth -= damage.DamageAmount;
        
        if(HealthBar != null)
        {
            HealthBar.fillAmount = (float)CurrentHealth / MaxHealth;
        }
        
        if(CurrentHealth <= 0)
        {
            Die();
            return true;
        }
        else if(CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            if(audioSource != null)
            {
                audioSource.PlayOneShot(HurtSoundEffect);
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

        if(audioSource != null)
        {
            audioSource.PlayOneShot(DieSoundEffect);
        }

        if (gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            if(audioSource != null)
            {
                Destroy(gameObject, DieSoundEffect.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
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
