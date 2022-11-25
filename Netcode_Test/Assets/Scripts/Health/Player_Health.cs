using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3;
    [SerializeField] private GameObject _deathEffect, _hitEffect;
    private float _currentHealth;

    [SerializeField] private Healthbar _healthbar;

    void Start()
    {
        _currentHealth = _maxHealth;
        
        _healthbar.UpdateHealthBar(_maxHealth,_currentHealth);
    }

    void OnMouseDown()
    {
        _currentHealth -= Random.Range(0.5f, 1.5f);

        if(_currentHealth <= 0)
        {
            Instantiate(_deathEffect, transform.position, Quaternion.Euler(-90, 0, 0));
            Destroy(gameObject);
        }
        else 
        {
            _healthbar.UpdateHealthBar(_maxHealth, _currentHealth);
            Instantiate(_hitEffect, transform.position, Quaternion.identity);
        }
    }
}
