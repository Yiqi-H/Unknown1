using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float addTorqueAmountInDegrees;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] public GameObject player;

    private Rigidbody2D rb;
    private Rigidbody2D rb2;

    public AudioClip explosionSound;

    private AudioSource audioSource;
    public float sidemod;
    public float upmod;
    private void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        rb2 = player.GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>();

        SetDestroyTime();
        SetStraightVelocity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * normalBulletSpeed;
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnDestroy()
    {
        Explode();
    }

    private void Explode()
    {
        audioSource.PlayOneShot(explosionSound);
        GameObject toRemove = Instantiate(impactEffect, transform.position, transform.rotation);
        toRemove.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody2D rigid = colliders[i].GetComponent<Rigidbody2D>();

            //if (temp != null)
            if (rigid != null)
            {

                // rigid.AddTorque(addTorqueAmountInDegrees * Mathf.Deg2Rad * rigid.inertia);
                AddExplosionForce(rigid, explosionForce, transform.position, explosionRadius);
            }

            if (colliders[i].gameObject.GetComponent<IDamageable>() != null)
            {
                colliders[i].gameObject.GetComponent<IDamageable>().Damage(20);
            }
        }

        Destroy(toRemove, .5f);
    }


    public void AddExplosionForce(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
            explosionDir /= explosionDistance;
        else
        {
            // From Rigidbody.AddExplosionForce doc:
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
    }
}
