using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;          // Geschwindigkeit vorwärts (W)
    public float sideSpeed = 3f;          // Geschwindigkeit seitwärts/rückwärts (A, D, S)
    public float sprintMultiplier = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float turnSpeed = 90f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Einzelne Tasten separat abfragen
        float x = 0f; // A / D
        float z = 0f; // W / S
        float y = 0f; // Q / E

        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;
        if (Input.GetKey(KeyCode.Q)) y -= 1f;
        if (Input.GetKey(KeyCode.E)) y += 1f;

        // Vorwärts- und Seitwärtsbewegung getrennt berechnen
        Vector3 forwardMove = transform.forward * Mathf.Max(z, 0f) * moveSpeed;   // nur W
        Vector3 backwardMove = transform.forward * Mathf.Min(z, 0f) * sideSpeed;  // S ist langsamer
        Vector3 sideMove = transform.right * x * sideSpeed;                        // A/D sind langsamer

        Vector3 move = forwardMove + backwardMove + sideMove;
        transform.Rotate(Vector3.up, y * turnSpeed * Time.deltaTime, 0f);

        // Sprint nur auf die Gesamtbewegung anwenden (optional)
        float sprintFactor = Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f;

        controller.Move(move * sprintFactor * Time.deltaTime);

        // Boden prüfen
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Springen
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravitation
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}