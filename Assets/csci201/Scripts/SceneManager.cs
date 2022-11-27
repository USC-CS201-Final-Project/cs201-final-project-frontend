using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager ins;
    
    public GameObject game;
    public GameObject connect;
    public GameObject login;
    public GameObject gameOver;
    public GameObject wait;


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
        GameManager.ins.gameObject.SetActive(true);
        ins.game.SetActive(true);
    }

    public static void EnterLogIn()
    {
        ins.connect.SetActive(false);
        ins.login.SetActive(true);
    }

    public static void EnterGameOver()
    {
        ins.game.SetActive(false);
        GameManager.ins.gameObject.SetActive(false);
        ins.gameOver.SetActive(true);
    }

    public static void EnterWait()
    {
        ins.login.SetActive(false);
        ins.wait.SetActive(true);
    }

    public static void EnterConnect()
    {
        ins.gameOver.SetActive(false);
        ins.connect.SetActive(true);
    }
}
