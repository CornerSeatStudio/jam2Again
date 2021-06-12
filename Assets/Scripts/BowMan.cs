using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class BowMan : MonoBehaviour {

    public float arrowForce = 500f;
    Player player;
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    private void Start() {
        player = this.GetComponent<Player>();
    }
    private void Update() {
        if(Input.GetButtonDown("Fire1")) conductAttack();    
    }

    void conductAttack(){
        player.Animator.SetTrigger(Animator.StringToHash("Hitting"));
       
        //todo: do rotation
         Vector3 rot = transform.rotation.eulerAngles;
        
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.Euler(new Vector3(rot.x,rot.y+ (player.FacingLeft ? 180 : 0),rot.z)));
        Destroy(arrow, 5f);
        Rigidbody2D arrb = arrow.GetComponent<Rigidbody2D>();
        arrb.AddForce(transform.right * arrowForce * (transform.localScale.x > 0 ? 1 : -1), ForceMode2D.Impulse);
    

    }
}
