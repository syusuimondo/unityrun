using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameConroller : MonoBehaviour
{
    public NejikoContoller nejiko;
    public Text scoretext;
    public LifePanel lifePanel;
    public BunPanel bunPanel;
    
    int bounas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update1 is called once per frame
    void Update()
    {
        int score = CalcScore() + bounas;
        scoretext.text = "Score" + score + "m";
        lifePanel.UpdateLife(nejiko.Life());
        bunPanel.UpdateBun(nejiko.Count());
        if (nejiko.Life() <= 0)
        {
            enabled = false;
            if (PlayerPrefs.GetInt("HighScore")<score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            Invoke("ReturnToTitle", 1.5f);
        }
    }
    //0 -0.35 7.75
    int CalcScore()
    {
        return (int)nejiko.transform.position.z;
    }
    public void GetScore()
    {
        bounas += 100;
    }
    void ReturnToTitle()
    {
        SceneManager.LoadScene("title");
    }

}
