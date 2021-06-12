using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbColhandler : MonoBehaviour
{
    public Rigidbody2D Rb2D {get; private set; }
    Collider2D col2D;
    WizMan player;
    SpriteRenderer rend;
    public bool Exploding {get; set; } = false;
    private void Awake() {
        col2D = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        Rb2D = GetComponent<Rigidbody2D>();
        // player = FindObjectOfType<WizMan>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("contact with orb");
        Exploding = true;
    }
}
