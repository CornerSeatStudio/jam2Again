using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public readonly static int MAIN_MENU_ID = 0;
    public readonly static int GAME_ID = 1;

    public void loadMainMenu() => loadScene(MAIN_MENU_ID);
    public void loadGame() => loadScene(GAME_ID);
    public void loadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);

    public void QuitGame() => Application.Quit();
    

}
