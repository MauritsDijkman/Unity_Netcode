using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerOverheadDisplay : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text displayNameText;

    private NetworkVariable<FixedString32Bytes> displayName = new NetworkVariable<FixedString32Bytes>();

    public override void OnNetworkSpawn()
    {
        if (IsServer) return;

        PlayerData? playerData = PasswordNetworkManager.GetPlayerData(OwnerClientId);

        if (playerData.HasValue)
            displayName.Value = playerData.Value.PlayerName;
    }

    private void OnEnable()
    {
        displayName.OnValueChanged += HandleDisplayNameChanged;
    }

    private void OnDisable()
    {
        displayName.OnValueChanged -= HandleDisplayNameChanged;
    }

    private void HandleDisplayNameChanged(FixedString32Bytes oldDisplayName, FixedString32Bytes newDisplayName)
    {
        displayNameText.text = newDisplayName.ToString();
    }
}
