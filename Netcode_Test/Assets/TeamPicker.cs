using Unity.Netcode;
using UnityEngine;

public class TeamPicker : MonoBehaviour
{
    public void SelectTeam(int teamIndex)
    {
        // Get the local client's id
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        // Try to get the local client object
        // If unsuccessful, return
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient)) return;

        // Try to get the TeamPlayer component from the player object
        // If unsuccessful, return
        if (!networkClient.PlayerObject.TryGetComponent<TeamPlayer>(out TeamPlayer teamPlayer)) return;

        // Send a message to the server to the the local client's team
        teamPlayer.SetTeamServerRpc((byte)teamIndex);
    }
}
