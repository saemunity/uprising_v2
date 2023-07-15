using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damageAttack = 20;

    public Vector2 knockBack = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // See if it can be hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y); 

            // Hit the target
            bool gotHit = damageable.Hit(damageAttack, deliveredKnockBack);
        }
    }
}