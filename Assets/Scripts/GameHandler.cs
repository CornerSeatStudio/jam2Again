using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    public static float currScore;
    public Animator cameraAnimator;
    private bool inTransition = false;

    public void Start(){
        spawnNextPickup();
    }

    public void increaseScore(Player player){
        currScore++;
        changeCharacter(player);
        spawnNextPickup();
    }

    void changeCharacter(Player player){
        Debug.Log("changing char");
    }

    void spawnNextPickup(){
        Debug.Log("spawning pickup");

    }

    private void Update() {
        if(!inTransition && Input.GetKeyDown(KeyCode.X)){
            StartCoroutine(OnFlipActivate());
        }
    }
    public float zoomOutCameraSize;
    public IEnumerator OnFlipActivate(){
        inTransition = true;
        //pause time
        //freeze player - preserve momentum
        snapCameraOut();//zoom out
        
        //rotate stage (direction dependent)
        
        snapCameraIn(); //zoom in
        //unfreeze player
        //resume time
        inTransition = false;
        yield return null;
    }

    void snapCameraOut() => cameraAnimator.Play("OutCam");
    void snapCameraIn() => cameraAnimator.Play("GameCam");
}
