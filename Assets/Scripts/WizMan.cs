using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class WizMan : MonoBehaviour {
    Player player;
    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {
        if(Input.GetButtonDown("Fire1")) conductAttack();    
    }

    void conductAttack(){
        player.Animator.SetTrigger(Animator.StringToHash("Hitting"));
       

    }
}

