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

    public string pain;

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
        pain = t_username.text;
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
        Debug.Log("Username = '"+t_username.text+"'. "+string.IsNullOrEmpty(t_username.text));
        Debug.Log("Password = '"+t_password.text+"'. "+string.IsNullOrEmpty(t_password.text));
        if(t_username.text==pain||t_password.text==pain) errorText.SetActive(true);
        else{
            if(ServerManager.ins.LogIn(t_username.text,t_password.text)) SceneManager.EnterWait();
            else errorText.SetActive(true);
        }
    }

    void register()
    {
        if(t_username.text==pain||t_password.text==pain) errorText.SetActive(true);
        else{
            if(ServerManager.ins.Register(t_username.text,t_password.text)) SceneManager.EnterWait();
            else errorText.SetActive(true);
        }
    }

    void guest()
    {
       if(ServerManager.ins.PlayGuest()) SceneManager.EnterWait();
       else errorText.SetActive(true);
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
