using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     public Vector2 velocity; // How fast you're moving in a direction
    public float jumpHeight = 20f; // how high you jump
    public float movementSpeed = 5f; // How fast you can run
    public float distanceDown = 0.6f; // The distance it checks if you're grounded or not
    public LayerMask collisionLayers;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
    }

    public bool is_ground()
    {
        Vector2 player_pos = transform.position;
        RaycastHit2D Ground_Check = Physics2D.Raycast(player_pos, Vector2.down, distanceDown, collisionLayers);
        if (Ground_Check.collider != null)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        ForceMode2D mode = ForceMode2D.Force;
        float moveInput = Input.GetAxisRaw("Horizontal"); // Gets your input
        bool grounded = is_ground(); // Checks if you're on the ground



        if (grounded)
        {
            if ((Input.GetKeyDown("space")) || (Input.GetKeyDown("w"))) // Makes you jump if you're on the ground
            {
                rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse); // launches the player upwards
            }
        }
        if (moveInput == 0 && grounded)
        {
            velocity.x = 0;
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, movementSpeed * moveInput, 20 * Time.deltaTime); // This tells the program how fast you're going to be moving when pressing A or D
        }
        rb.AddForce(velocity, mode); // This makes the character move
    }
}
