using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeedX = 2f;
    public float lookSpeedY = 2f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    private Camera playerCamera;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoveCamera();
        LookAround();
    }

    void MoveCamera()
    {
        isGrounded = Physics.Raycast(playerCamera.transform.position, Vector3.down, 1f);

        // Movimiento horizontal (izquierda/derecha, adelante/atrás)
        float moveX = Input.GetAxis("Horizontal"); // A y D
        float moveZ = Input.GetAxis("Vertical");   // W y S

        Vector3 move = playerCamera.transform.right * moveX + playerCamera.transform.forward * moveZ;

        // Movimiento en el mundo 3D
        playerCamera.transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // Salto (si está en el suelo)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Fórmula del salto
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        playerCamera.transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        transform.Rotate(Vector3.up * mouseX);

        // Limitar la rotación vertical de la cámara (para evitar mirar demasiado arriba o abajo)
        float rotationX = playerCamera.transform.localRotation.eulerAngles.x - mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}