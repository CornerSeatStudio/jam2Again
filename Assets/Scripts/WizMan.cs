using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class WizMan : MonoBehaviour {

    public AudioClip audExplod;
    public AudioClip audCast;
    Player player;
    public float orbLifetime = 5f;
    public GameObject orbPrefab;
    public Transform orbSpawnPos;
    public float orbMoveSpeed;
    public float explosionRadius = 8f;

    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {
        if(Input.GetButtonDown("Fire1")) conductAttack();    
    }

    void conductAttack(){
        player.Animator.SetTrigger(Animator.StringToHash("Hitting"));
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audCast);
        GameObject orb = Instantiate(orbPrefab, orbSpawnPos.position, transform.rotation);
        StartCoroutine(handleOrb(orb.GetComponent<Rigidbody2D>(), !player.FacingLeft ? 1 : -1));
        // orbRB.AddForce(transform.right * arrowForce * , ForceMode2D.Impulse);

    }

    IEnumerator handleOrb(Rigidbody2D orbRB, float launchDir){
        float t = 0;
        while(t < orbLifetime){
            orbRB.velocity = transform.right * orbMoveSpeed * launchDir;
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }   
        orbRB.velocity = Vector2.zero;
        explodeOrb(orbRB.gameObject);
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

