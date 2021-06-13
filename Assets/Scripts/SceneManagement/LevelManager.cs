using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public readonly static int MAIN_MENU_ID = 0;
    public readonly static int GAME_ID = 1;

    public readonly static int CREDIT_ID = 2;

    public void loadMainMenu() => loadScene(MAIN_MENU_ID);
    public void loadGame() => loadScene(GAME_ID);

    public void loadCredit() => loadScene(CREDIT_ID);
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
