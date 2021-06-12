using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Melee : MonoBehaviour {

    Player player;
    public float meleeRange = 8f; 
    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {
        if(Input.GetButtonDown("Fire1")) conductAttack();    
    }

    void conductAttack(){
        player.Animator.SetTrigger(Animator.StringToHash("Hitting"));
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + transform.right * meleeRange * (transform.localScale.x > 0 ? 1 : -1), 5f, ~LayerMask.GetMask("Player"));
        // Debug.DrawLine(transform.position, transform.position + transform.right * meleeRange, Color.red);
        foreach(Collider2D col in cols){
            if(col.GetComponent<Baddie>()){
                col.GetComponent<Baddie>().takeDamage();
            }
        }

    }
}
