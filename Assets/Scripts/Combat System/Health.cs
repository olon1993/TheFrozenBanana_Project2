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
    [SerializeField] AudioSource HurtSoundEffect;
    [SerializeField] AudioSource DieSoundEffect;
    [SerializeField] List<Damage> DamageMultipliers;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

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
            if(HurtSoundEffect != null)
            {
                HurtSoundEffect.Play();
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

        if(DieSoundEffect != null)
        {
            DieSoundEffect.Play();
        }

        if (gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            if(DieSoundEffect != null)
            {
                Destroy(gameObject, DieSoundEffect.clip.length);
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
