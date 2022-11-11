using Unity.Netcode;
using UnityEngine;

public class CameraHandler_Server : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera serverCamera;

    private void Start()
    {
        // Check if the server camera is null
        if (serverCamera == null)
            FindServerCamera();
    }

    private void Update()
    {
        Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer} || IsHost: {NetworkManager.Singleton.IsHost} || IsClient: {NetworkManager.Singleton.IsClient}");

        if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsClient)
        {
            // Check if the server camera is null
            if (serverCamera == null)
                FindServerCamera();

            // Turn on server camera
            if (!serverCamera.gameObject.activeSelf)
                serverCamera.gameObject.SetActive(true);

            // Turn off all client cameras
            foreach (Camera cam in FindObjectsOfType<Camera>())
            {
                if (cam.CompareTag("ClientCamera"))
                {
                    // if (cam.enabled)
                    //     cam.enabled = false;

                    // if (cam.gameObject.GetComponent<CameraHandler_Client>().enabled)
                    //     cam.gameObject.GetComponent<CameraHandler_Client>().enabled = false;

                    //if (cam.gameObject.GetComponent<MouseLook>().enabled)
                    //    cam.gameObject.GetComponent<MouseLook>().enabled = false;

                    if (cam.gameObject.activeSelf)
                        cam.gameObject.SetActive(false);
                }
            }
        }
        else if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.IsHost && NetworkManager.Singleton.IsClient)
        {
            // Turn off server camera
            if (serverCamera.gameObject.activeSelf)
                serverCamera.gameObject.SetActive(false);

            // Turn off this object
            gameObject.SetActive(false);
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
}
