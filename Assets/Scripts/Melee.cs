using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Melee : MonoBehaviour {
    public AudioClip audSwing;
    Player player;
    public float meleeRange = 8f; 
    public float dashDistance = 5f;
    public float abilityCooldown = 3f;

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
        player.Animator.Play(Animator.StringToHash("Ability"));
   
        player.Invulnerable = true;
        
        yield return new WaitForSeconds(.2f);


        player.Col.enabled = false;

        

        Vector3 initPos = transform.position;
        Vector3 goalPos = transform.position + transform.right * dashDistance * (transform.localScale.x > 0 ? 1 : -1);

        float t = 0;
        while(t < 1){
            player.Rb2D.MovePosition(Vector3.Lerp(initPos, goalPos, t));
            t += Time.fixedDeltaTime * 3f;

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 5f, ~LayerMask.GetMask("Player"));
        // Debug.DrawLine(transform.position, transform.position + transform.right * meleeRange, Color.red);
            foreach(Collider2D col in cols){
                if(col.GetComponent<Baddie>()){
                    col.GetComponent<Baddie>().takeDamage();
                }
            }

            yield return new WaitForEndOfFrame();
        }

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
