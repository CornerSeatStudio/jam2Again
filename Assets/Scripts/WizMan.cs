using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class WizMan : MonoBehaviour {

    public AudioClip audExplod;
    public AudioClip audExplodsec;
    public AudioClip audCast;
    Player player;
    public float orbLifetime = 5f;
    public GameObject orbPrefab;
    public Transform orbSpawnPos;
    public float orbMoveSpeed;
    public float explosionRadius = 8f;
    public float attackCooldown = 2f;
    public float abilityCooldown = 5f;
    bool cooldowning = false;
    bool abilityCooldowning = false;
   
    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {
        if(Input.GetButtonDown("Fire1") && !cooldowning) {
            
            StartCoroutine(manageCooldown());
        } 

        if(Input.GetButtonDown("Fire2") && !abilityCooldowning){
            StartCoroutine(doAbility());

        }
    }



    IEnumerator doAbility(){
        abilityCooldowning = true;
        player.Animator.Play(Animator.StringToHash("Ability"));
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audExplodsec);
        yield return new WaitForSeconds(.2f);
        conductAbility();
        yield return new WaitForSeconds(abilityCooldown);
        abilityCooldowning = false;
    }


    IEnumerator manageCooldown(){
        cooldowning = true;
        player.Animator.SetTrigger(Animator.StringToHash("Hitting"));
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audCast);
        yield return new WaitForSeconds(.3f);
        conductAttack();   
        yield return new WaitForSeconds(attackCooldown);
        cooldowning = false;
    }

    void conductAbility(){
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D col in cols){
            if(col.GetComponent<Baddie>()){
                col.GetComponent<Baddie>().takeDamage();
            }
        }
    }

    void conductAttack(){


        GameObject orb = Instantiate(orbPrefab, orbSpawnPos.position, transform.rotation);
        StartCoroutine(handleOrb(orb.GetComponent<OrbColhandler>(), !player.FacingLeft ? 1 : -1));
        // orbRB.AddForce(transform.right * arrowForce * , ForceMode2D.Impulse);

    }

    IEnumerator handleOrb(OrbColhandler orb, float launchDir){
        float t = 0;
        while(!orb.Exploding && t < orbLifetime){
                    // Debug.Log(orb.Rb2D.velocity);

            orb.Rb2D.velocity = transform.right * orbMoveSpeed * launchDir;
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }   
        orb.Rb2D.velocity = Vector2.zero;

        explodeOrb(orb.gameObject);
    }

    public void explodeOrb(GameObject orb){
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audExplod);
        orb.GetComponent<Animator>().SetTrigger("Blow");

        Collider2D[] cols = Physics2D.OverlapCircleAll(orb.transform.position, explosionRadius);
        foreach(Collider2D col in cols){
            if(col.GetComponent<Baddie>()){
                col.GetComponent<Baddie>().takeDamage();
            }
            if(col.GetComponent<Player>()){
                col.GetComponent<Player>().takeDamage();
            }
        }
        Destroy(orb, 1f);
    }
}

