using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class EnemyTwoController : MonoBehaviour
{
    // Enemy walk acceleration towards max speed
    // public float walkAcceleration = 3f;
    // public float maxSpeed = 3f;

    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public enum WalkableDirection
    {
        Right,
        Left
    }

    private WalkableDirection _walkDirection;

    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(
                    gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y
                );

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (
            touchingDirections.IsGrounded && touchingDirections.IsOnWall
            || cliffDetectionZone.detectedColliders.Count == 0
        )
        {
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)
            {
                // // Accelerate towards max speed
                // rb.velocity = new Vector2(
                //     Mathf.Clamp(
                //         rb.velocity.x
                //             + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime),
                //         -maxSpeed,
                //         maxSpeed
                //     ),
                //     rb.velocity.y
                // );
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(
                    Mathf.Lerp(rb.velocity.x, 0, walkStopRate),
                    rb.velocity.y
                );
            }
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError(
                "Current walkable direction is not set to legal values of right or left"
            );
        }
    }

    public void OnHit(int damage, Vector2 knockBack)
    {
        rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
