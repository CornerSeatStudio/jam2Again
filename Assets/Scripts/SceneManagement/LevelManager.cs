using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject coopUI;
    public readonly static int MAIN_MENU_ID = 0;
    public readonly static int GAME_ID = 1;

    public readonly static int CREDIT_ID = 2;
    public readonly static int TUT1 = 3;
    public readonly static int TUT2 = 4;


    public void loadMainMenu() => loadScene(MAIN_MENU_ID);

    public void loadCoopChoice(){
        coopUI.SetActive(true);
    }
    public void loadGame() {
        GameHandler.IsCoop = false;
        loadScene(GAME_ID);
    }
    public void loadCoopGame() {
        GameHandler.IsCoop = true;
        loadScene(GAME_ID);
    }

    public void loadCredit() => loadScene(CREDIT_ID);

    public void loadTut1() => loadScene(TUT1);
    public void loadTut2() => loadScene(TUT2);

    public void loadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);


    public void QuitGame() => Application.Quit();
    
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused= false;
    }
    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused= true;
    }

}
