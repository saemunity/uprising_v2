using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockBack = new Vector2(0, 0);

    private Vector2 initialObject;

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // If you want the projectile to be effected by gravity by default, make it dynamic mode rigidbody
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
        initialObject.x = transform.position.x + 10f;
    }

    private void Update()
    {
        if (initialObject.x < transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockBack =
                transform.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);

            // Hit the target
            bool gotHit = damageable.Hit(damage, deliveredKnockBack);
            if (gotHit)
            {
                animator.SetTrigger(AnimationStrings.fire);
                Destroy(gameObject);
            }
        }
    }
}
