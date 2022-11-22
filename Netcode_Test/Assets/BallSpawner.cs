using Unity.Netcode;
using UnityEngine;

public class BallSpawner : NetworkBehaviour
{
    [Header("Prefab")]
    [SerializeField] private NetworkObject ballPrefab;

    private Camera mainCamera;

    private void Start()
    {
        // Cache a reference to the main camera
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Make sure this belongs to us
        if (!IsOwner) return;

        // Check to see if the left mouse button is hit
        if (!Input.GetMouseButtonDown(0)) return;

        // Find where we clicked in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;

        // Send a message to THE SERVER to execute this method
        SpawnBallServerRpc(hit.point);
    }

    [ServerRpc]
    private void SpawnBallServerRpc(Vector3 spawnPos)
    {
        // Spawn the prefab in normally (on the server)
        NetworkObject ballInstance = Instantiate(ballPrefab, spawnPos, Quaternion.identity);

        // Replicate the object to all clients and give ownership to the client that owns this player
        ballInstance.SpawnWithOwnership(OwnerClientId);
        Debug.Log($"Spawned ball colour: {ballInstance.GetComponentInChildren<MeshRenderer>().material.color}");
    }
}
