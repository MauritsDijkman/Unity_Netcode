using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
//using Random = System.Random;

public class Player : NetworkBehaviour
{
    public int maxHealth = 100;
    [SyncVar]public int currentHealth;
   // public RectTransform health_bar;

    public Health_network healthbar;


    public void TakeDamage(int damage)
    {
        if(IsServer)
        {
            return;
        }

        currentHealth -= damage;
  
        if(currentHealth <= 0)
        {
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);
            RpcRespawn();
        }
        healthbar.SetHealth(currentHealth);
    }
    void RpcRespawn()
    {
        if(IsLocalPlayer)
        {
            transform.position = Vector3.zero;
            
        }
    }
}

internal class SyncVarAttribute : Attribute
{
}