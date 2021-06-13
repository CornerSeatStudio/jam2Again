using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class Melee : MonoBehaviour {
    public AudioClip audSwing;
    public AudioClip audAbility;
    Player player;
    public float meleeRange = 8f; 
    public float dashDistance = 5f;
    public float abilityCooldown = 3f;

    bool abilityCooldowning = false;
    private bool cooldowning = false;


    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {
        if(player.Health <= 0) return;

        if(Input.GetButtonDown("Fire1") && !cooldowning) StartCoroutine(manageCooldown());    
        if(Input.GetButtonDown("Fire2") && !abilityCooldowning) {
            StartCoroutine(doAbility()); 
            StartCoroutine(barHandle());
        }
    }


    public Slider cooldownbar;
    IEnumerator barHandle(){
        float t = abilityCooldown;
        while(t > 0){
            cooldownbar.value = Mathf.InverseLerp(0, abilityCooldown, t);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cooldownbar.value = 0;
    }

    IEnumerator manageCooldown(){
        cooldowning = true;
        conductAttack();   
        yield return new WaitForSeconds(.34f);
        cooldowning = false;
    }

    IEnumerator doAbility(){
        abilityCooldowning = true;
        player.Animator.Play(Animator.StringToHash("Ability"));
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audAbility);
   
        player.Invulnerable = true;
        
        yield return new WaitForSeconds(.2f);


        player.Col.enabled = false;

        

        Vector3 initPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,  transform.right * (transform.localScale.x > 0 ? 1 : -1), dashDistance);
        Vector3 goalPos = hit ? new Vector3(hit.point.x, hit.point.y, 0) : transform.position + transform.right * dashDistance * (transform.localScale.x > 0 ? 1 : -1);

        float t = 0;
        while(t < 1){
            player.Rb2D.MovePosition(Vector3.Lerp(initPos, goalPos, t));
            t += Time.fixedDeltaTime * 3f;

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 6f, ~LayerMask.GetMask("Player"));
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
