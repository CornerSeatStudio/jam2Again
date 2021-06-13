using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class BowMan : MonoBehaviour {

    public AudioClip audDynamit;
     public AudioClip audArrow;
    public float arrowForce = 500f;
    public float grenadeForce = 15f;
    public float grenadeRadius = 30f;
    Player player;
    public GameObject arrowPrefab;
    public GameObject grenadePrefab;
    public Transform arrowSpawn;
    bool firing = false;
    public float abilityCooldown = 5f;
    bool abilityCooldowning = false;
    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {

                if(player.Health <= 0) return;

        if(Input.GetButtonDown("Fire1") && !firing) StartCoroutine(conductAttackCo());   

        if(Input.GetButtonDown("Fire2") && !abilityCooldowning){
            StartCoroutine(doAbility());

        } 
    }

    IEnumerator doAbility(){
        abilityCooldowning = true;
        player.Animator.Play(Animator.StringToHash("Ability"));
        yield return new WaitForSeconds(.2f);
        conductAbility();
        yield return new WaitForSeconds(abilityCooldown);
        abilityCooldowning = false;
    }

    void conductAbility(){
        //todo: do rotation
         Vector3 rot = transform.rotation.eulerAngles;
        GameObject grenade = Instantiate(grenadePrefab, arrowSpawn.position, Quaternion.Euler(new Vector3(rot.x,rot.y+ (player.FacingLeft ? 180 : 0),rot.z)));
        Rigidbody2D grenadeRB = grenade.GetComponent<Rigidbody2D>();
        grenadeRB.AddForce(transform.right * grenadeForce * (transform.localScale.x > 0 ? 1 : -1), ForceMode2D.Impulse);
        StartCoroutine(tickerNade(grenade));
        AudioSource audio = GetComponent<AudioSource>();
    }

    IEnumerator tickerNade(GameObject grenade){
        yield return new WaitForSeconds(3f);

        grenade.GetComponent<Animator>().Play("explode");
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audDynamit);
        Collider2D[] cols = Physics2D.OverlapCircleAll(grenade.transform.position, grenadeRadius);
        foreach(Collider2D col in cols){
            if(col.GetComponent<Baddie>()){
                col.GetComponent<Baddie>().takeDamage();
            }
            if(col.GetComponent<Player>()){
                col.GetComponent<Player>().takeDamage();
            }
        }

        Destroy(grenade.gameObject, 1f);
    }

    void conductAttack(){
       
        

        //todo: do rotation
         Vector3 rot = transform.rotation.eulerAngles;
        
        
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.Euler(new Vector3(rot.x,rot.y+ (player.FacingLeft ? 180 : 0),rot.z)));
        Destroy(arrow, 5f);
        Rigidbody2D arrb = arrow.GetComponent<Rigidbody2D>();
        arrb.AddForce(transform.right * arrowForce * (transform.localScale.x > 0 ? 1 : -1), ForceMode2D.Impulse);
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audArrow);
    
    }

    IEnumerator conductAttackCo(){
        firing = true;
        player.Animator.SetTrigger(Animator.StringToHash("Hitting"));
        yield return new WaitForSeconds(.3f);
        conductAttack();
        firing = false;
    }

    void delay(){}
}
