using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [Header("Renderer")]
    [SerializeField] private Renderer ballRenderer;
    private NetworkVariable<Color> ballColour = new NetworkVariable<Color>();

    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn");
        Debug.Log($"Ball script || Spawned ball colour: {GetComponentInChildren<MeshRenderer>().material.color}");

        // Make sure we are the server
        if (!IsServer) return;

        // Generate a random colour for this ball
        Color randomColor = Random.ColorHSV();
        Debug.Log($"RandomColor: {randomColor}");
        ballColour.Value = randomColor;
    }

    private void Update()
    {
        // Make sure this belongs to us
        if (!IsOwner) return;

        // Check to see if the space key is pressed
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        // Send a message to THE SERVER to execute this method
        DestroyBallServerRpc();
    }

    private void OnEnable()
    {
        // Start listening for the ball colour updated
        ballColour.OnValueChanged += OnBallColourChanged;
    }

    private void OnDisable()
    {
        // Stop listening for the ball colour updated
        ballColour.OnValueChanged -= OnBallColourChanged;
    }

    private void OnBallColourChanged(Color oldBallColour, Color newBallColour)
    {
        Debug.Log("In colour change function!");

        // Only clients need to update the renderer
        if (!IsClient) return;

        Debug.Log($"OldBallColour: {oldBallColour} || NewBallColour: {newBallColour}");

        //ballRenderer.material.SetColor("_BaseColor", newBallColour);   // HDRP / URP
        ballRenderer.material.SetColor("_Color", newBallColour);    // Normal 3D
    }

    [ServerRpc]
    private void DestroyBallServerRpc()
    {
        // By destroying a NetworkObject on the server,
        // the object will then be destroyed on all clients
        Destroy(gameObject);
        //GetComponent<NetworkObject>().Despawn();
    }
}
