using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingStation : MonoBehaviour
{
    [SerializeField]
    private float healTime;

    [SerializeField]
    private int healAmount;

    [SerializeField]
    private float healingRadius;

    [SerializeField]
    private float duration;

    [SerializeField]
    private Sound healSound;

    private bool _playerIsNear;

    private bool _isHealing;

    private PlayerEntity _player;

    private void Start()
    {
        _player = GameManager.playerEntity;
    }

    private void OnEnable()
    {
        StartCoroutine(InactiveDelay());
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, _player.transform.position);

        if(distanceToPlayer <= healingRadius)
        {
            if(!_isHealing)
            {
                StartCoroutine(HealDelay());
                _isHealing = true;
            }

            _playerIsNear = true;
        }
        else
        {
            _playerIsNear = false;
        }
    }

    private IEnumerator HealDelay()
    {
        yield return new WaitForSeconds(healTime);
        _player.Heal(healAmount);
        GameManager.audioManager.PlaySound(healSound);

        if(_playerIsNear)
        {
            StartCoroutine(HealDelay());
        }
        else
        {
            _isHealing = false;
        }
    }

    private IEnumerator InactiveDelay()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }
}
