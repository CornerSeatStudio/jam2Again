using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Melee : MonoBehaviour {
    public AudioClip audSwing;
    public AudioClip audAbility;
    Player player;
    public float meleeRange = 8f; 
    public float abilityCooldown = 5f;

    bool abilityCooldowning = false;

    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {
        if(Input.GetButtonDown("Fire1")) conductAttack();    
        if(Input.GetButtonDown("Fire2") && !abilityCooldowning) StartCoroutine(doAbility()); 
    }

    IEnumerator doAbility(){
        abilityCooldowning = true;
        player.Animator.SetTrigger(Animator.StringToHash("Ability"));
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audAbility);
        player.Invulnerable = true;
        player.Col.enabled = false;

        


        player.Col.enabled = true;
        player.Invulnerable = false;
        yield return new WaitForSeconds(abilityCooldown);
        abilityCooldowning = false;
    }

    
    void conductAttack(){
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audSwing);
        player.Animator.SetTrigger(Animator.StringToHash("Hitting"));
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + transform.right * meleeRange * (transform.localScale.x > 0 ? 1 : -1), 8f, ~LayerMask.GetMask("Player"));
        // Debug.DrawLine(transform.position, transform.position + transform.right * meleeRange, Color.red);
        foreach(Collider2D col in cols){
            if(col.GetComponent<Baddie>()){
                col.GetComponent<Baddie>().takeDamage();
            }
        }

    }
}
