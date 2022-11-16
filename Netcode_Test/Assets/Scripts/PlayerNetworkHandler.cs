using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkHandler : NetworkBehaviour
{
    public static PlayerNetworkHandler singleton;

    private void Start()
    {
        singleton = this;

        if (IsLocalPlayer)
        {

        }
    }
}
