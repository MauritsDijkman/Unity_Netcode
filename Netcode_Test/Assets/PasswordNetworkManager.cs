using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Netcode;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class PasswordNetworkManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private GameObject passwordEntryUI;
    [SerializeField] private GameObject colourPickerUI;
    [SerializeField] private GameObject leaveButton;

    private static Dictionary<ulong, PlayerData> clientData;

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
        clientData = new Dictionary<ulong, PlayerData>();
        clientData[NetworkManager.Singleton.LocalClientId] = new PlayerData(nameInputField.text);

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        var payload = JsonUtility.ToJson(new ConnectionPayload()
        {
            password = passwordInputField.text,
            playerName = nameInputField.text
        });

        byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);

        if (payload.IsNullOrEmpty())
            Debug.Log("Payload is null or empty!");

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
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
        colourPickerUI.SetActive(false);
    }

    public static PlayerData? GetPlayerData(ulong clientID)
    {
        if (clientData.TryGetValue(clientID, out PlayerData playerData))
            return playerData;
        else
            return null;
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
            colourPickerUI.SetActive(true);
        }
    }

    private void HandleClientDisconnected(ulong clientID)
    {
        if (NetworkManager.Singleton.IsServer)
            clientData.Remove(clientID);

        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            passwordEntryUI.SetActive(true);
            leaveButton.SetActive(false);
            colourPickerUI.SetActive(false);
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("ID: " + request.ClientNetworkId);
        Debug.Log("Request: " + request.Payload == null);

        string payload = Encoding.ASCII.GetString(request.Payload);
        var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

        Debug.Log($"Request: {request.Payload.ToString()}");

        if (payload == null) Debug.Log("payload is null!");
        else Debug.Log($"payload: {payload}");
        if (connectionPayload == null) Debug.Log("ConnectionPayload is null!");

        bool approvedConnection = connectionPayload.password == passwordInputField.text;

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

        if (approvedConnection)
        {
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

            clientData[clientId] = new PlayerData(connectionPayload.playerName);
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
