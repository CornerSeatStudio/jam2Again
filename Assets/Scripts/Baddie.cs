using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Baddie : MonoBehaviour {
    public int Health {get; set; } = 1;
    public AIPath aIPath;
    Animator animator;
    public Collider2D Col {get; private set; }

    private void Start() {
        aIPath = this.GetComponent<AIPath>();
        Col = this.GetComponent<Collider2D>();
        animator = this.GetComponent<Animator>();
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
        Col.enabled = false;
        Debug.Log("baddie died");
        //stop moving
        //play sound
        //disintegrate/explode
        animator.SetTrigger(Animator.StringToHash("Dead"));
        Destroy(this.gameObject, 3f);
    }

}
