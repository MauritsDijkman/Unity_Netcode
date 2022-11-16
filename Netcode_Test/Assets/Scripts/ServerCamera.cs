using Unity.Netcode;
using UnityEngine;

public class ServerCamera : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera serverCamera;
    private Camera ownClientCam;

    private void Start()
    {
        FindServerCamera();
    }

    private void Update()
    {
        Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer} || IsHost: {NetworkManager.Singleton.IsHost} || IsClient: {NetworkManager.Singleton.IsClient}");

        SetServerCamera(false);

        /**/
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        {
            Debug.Log("IsHost or IsClient");

            ownClientCam = GetComponent<Camera>();

            foreach (Camera cam in FindObjectsOfType<Camera>())
            {
                if (cam.CompareTag("ClientCamera") && cam != ownClientCam)
                    cam.gameObject.SetActive(false);
            }

            //ownClientCam.gameObject.SetActive(true);
        }
        else if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Is only server");

            SetServerCamera(true);
            SetClientCameras(false);
        }
        /**/
    }

    private void SetServerCamera(bool turnOn)
    {
        if (serverCamera == null)
            FindServerCamera();

        serverCamera.gameObject.SetActive(turnOn);
    }

    private void SetClientCameras(bool turnOn)
    {
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam.CompareTag("ClientCamera"))
                cam.gameObject.SetActive(turnOn);
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
