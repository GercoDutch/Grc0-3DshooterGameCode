using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Stats
    [SerializeField][Range(0, 1000)] private float mouseSensitivity;
    float xRotation = 0f;

    public float maxClamp; // 90f
    public float minClamp; // -90f

    public Transform _player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

            if (mouseX == 0 && mouseY == 0)
                return;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, minClamp, maxClamp);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            _player.Rotate(Vector3.up * mouseX);
        }
    }
}