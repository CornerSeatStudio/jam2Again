using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Baddie : MonoBehaviour {
    public int Health {get; set; } = 1;
    public AIPath aIPath;

    private void Start() {
        aIPath = this.GetComponent<AIPath>();
    }
    void Update(){
        if(aIPath.desiredVelocity.x >= 0.01){
            transform.localScale = new Vector2(-1f, 1f);
        } else if(aIPath.desiredVelocity.x <= -0.01) {
            transform.localScale = new Vector2(1f, 1f);

        }
    }
    public void takeDamage(){
        Health--;
        if(Health <= 0) onDeath();
    }

    void onDeath(){
        Debug.Log("baddie died");
        //play sound
        //disintegrate/explode
    }

}
