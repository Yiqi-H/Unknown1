using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimandShoot : MonoBehaviour
{
   [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private GameObject bulletInst;

    private Vector2 worldPosition;
    private Vector2 direction;

    private float angle;

    // Delay between shots
    [SerializeField] private float shootDelay = 0.5f;
    private float shootTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        HandleGunRotation();
        HandleGunShooting();
    }

    private void HandleGunRotation()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        Debug.DrawRay(gun.transform.position, direction);

        gun.transform.right = direction;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void HandleGunShooting()
    {
        // Increment the shoot timer
        shootTimer += Time.deltaTime;

        // Check if the shoot timer has reached the shoot delay
        if (Mouse.current.leftButton.wasPressedThisFrame && shootTimer >= shootDelay)
        {
            // Reset the shoot timer
            shootTimer = 0f;

            // Spawn the bullet
            bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
        }
    }
}
