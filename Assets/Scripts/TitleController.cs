using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Text highscore;
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }
    public void OnStartButtonClicked2()
    {
        SceneManager.LoadScene("Main2");
    }
    public void OnStartButtonClicked3()
    {
        SceneManager.LoadScene("title");
    }
    // Start is called before the first frame update
    void Start()
    {
        highscore.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
