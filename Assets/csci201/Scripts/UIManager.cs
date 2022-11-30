using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager ins;

    public TMP_Text t_IP;
    public TMP_Text t_username;
    public TMP_Text t_password;

    public GameObject errorText;

    public Button b_connect;

    public Button b_login;
    public Button b_register;
    public Button b_guest;

    public Button b_costume;

    public Button b_playAgain;
    public Button b_quit;

    private void Awake() {
        if(ins == null){
            ins = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        b_connect.onClick.AddListener(connect);
        b_login.onClick.AddListener(login);
        b_register.onClick.AddListener(register);
        b_guest.onClick.AddListener(guest);
        b_playAgain.onClick.AddListener(playAgain);
        b_quit.onClick.AddListener(quit);
        b_costume.onClick.AddListener(costume);
    }


    void connect()
    {
        if(ServerManager.ins.Connect(t_IP.text))
            SceneManager.EnterLogIn();
        else
        {
            Debug.Log("Failed connect");
        }
        Debug.Log("connect pressed");
    }

    void login()
    {
        if(t_username.text==""||t_password.text=="") return;
        else errorText.SetActive(true);
        if(ServerManager.ins.LogIn(t_username.text,t_password.text)) SceneManager.EnterWait();
        else errorText.SetActive(true);
    }

    void register()
    {
        if(t_username.text==""||t_password.text=="") return;
        else errorText.SetActive(true);
        if(ServerManager.ins.Register(t_username.text,t_password.text)) SceneManager.EnterWait();
        else errorText.SetActive(true);
    }

    void guest()
    {
       if(ServerManager.ins.PlayGuest()) SceneManager.EnterWait();
    }

    void playAgain()
    {
        ServerManager.ins.PlayAgain(true);
        SceneManager.EnterWait();
    }

    void quit() 
    {
        ServerManager.ins.PlayAgain(false);
        SceneManager.EnterConnect();
        errorText.SetActive(false);
    }

    void costume()
    {
        if(!ServerManager.ins.inGameplay) return;
        Player[] players = ServerManager.ins.playerPool.GetComponentsInChildren<Player>();
        players[ServerManager.ins.clientIndex].NextCostume();
    }
}
