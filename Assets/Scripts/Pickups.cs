using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{

    public GameHandler gameHandler;
    Collider2D col2D;

    private void Start() {
        col2D = GetComponent<Collider2D>();
        gameHandler = FindObjectOfType<GameHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>()){
            gameHandler.increaseScore(other.GetComponent<Player>());
            Destroy(this.gameObject);
        }
    }



}
