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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update1 is called once per frame
    void Update()
    {
        int score = CalcScore();
        scoretext.text = "Score" + score + "m";
        lifePanel.UpdateLife(nejiko.Life());
        if (nejiko.Life() <= 0)
        {
            enabled = false;

            Invoke("ReturnToTitle", 1.5f);
        }
    }
    //0 -0.35 7.75
    int CalcScore()
    {
        return (int)nejiko.transform.position.z;
    }
    void ReturnToTitle()
    {
        SceneManager.LoadScene("title");
    }

}
