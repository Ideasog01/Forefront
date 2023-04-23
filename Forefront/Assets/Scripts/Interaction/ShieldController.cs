using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField]
    private Collider shieldCollider;

    [SerializeField]
    private GameObject shieldMesh;

    [Header("Sounds")]

    [SerializeField]
    private Sound shieldBreakSound;

    [SerializeField]
    private Sound shieldRegenerateSound;

    private bool _shieldResetting;

    private GUIManager _guiManager;

    private float _shieldHealth;

    private float _shieldMaxHealth;

    private bool _regenerateShield;

    private float _shieldHealthAfterHit; //The health the shield was at to check whether regeneration is valid

    private void Start()
    {
        _shieldMaxHealth = GameManager.gameSettings.ShieldMaxHealth;
        _shieldHealth = _shieldMaxHealth;

        _guiManager = GameManager.guiManager;

        _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
    }

    private void Update()
    {
        if(_shieldHealth < _shieldMaxHealth && _shieldHealth > 0 && _regenerateShield)
        {
            _shieldHealth += Time.deltaTime * GameManager.gameSettings.ShieldChargeRate;
            _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
        }

        if(_shieldHealth >= _shieldMaxHealth)
        {
            _regenerateShield = false;
        }

        Debug.Log("Shield Health: " + _shieldHealth);
    }

    private IEnumerator DelayShieldReset()
    {
        shieldMesh.SetActive(false);
        shieldCollider.enabled = false;
        yield return new WaitForSeconds(GameManager.gameSettings.ShieldCooldown);
        _shieldHealth = _shieldMaxHealth / 4;
        _shieldResetting = false;
        _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
        shieldMesh.SetActive(true);
        shieldCollider.enabled = true;
        GameManager.audioManager.PlaySound(shieldRegenerateSound);
    }

    public void DamageShield(float amount)
    {
        _shieldHealth -= amount;
        _guiManager.DisplayShieldHealth(_shieldHealth, _shieldMaxHealth);
        _shieldHealthAfterHit = _shieldHealth;

        _regenerateShield = false;
        StartCoroutine(DelayShieldRegeneration());

        if (_shieldHealth <= 0 && !_shieldResetting)
        {
            _shieldResetting = true;
            GameManager.audioManager.PlaySound(shieldBreakSound);
            StartCoroutine(DelayShieldReset());
        }
    }

    private IEnumerator DelayShieldRegeneration()
    {
        yield return new WaitForSeconds(GameManager.gameSettings.ShieldRegenerateWaitTime);

        if(_shieldHealth == _shieldHealthAfterHit)
        {
            _regenerateShield = true;
        }
    }
}
