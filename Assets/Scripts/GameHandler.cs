using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    public GameObject rotatingContraption;
    public static float currScore;
    public Animator cameraAnimator;
    public float rotSmoothness;
    private bool inTransition = false;

    private readonly Vector3 DIR1 = new Vector3(0f, 0f, 0f);
    private readonly Vector3 DIR2 = new Vector3(90f, 0f, 0f);
    private readonly Vector3 DIR3 = new Vector3(180f, 0f, 0f);
    private readonly Vector3 DIR4 = new Vector3(270f, 0f, 0f);
    private Vector3 currRotation;
    public void Start(){
        spawnNextPickup();
        //initial rotate stage? default for now
        currRotation = DIR1;
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

    public void onGameEnd(){
        Debug.Log("You're deaded");
        //display exit/restart 
    
    }

    private void Update() {
        if(!inTransition && Input.GetKeyDown(KeyCode.X)){
            StartCoroutine(OnFlipActivate(true));
        } else if(!inTransition && Input.GetKeyDown(KeyCode.Z)){
            StartCoroutine(OnFlipActivate(false));
        }
    }
    public float zoomOutCameraSize;
    public IEnumerator OnFlipActivate(bool dir){
        // Debug.Log(rotatingContraption.transform.rotation);
        inTransition = true;
        //pause time
        //freeze player - preserve momentum
        snapCameraOut();//zoom out
        
        yield return new WaitForSeconds(.5f);
        //rotate stage (direction dependent)
        yield return StartCoroutine(rotateStage(dir));

        snapCameraIn(); //zoom in
        //unfreeze player
        //resume time
        inTransition = false;
        Debug.Log("finished flip");
        yield return null;
    }

    public Vector3 nextRot(bool pos){
        if(currRotation == DIR1){
            return pos ? DIR2 : DIR4;
        } else if (currRotation == DIR2){
            return pos ? DIR3 : DIR1;
        } else if(currRotation == DIR3){
            return pos ? DIR4 : DIR2;
        } else if(currRotation == DIR4){
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

        // Debug.Log($"curr: {startRotation}");
        // Debug.Log($"next: {endRotation}");

        // Debug.Log(endRotation);

        // Quaternion startRotation = rotatingContraption.transform.rotation;
        // Vector3 goalRotVec = new Vector3(rotatingContraption.transform.rotation.eulerAngles.x + 90, rotatingContraption.transform.rotation.eulerAngles.y , rotatingContraption.transform.rotation.eulerAngles.z);
        // Quaternion goalRotation = Quaternion.Euler(goalRotVec);
        float t = 0f;
        while(t < 1){
            // Debug.Log(initDir);
            // Debug.Log(goalDir);
            rotatingContraption.transform.eulerAngles = simpleLerp(currRotation, endRotation, t);
            // Debug.Log(simpleLerp(currRotation.eulerAngles, endRotation, t));
            // rotatingContraption.transform.e(Vector3.Lerp(transform.rotation.eulerAngles, transform.rotation.eulerAngles + Vector3.right * 90 * dir, t));
            t += Time.deltaTime * rotSmoothness;
            yield return new WaitForEndOfFrame();
        }

        rotatingContraption.transform.eulerAngles = endRotation;
        currRotation = endRotation;

    }


    void snapCameraOut() => cameraAnimator.Play("OutCam");
    void snapCameraIn() => cameraAnimator.Play("GameCam");
}
