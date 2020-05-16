using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] Text highscoreVal;
    public int buildindex = 1;
    public void Start()
    {
        //highscoreVal.text = PlayerPrefs.GetInt("HighScore",0).ToString();
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(buildindex, LoadSceneMode.Single);
    }

    public void setBuildIndex(int b)
    {
        buildindex = b;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
