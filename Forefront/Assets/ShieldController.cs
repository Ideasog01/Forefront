using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private bool _shieldResetting;

    private GUIManager _guiManager;

    private float _shieldHealth;

    private float _shieldMaxHealth;

    private void Start()
    {
        _shieldMaxHealth = GameManager.gameSettings.ShieldMaxHealth;
        _shieldHealth = _shieldMaxHealth;

        _guiManager = GameManager.guiManager;

        _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
    }

    private void Update()
    {
        if(_shieldHealth < _shieldMaxHealth && _shieldHealth != 0)
        {
            _shieldHealth += Time.deltaTime * GameManager.gameSettings.ShieldChargeRate;
            _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
        }
        else if (_shieldHealth == 0 && !_shieldResetting)
        {
            StartCoroutine(DelayShieldReset());
            _shieldResetting = true;
        }

        Debug.Log("Shield Health: " + _shieldHealth);
    }

    private IEnumerator DelayShieldReset()
    {
        yield return new WaitForSeconds(1);
        _shieldHealth = _shieldMaxHealth / 4;
        _shieldResetting = false;
        _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
    }

    public void DamageShield(float amount)
    {
        _shieldHealth -= amount;
        _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
    }
}
