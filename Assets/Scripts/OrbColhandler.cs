using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbColhandler : MonoBehaviour
{
    Collider2D col2D;
    Rigidbody2D rb2D;
    private void Start() {
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // rb2D.bodyType = RigidbodyType2D.Static;
        // transform.parent = other.gameObject.transform;
        // col2D.enabled = false;
        // if (other.gameObject.CompareTag("Enemy")) {
        //     other.gameObject.GetComponent<Baddie>().takeDamage();
        // }
    }
}
