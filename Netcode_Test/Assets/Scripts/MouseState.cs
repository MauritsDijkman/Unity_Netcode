using UnityEngine;

public class MouseState : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode input = KeyCode.P;

    private void Update()
    {
        if (Input.GetKeyDown(input))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;

            if (Cursor.visible)
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }
    }
}
