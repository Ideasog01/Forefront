using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPController : MonoBehaviour
{
    [Header("EMP Settings")]

    [SerializeField]
    private float startRadius;

    [SerializeField]
    private float endRadius;

    [SerializeField]
    private float increaseOvertime;

    [SerializeField]
    private float disableDuration;

    [SerializeField]
    private LayerMask enemyLayer;

    [Header("Effects")]

    [SerializeField]
    private Sound disabledSfx;

    private float _currentRadius;

    private Transform _mesh;

    private void Start()
    {
        _mesh = this.transform.GetChild(0);
    }

    private void Update()
    {
        IncreaseSize();
        DetectEnemies();
    }

    private void OnEnable()
    {
        _currentRadius = startRadius;
    }

    private void IncreaseSize()
    {
        if(_currentRadius < endRadius)
        {
            _currentRadius += Time.deltaTime * increaseOvertime;
            _mesh.transform.localScale = Vector3.one * _currentRadius;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _currentRadius, enemyLayer);

        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("EnemyDefault"))
            {
                EnemyEntity enemy = collider.transform.parent.GetComponent<EnemyEntity>();

                if(!enemy.DisableEnemy)
                {
                    enemy.Disable(disableDuration);
                    GameManager.audioManager.PlaySound(disabledSfx);
                }
            }
        }
    }
}
