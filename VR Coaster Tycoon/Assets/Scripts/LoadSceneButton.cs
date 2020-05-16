using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }
}
