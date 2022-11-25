using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Netcode;
using System;


  public class Health_network : NetworkBehaviour
{ 

    public const int maxHealth = 100;
    // [SyncVar(hook = "OnChangeHealth")] public int currentHealth = maxHealth;
    [SyncVar] public int currentHealth = maxHealth;
    public RectTransform healthbar;

    public void TakeDamage(int amount)
    {
       if (IsServer)
       {
            return;
       }
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = maxHealth;
          //  RpcRespawn();
        }
        healthbar.sizeDelta = new Vector2(currentHealth * 2, healthbar.sizeDelta.y);
    }

    /*
    void OnChangeHealth(int health)
    {
        healthbar.sizeDelta = new Vector2(currentHealth * 2, healthbar.sizeDelta.y);
    }
    */

    internal class SyncVarAttribute : Attribute
    {
    }
    /*
    [ClientRpc]
    void RpcRespawn()
    {
        if(IsLocalPlayer)
        {
            transform.position = Vector3.zero;
        }
    }
*/    
}


