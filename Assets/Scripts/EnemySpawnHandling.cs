using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySpawnHandling : MonoBehaviour
{
    public AudioClip audPortal;

    public enum level {GREEN, WEEB, SAND, COMB};
    public GameObject levelObject;
    public List<Transform> spawnpoints;
    public List<GameObject> enemies;
    public float spawnsPerSecStart;
    public float spwansPerSecCap = .5f;
    public float SpawnsPerSec {get; private set; }
    public level thisLevel;
    public static Dictionary<level, float> orientationMapping = new Dictionary<level, float>(){
        { level.GREEN, 0f },
        { level.WEEB, 270f },
        { level.SAND, 180f },
        { level.COMB, 90f }
    };
    GameHandler gameHandler;

    public List<Baddie> activeBaddies;

    bool isActive = false;

    private void Start() {
        SpawnsPerSec = spawnsPerSecStart;

        gameHandler =  FindObjectOfType<GameHandler>();
        isActive = orientationMapping[thisLevel] == gameHandler.CurrRotation.x;
        StartCoroutine(spawnHandling());
    }


    public void checkIsActiveSubscriber() {
        isActive = orientationMapping[thisLevel] == gameHandler.CurrRotation.x;
        // Debug.Log(orientationMapping[thisLevel]);
        // Debug.Log(gameHandler.CurrRotation.x);
        SpawnsPerSec = spawnsPerSecStart;
        if(isActive) reanimateOutPlane();
    }

    IEnumerator spawnHandling(){
        while(!gameHandler.GameEnd){
            if(!isActive) {
                yield return new WaitForSeconds(SpawnsPerSec);
                continue;
            }
            //spawn an AI 

            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audPortal);
            Debug.Log(gameHandler.portalPrefab);
            Vector3 spawnPosition = spawnpoints[Random.Range(0, spawnpoints.Count)].position;
            GameObject portal = Instantiate(gameHandler.portalPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(1f);
            
            if(gameHandler.InTransition) {
                portal.GetComponent<Animator>().Play(Animator.StringToHash("portalclose"));
                Destroy(portal, 1f);  
                yield return new WaitForSeconds(SpawnsPerSec);
                continue;
            }
            
            GameObject go = Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPosition, transform.rotation);
            Baddie temp = go.GetComponent<Baddie>();
            activeBaddies.Add(temp);

            temp.onDeathEvent += deathHandlingSubscriber;
            go.GetComponent<AIDestinationSetter>().target = gameHandler.Player1.transform; //temp if we want coop

            yield return new WaitForSeconds(1f);
            portal.GetComponent<Animator>().Play(Animator.StringToHash("portalclose"));
            Destroy(portal, 1f);    

            yield return new WaitForSeconds(SpawnsPerSec);
            SpawnsPerSec = Mathf.Max(SpawnsPerSec - .4f, spwansPerSecCap);
        }
        yield break;
    }

    public void deathHandlingSubscriber(Baddie deadBloke){
        activeBaddies.Remove(deadBloke);
    }

    private void Update() {
        if(gameHandler.GameEnd){
            foreach(Baddie bd in activeBaddies.ToArray()){
                bd.takeDamage();
            }
            

        }
    }
    public void flattenIntoPlaneSubscriber(){
        // Debug.Log("OSDFJ");
        if(isActive) { //if we're flattening this plane specifically
            isActive = false;
            foreach(Baddie bd in activeBaddies){
                // Debug.Log(bd.name);
                bd.GetComponent<AIPath>().canMove = false;
                bd.GetComponent<AIPath>().canSearch = false;

                bd.transform.parent = levelObject.transform;
                
            }
        }
        // Debug.Log("idk");
    }

    void reanimateOutPlane(){
        // Debug.Log("sdf");
        foreach(Baddie bd in activeBaddies){
            if(bd == null) break;
            bd.transform.parent = null;  
            bd.GetComponent<AIPath>().canMove = true;    
            bd.GetComponent<AIPath>().canSearch = true;
            // bd.GetComponent<AIPath>().
        }
    }
}
