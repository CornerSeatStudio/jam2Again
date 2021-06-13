using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    
    Text score;
    GameHandler gameHandler;
    void Start()
    {
        score = GetComponent<Text>();
        gameHandler = FindObjectOfType<GameHandler>();
        gameHandler.onPickupEvent += onScoreUpdate;
    }

    // Update is called once per frame
    public void onScoreUpdate(int newScore){
        score.text = newScore.ToString();
    }
}
