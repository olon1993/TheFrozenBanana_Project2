using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\
    [SerializeField] float _maxStamina;
    float _currentStamina;

    [SerializeField] Slider staminaBar;


    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    private void Awake()
    {
        _currentStamina = _maxStamina;
        UpdateStaminaBar();
    }

    // returns false if not enough stamina
    public bool UseStamina(float amount)
    {
        if (_currentStamina < amount)
        {
            StaminaDrained();
            return false;
        }

        _currentStamina -= amount;
        UpdateStaminaBar();
        return true;
    }

    void StaminaDrained()
    {
        _currentStamina = 0.0f;
        UpdateStaminaBar();
        // TODO: eventHandler -> stamina drained
    }

    public void ReplenishStamina(float amount)
    {
        _currentStamina = Mathf.Min(_currentStamina + amount, _maxStamina);
        UpdateStaminaBar();
    }

    void UpdateStaminaBar()
    {
        staminaBar.value = _currentStamina / _maxStamina;
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\
    public float MaxStamina
    {
        get { return _maxStamina; }
        set { _maxStamina = value; }
    }

    public float CurrentStamina
    {
        get { return _currentStamina; }
        set { _currentStamina = value; }
    }
}
