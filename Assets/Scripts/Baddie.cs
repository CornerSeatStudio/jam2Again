using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;
using System;
using Pathfinding;
public class Baddie : MonoBehaviour {
    public AudioClip audEnemyDeath;
    public int Health {get; set; } = 1;
    protected AIPath aIPath;
    public Collider2D Col {get; private set; }
    protected Animator animator;
    public double atkDistance;
    public double coolDown;
    public double NextAttack;
    
    protected GameHandler gameHandler;
    public AIDestinationSetter destSetter;
    public event Action<Baddie> onDeathEvent;     

    private void Start() {
        aIPath = this.GetComponent<AIPath>();
        Col = this.GetComponent<Collider2D>();
        animator = this.GetComponent<Animator>();
        gameHandler = FindObjectOfType<GameHandler>();
        destSetter = this.GetComponent<AIDestinationSetter>();

        StartCoroutine(TargetUpdater());

        NextAttack = Time.time;
        atkDistance = 10;
        coolDown = 5;
    }

    IEnumerator TargetUpdater(){
        while(true){
            if(!GameHandler.IsCoop){
                destSetter.target = gameHandler.Player1.transform;
            }else {
                if( gameHandler.Player1.Health > 0 && gameHandler.Player2.Health > 0){
                    float p1Dist = Vector3.Distance(transform.position, gameHandler.Player1.transform.position);
                    float p2Dist = Vector3.Distance(transform.position, gameHandler.Player2.transform.position);

                    if(p1Dist > p2Dist){
                        destSetter.target = gameHandler.Player2.transform;

                    } else {
                        destSetter.target = gameHandler.Player1.transform;

                    }

                } else if (gameHandler.Player1.Health > 0){
                    destSetter.target = gameHandler.Player1.transform;

                } else if (gameHandler.Player2.Health > 0){
                    destSetter.target = gameHandler.Player2.transform;

                } else {
                    destSetter.target = null;
                }
            }
            
            
            yield return new WaitForSeconds(1f);
        }
    }
    void Update(){
        if(aIPath.desiredVelocity.x >= 0.01){
            transform.localScale = new Vector2(-1f, 1f);
        } else if(aIPath.desiredVelocity.x <= -0.01) {
            transform.localScale = new Vector2(1f, 1f);
        } 
        
        // if(Time.time > NextAttack && checkAttackPosition()) {
        //     attack();
        //     NextAttack = Time.time + coolDown;
        // }
    }


    public void takeDamage(){
        Health--;
        if(Health <= 0) onDeath();
    }

    void onDeath(){
        onDeathEvent?.Invoke(this);
        Col.enabled = false;
        Debug.Log("baddie died");
        //stop moving
        //play sound
        //disintegrate/explode
        animator.SetTrigger(Animator.StringToHash("Dead"));
        Destroy(this.gameObject, 1.5f);
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audEnemyDeath);
    }
    
    //presumably, we would have the player take damage through a call here
    // public void attack(){
    //     // THis is supposed to be a placeholder for a physical attack
    //     Debug.Log("Player attacked");
        
    // }

    // //check if player is in range
    // bool checkAttackPosition(){
    //     if (Vector2.Distance(player.transform.position, this.transform.position) < atkDistance){
    //         return true;
    //     } else {
    //         return false;
    //     }
    // }

}
