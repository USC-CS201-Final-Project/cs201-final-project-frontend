using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    public static SceneManager ins;
    
    public GameObject game;
    public GameObject connect;
    public GameObject login;
    public GameObject gameOver;
    public GameObject wait;

    public GameObject gameManager;

    public TMP_Text t_WPM;
    public TMP_Text t_gameOver;


    // Start is called before the first frame update
    void Awake()
    {
        if(ins == null){
            ins = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public static void EnterGame()
    {
        ins.wait.SetActive(false);
        ins.gameManager.SetActive(true);
        ins.game.SetActive(true);
    }

    public static void EnterLogIn()
    {
        ins.connect.SetActive(false);
        ins.login.SetActive(true);
    }

    public static void EnterGameOver(int wpm, bool b)
    {
        if(b) ins.t_gameOver.text = "You defeated the slime!";
        else ins.t_gameOver.text = "The slime defeated you!";
        ins.t_WPM.text = "WPM: "+wpm;
        ins.game.SetActive(false);
        ins.gameManager.SetActive(false);
        ins.gameOver.SetActive(true);
    }

    public static void EnterWait()
    {
        ins.login.SetActive(false);
        ins.gameOver.SetActive(false);
        ins.wait.SetActive(true);
    }

    public static void EnterConnect()
    {
        ins.gameOver.SetActive(false);
        ins.connect.SetActive(true);
    }
}
