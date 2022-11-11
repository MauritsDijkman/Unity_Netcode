using Unity.Netcode;
using UnityEngine;

public class CameraHandler_Client : NetworkBehaviour
{
    public static CameraHandler_Client Singleton { get; private set; }

    [Header("Camera")]
    [SerializeField] private Camera clientCamera;
    private Camera serverCamera;

    private void Start()
    {
        if (CameraHandler_Client.Singleton == null)
        {
            Debug.Log("Singleton was null!");
            Singleton = this;
        }

        // Check if the client camera is null
        if (clientCamera == null)
            FindClientCamera();
    }

    private void Update()
    {
        Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer} || IsHost: {NetworkManager.Singleton.IsHost} || IsClient: {NetworkManager.Singleton.IsClient}");

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        {
            if (serverCamera == null)
                FindServerCamera();

            if (serverCamera != null)
            {
                // Turn off server camera
                if (serverCamera.gameObject.activeSelf)
                    serverCamera.gameObject.SetActive(false);
            }

            if (!IsLocalPlayer)
                return;

            // Check if the client camera is null
            if (clientCamera == null)
                FindClientCamera();

            // Turn on client camera
            if (!clientCamera.enabled)
                clientCamera.enabled = true;

            Debug.Log("Update function for client camera!");

            // Turn off all other client cameras
            foreach (Camera cam in FindObjectsOfType<Camera>())
            {
                Debug.Log("Found client camera!");

                if (cam.CompareTag("ClientCamera"))
                {
                    if (cam != clientCamera)
                    {
                        //if (cam.enabled)
                        //    cam.enabled = false;

                        //if (cam.gameObject.GetComponent<CameraHandler_Client>().enabled)
                        //    cam.gameObject.GetComponent<CameraHandler_Client>().enabled = false;

                        //if (cam.gameObject.GetComponent<MouseLook>().enabled)
                        //    cam.gameObject.GetComponent<MouseLook>().enabled = false;

                        if (cam.gameObject.activeSelf)
                            cam.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void FindServerCamera()
    {
        if (serverCamera == null)
        {
            foreach (Camera cam in FindObjectsOfType<Camera>())
            {
                if (cam.CompareTag("ServerCamera"))
                {
                    serverCamera = cam;
                    return;
                }
            }
        }
    }

    private void FindClientCamera()
    {
        if (clientCamera == null)
            clientCamera = GetComponent<Camera>();
    }
}
