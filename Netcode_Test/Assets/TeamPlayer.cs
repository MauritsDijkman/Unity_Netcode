using System;
using Unity.Netcode;
using UnityEngine;

public class TeamPlayer : NetworkBehaviour
{
    [Header("PlayerInfo")]
    [SerializeField] private Renderer teamColourRenderer;
    [SerializeField] private Color[] teamColours;

    private NetworkVariable<byte> teamIndex = new NetworkVariable<byte>(byte.MinValue);

    [ServerRpc]
    public void SetTeamServerRpc(byte newTeamIndex)
    {
        // Make sure the newTeamIndex being received is valid
        if (newTeamIndex > 3) return;

        // Updat the teamIndex NetworkVariable
        teamIndex.Value = newTeamIndex;
    }

    private void OnEnable()
    {
        // Start listening for the teamIndex being updated
        teamIndex.OnValueChanged += OnTeamChanged;
    }

    private void OnDisable()
    {
        // Stop listening for the teamIndex being updated
        teamIndex.OnValueChanged -= OnTeamChanged;
    }

    private void OnTeamChanged(byte oldTeamIndex, byte newTeamIndex)
    {
        // Only clients need to update the renderer
        if (!IsClient) return;

        // Update the colour of the player's mesh renderer
        //teamColourRenderer.material.SetColor("_BaseColor", teamColours[newTeamIndex]);     // HDRP / URP
        teamColourRenderer.material.SetColor("_Color", teamColours[newTeamIndex]);      // Normal 3D
    }
}
