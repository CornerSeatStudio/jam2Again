using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    public AudioClip audCOIN;

    public Player Player1 {get; private set; }
    public Player Player2 {get; private set; }
    public GameObject rotatingContraption;
    public List<GameObject> characters;
    public static float currScore;
    public Animator cameraAnimator;
    public float rotSmoothness;
    public GameObject pickup;
    public List<Transform> pickupSpawnpoints;
    public GameObject portalPrefab;
    public Transform playerSpawnPos1;
    public Transform playerSpawnPos2;
    public bool InTransition {get; set; }= false;

    private static readonly Vector3 DIR1 = new Vector3(0f, 0f, 0f);
    private static readonly Vector3 DIR2 = new Vector3(90f, 0f, 0f);
    private static readonly Vector3 DIR3 = new Vector3(180f, 0f, 0f);
    private static readonly Vector3 DIR4 = new Vector3(270f, 0f, 0f);
    public Vector3 CurrRotation {get; private set;}
    public UnityEvent preFlipEvent;

    public UnityEvent postFlipEvent;
    public event Action<int> onPickupEvent;
    public bool GameEnd {get; private set; } = false;
    public GameObject pauseMenu;
    public GameObject deathMenu;

    int CURR_INDEX;
    public static bool IsCoop {get; set; } = false;
    public void Start(){
        spawnNextPickup();
        currScore = 0f;
        //spawn player(s) and link them to the target
        CURR_INDEX = UnityEngine.Random.Range(0, characters.Count);
        GameObject newPlayer = Instantiate(characters[CURR_INDEX], playerSpawnPos1.position, playerSpawnPos1.rotation);
        Player1 = newPlayer.GetComponent<Player>();
        Player1.Player_ID = 0;
        if(IsCoop){
            GameObject newPlayer2 = Instantiate(characters[(CURR_INDEX+1) % 3], playerSpawnPos2.position, playerSpawnPos2.rotation);
            Player2 = newPlayer2.GetComponent<Player>();
            Player2.Player_ID = 2;
        }
        
        //initial rotate stage? default for now
        CurrRotation = DIR1;
        GameEnd = false;
    }

    public void increaseScore(Player oldPlayer){
        currScore++;
        onPickupEvent?.Invoke((int)currScore);
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audCOIN);
        changeCharacter(oldPlayer);
        spawnNextPickup();
    }

    void changeCharacter(Player oldPlayer){
        // Debug.Log("changing char");
        //for now allow to switch to same character by chance
        
        //POOF and switcheroo
        int newPlayerIndex = UnityEngine.Random.Range(0, characters.Count);
        if(CURR_INDEX == newPlayerIndex) newPlayerIndex = (newPlayerIndex + 1)%3;
        CURR_INDEX = newPlayerIndex;
        GameObject newPlayer = Instantiate(characters[newPlayerIndex], oldPlayer.transform.position, oldPlayer.transform.rotation);
        Player1 = newPlayer.GetComponent<Player>();
        Player1.Health = oldPlayer.Health;
        Player1.Player_ID = oldPlayer.Player_ID;
        oldPlayer.gameObject.SetActive(false);

    }

    void spawnNextPickup(){
        // Debug.Log("spawning pickup");

        //spawn orientation depends on list
        Transform toSpawn = pickupSpawnpoints[UnityEngine.Random.Range(0, pickupSpawnpoints.Count)];
        Instantiate(pickup, toSpawn.position, toSpawn.rotation);

    }

    public void onGameEnd(){
        GameEnd = true;
        deathMenu.SetActive(true);
        //display exit/restart 
    
    }

    private void Update() {
        if(!InTransition && Input.GetKeyDown(KeyCode.X)){
            StartCoroutine(OnFlipActivate(true));
        } else if(!InTransition && Input.GetKeyDown(KeyCode.Z)){
            StartCoroutine(OnFlipActivate(false));
        }

        if(Input.GetKeyDown(KeyCode.P)) {
            if(paused){
                resumeGame();
            } else {
                pauseGame();
            }
        };

    }
    bool paused = false;
    void pauseGame(){
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void resumeGame(){
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

    }

    public float zoomOutCameraSize;
    public IEnumerator OnFlipActivate(bool dir){
        // Debug.Log(rotatingContraption.transform.rotation);
        InTransition = true;

        preFlipEvent?.Invoke(); //freeze AI
        
        //freeze player - preserve momentum
        Rigidbody2D player1RB =  Player1.GetComponent<Rigidbody2D>();
        Vector3 velBackup = player1RB.velocity;
        player1RB.velocity = Vector3.zero;

        //Finally freeze the body in place so forces like gravity or movement won't affect it
        player1RB.constraints = RigidbodyConstraints2D.FreezeAll;

        snapCameraOut();//zoom out
        
        yield return new WaitForSeconds(.5f);
        //rotate stage (direction dependent)
        yield return StartCoroutine(rotateStage(dir));

        snapCameraIn(); //zoom in
        //unfreeze player
        player1RB.constraints = RigidbodyConstraints2D.None;
        player1RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        player1RB.velocity = velBackup;

        postFlipEvent?.Invoke();

        //resume time
        InTransition = false;
        Debug.Log("finished flip");
        yield return null;
    }

    public Vector3 nextRot(bool pos){
        if(CurrRotation == DIR1){
            return pos ? DIR2 : DIR4;
        } else if (CurrRotation == DIR2){
            return pos ? DIR3 : DIR1;
        } else if(CurrRotation == DIR3){
            return pos ? DIR4 : DIR2;
        } else if(CurrRotation == DIR4){
            return pos ? DIR1 : DIR3;
        } else {
            return Vector3.zero;
        }
    }

    public Vector3 simpleLerp(Vector3 startVec, Vector3 endVec, float t){
        float startVecVal = startVec.x;
        float endVecVal = endVec.x;
        if(startVec == DIR1 && endVec == DIR4) {
            startVecVal = 0f;
            endVecVal = -90f;
        }
        if(startVec == DIR4 && endVec == DIR1) {
            startVecVal = 270f;
            endVecVal = 360f;
        }

        return new Vector3(Mathf.Lerp(startVec.x, endVecVal, t), 0f, 0f);
    }

    public IEnumerator rotateStage(bool pos){

        // yield break;
        Vector3 endRotation = nextRot(pos);

    
        float t = 0f;
        while(t < 1){
            rotatingContraption.transform.eulerAngles = simpleLerp(CurrRotation, endRotation, t);
            // Debug.Log(simpleLerp(currRotation.eulerAngles, endRotation, t));
            // rotatingContraption.transform.e(Vector3.Lerp(transform.rotation.eulerAngles, transform.rotation.eulerAngles + Vector3.right * 90 * dir, t));
            t += Time.deltaTime * rotSmoothness;
            yield return new WaitForEndOfFrame();
        }

        rotatingContraption.transform.eulerAngles = endRotation;
        CurrRotation = endRotation;

    }


    void snapCameraOut() => cameraAnimator.Play("OutCam");
    void snapCameraIn() => cameraAnimator.Play("GameCam");
}
