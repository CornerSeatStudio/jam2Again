using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowColHandler : MonoBehaviour
{
    Collider2D col2D;

    private void Start() {
        col2D = GetComponent<Collider2D>();

    }

    private void OnCollisionEnter2D(Collision2D other) {
        // if (other.gameObject.CompareTag()) {
        //   Physics.IgnoreCollision(theobjectToIgnore.collider, collider);
        // }
    }
}
