using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnHandling : MonoBehaviour
{

    public List<Transform> spawnpoints;
    public List<GameObject> enemies;
    public float spawnsPerSecStart;
    public float spawnsPerSecClimb;
    public float SpawnsPerSec {get; private set; }
    
    public float orientationConfiguration = 0f; //todo - to determine which plane to spawn on lol
    GameHandler gameHandler;
    
    bool isActive = false;

    private void Start() {
        SpawnsPerSec = spawnsPerSecStart;

        gameHandler =  FindObjectOfType<GameHandler>();
        checkIsActive();
        StartCoroutine(spawnHandling());
    }

    public void checkIsActive() => isActive = orientationConfiguration == gameHandler.CurrRotation.x;

    IEnumerator spawnHandling(){
        while(isActive && !gameHandler.GameEnd){
            //spawn an AI 
            Instantiate(enemies[Random.Range(0, enemies.Count)], spawnpoints[Random.Range(0, spawnpoints.Count)].position, transform.rotation);
            yield return new WaitForSeconds(SpawnsPerSec);
        }
        yield break;
    }
}
