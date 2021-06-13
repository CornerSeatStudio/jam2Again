using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBounds : MonoBehaviour
{
    public Rigidbody2D Rb2D {get; private set; }
    public Collider2D Col {get; private set; }

    public void Start(){
        Rb2D = this.GetComponent<Rigidbody2D>();
        Col = this.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.GetComponent<Player>().Health = 0;
            other.GetComponent<Player>().onDeath();
        }
    }

}
