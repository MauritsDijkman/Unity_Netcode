using Unity.Netcode;
using UnityEngine;

public class ParticleSpawner : NetworkBehaviour
{
    [Header("Particle")]
    [SerializeField] private GameObject particlePrefab;

    private void Update()
    {
        if (!IsOwner) return;                           // Make sure if it belongs to us
        if (!Input.GetKeyDown(KeyCode.Space)) return;   // Check to see if the space key is hit

        // Send a message to THE SERVER to execute this method
        SpawnParticleServerRpc();

        // Spawn it instantly for ourself, since we don't need to wait for the server
        Instantiate(particlePrefab, transform.position, transform.rotation);
    }

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnParticleServerRpc()
    {
        // Send a message to ALL CLIENTS to execute this method
        SpawnParticleClientRpc();
    }

    [ClientRpc(Delivery = RpcDelivery.Unreliable)]
    private void SpawnParticleClientRpc()
    {
        // Make sure we don't spawn it twice for ourselves
        if (IsOwner) return;

        // Spawn the particle
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }
}
