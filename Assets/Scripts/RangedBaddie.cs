using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedBaddie : Baddie {


    void Update(){
        coolDown = 5;
        atkDistance = 40;
        if(aIPath.desiredVelocity.x >= 0.01){
            transform.localScale = new Vector2(-1f, 1f);
        } else if(aIPath.desiredVelocity.x <= -0.01) {
            transform.localScale = new Vector2(1f, 1f);
        }
        if(Time.time > this.NextAttack && checkAttackPosition()) {
            attack();
            this.NextAttack = Time.time + coolDown;
        }
    }
    public void attack(){
        // THis is supposed to be a placeholder for a physical attack
        conductAttack();
        
    }

    public bool checkAttackPosition(){
        if (Physics2D.Raycast(this.transform.position, gameHandler.Player1.transform.position, (float)atkDistance)){
            return true;
        } else {
            return false;
        }
    }

    public AudioClip audBullet;
    public float bulletForce = 500000000000000f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void conductAttack(){

        //todo: do rotation
        bool left;
        Vector3 rot = this.transform.rotation.eulerAngles;
        if(aIPath.desiredVelocity.x >= 0.0) {
            left = true;
        } else {
            left = false;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.Euler(new Vector3(rot.x,rot.y+ (left ? 180 : 0),rot.z)));
        Destroy(bullet, 5f);
        Rigidbody2D bulletB = bullet.GetComponent<Rigidbody2D>();
        bulletB.AddForce((gameHandler.Player1.transform.position - this.transform.position) * bulletForce);
        AudioSource audio = GetComponent<AudioSource>();

    }



}
