using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PasswordNetworkManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private GameObject passwordEntryUI;
    [SerializeField] private GameObject leaveButton;

    private void Start()
    {
        if (leaveButton.activeSelf)
            leaveButton.SetActive(false);

        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
    }

    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwordInputField.text);
        NetworkManager.Singleton.StartClient();
    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if (NetworkManager.Singleton.IsClient)
            NetworkManager.Singleton.Shutdown();

        passwordEntryUI.SetActive(true);
        leaveButton.SetActive(false);
    }

    private void HandleServerStarted()
    {
        if (NetworkManager.Singleton.IsHost)
            HandleClientConnected(NetworkManager.Singleton.LocalClientId);
    }

    private void HandleClientConnected(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            passwordEntryUI.SetActive(false);
            leaveButton.SetActive(true);
        }
    }

    private void HandleClientDisconnected(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            passwordEntryUI.SetActive(true);
            leaveButton.SetActive(false);
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        string password = Encoding.ASCII.GetString(request.Payload);

        bool approvedConnection = password == passwordInputField.text;

        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        // Your approval logic determines the following values
        response.Approved = approvedConnection;
        response.CreatePlayerObject = true;

        // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
        response.PlayerPrefabHash = null;

        // Switch positions according to players
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        switch (NetworkManager.Singleton.ConnectedClients.Count)
        {
            case 0:
                spawnPos = new Vector3(-3f, 0f, 0f);
                spawnRot = Quaternion.Euler(0f, 135, 0f);
                break;

            case 1:
                spawnPos = new Vector3(-1f, 0f, 0f);
                spawnRot = Quaternion.Euler(0f, 180f, 0f);
                break;

            case 2:
                spawnPos = new Vector3(1f, 0f, 0f);
                spawnRot = Quaternion.Euler(0f, 225f, 0f);
                break;

            case 3:
                spawnPos = new Vector3(3f, 0f, 0f);
                spawnRot = Quaternion.Euler(0f, 270f, 0f);
                break;
        }

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = spawnPos;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = spawnRot;

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }

}
