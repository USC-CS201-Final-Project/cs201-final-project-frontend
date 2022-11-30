using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System;


public class ServerManager : MonoBehaviour
{
    public GameObject playerPool;
    [SerializeField] GameObject enemy;
    public static ServerManager ins;
    private StreamReader sr;
    private StreamWriter sw;
    private NetworkStream s;

    private bool isConnected = false;
    public bool inGameplay = false;
    private bool loggedIn = false;

    public string userID;
    public int clientIndex = 0;

    void Awake()
    {
        if (!ins)
        {
            ins = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
    }

    // Check if there is any data to be read from the server.
    void Update()
    {
        if (isConnected && loggedIn && s.DataAvailable)
        {
            if(!inGameplay)
            {
                StartGame();
            }
            else
            {
                HandleGameplay();
            }
        }
    }


    // handle initial connection to server

    public bool Connect(string ip = "localhost")
    {
        try{
            Debug.Log("connected to " +ip);
            TcpClient client = new TcpClient();
            client.Connect(ip,8080);
            
            s = client.GetStream();
            sr = new StreamReader(s);
            sw = new StreamWriter(s);
            sr.BaseStream.ReadTimeout = 4000;
            Debug.Log("streams created");
            sw.AutoFlush = true;
        }
        catch (SocketException e) {
            Debug.Log("Failed to connect");
            Debug.Log(e);
            return false;
        }
        isConnected = true;
        return true;
    }


    // Log in functionality

    public bool Register(string username, string password)
    {
        userID = username;
        ClientAuthentication c = new ClientAuthentication(username,password,false,true);
        return Authenticate(c);
    }

    public bool LogIn(string username, string password)
    {
        userID = username;
        ClientAuthentication c = new ClientAuthentication(username,password,false,false);
        return Authenticate(c);
    }

    public bool PlayGuest()
    {
        userID = "Guest";
        ClientAuthentication c = new ClientAuthentication("","",true,false);
        return Authenticate(c);
    }

    private bool Authenticate(ClientAuthentication c) {
        // Write to Server
        sw.WriteLine(JsonUtility.ToJson(c));
        // Receive from Server
        ServerAuthentication sa = JsonUtility.FromJson<ServerAuthentication>(sr.ReadLine());
        Debug.Log("made it past read line");
        // Handle
        if(sa.isValid){
            loggedIn = true;
            return true;
        }
        return false;
    }

    // Game Start functionality

    public void StartGame()
    {
        ServerGameStart g = JsonUtility.FromJson<ServerGameStart>(sr.ReadLine());
        playerPool.GetComponent<PlayerPoolManager>().InstantiatePlayer(g.usernames,g.startingPlayerHealth,g.startingBossHealth,g.startingWord,g.startingCostumeID);
        //playerPool.GetComponent<PlayerPoolManager>().InstantiatePlayer();
        SceneManager.EnterGame();
        for(int i = 0; i < g.usernames.Length; i++)
        {
            if(g.usernames[i]==userID)
            {
                clientIndex = i;
                GameManager.setWord(g.startingWord[i]);
            }
        }
        inGameplay = true;
    }

    // Client Gameplay functionality

    public void ChangeCostume(int id)
    {
        sw.WriteLine(JsonUtility.ToJson(new ClientGameplay(false,id)));
        Debug.Log("Sending Costume Change");
    }

    public void CompleteWord()
    {
        sw.WriteLine(JsonUtility.ToJson(new ClientGameplay(true,-1)));
        Debug.Log("Sending attack to server");
    }

    // Server Gameplay functionality

    public void HandleGameplay()
    {
        string s = sr.ReadLine();
        ServerGameplay st = JsonUtility.FromJson<ServerGameplay>(s);
        Debug.Log(s);
        if(st.packetID==1)
        {
            Debug.Log(st.playerHP);
            BossAttack(st.playerHP);
        }
        else if(st.packetID==2) 
        {
            CostumeChange(st.costumeID);
            Debug.Log("costume change from server");
        }
        else if(st.packetID==3)
        {
            PlayerAttack(st.playerID,st.bossHP,st.newWord);
        }
        if(st.packetID==0)
        {
            Player[] players = playerPool.GetComponentsInChildren<Player>();
            bool isWin = (players[0].playerHP > 0);
            GameOver(isWin,s);
        }
        else
        {
            sw.WriteLine(JsonUtility.ToJson(new ClientGameplay(false,-2)));
        }
    }

    public void BossAttack(int playerHP)
    {
        Debug.Log("Performing boss attack");
        Enemy[] boss = playerPool.GetComponentsInChildren<Enemy>();
        boss[0].enemyInfo.isAttacking=true;

        Player[] players = playerPool.GetComponentsInChildren<Player>();
        foreach (Player playerUnderAttack in players)
            playerUnderAttack.UpdatePlayerHealth(playerHP);
    }

    public void CostumeChange(int[] CostumeChange)
    {
        Player[] players = playerPool.GetComponentsInChildren<Player>();
        for(int i = 0; i< players.Length; i++){
            if(i!=clientIndex) players[i].playerInfo.ownedCustomes = CostumeChange[i];
            players[i].UpdateCostumeSprite();
        }
    }

    public void PlayerAttack(int playerID, int bossHP, string newWord)
    {
        Debug.Log("Performing player attack");
        //Find the corresponding player that is attacking
        Player[] players = playerPool.GetComponentsInChildren<Player>();
        Player attackingPlayer = players[playerID];

        attackingPlayer.playerInfo.isAttacking = true;
        attackingPlayer.SetCurWord(newWord);

        if(clientIndex==playerID) GameManager.setWord(newWord);

        Enemy[] boss = playerPool.GetComponentsInChildren<Enemy>();
        boss[0].UpdateEnemyHealth(bossHP);

    }


    // Game over functionality

    public void GameOver(bool b,string s)
    {
        ServerGameOver g = JsonUtility.FromJson<ServerGameOver>(s);
        //inGameplay = false;
        StartCoroutine(GameOverSequence(g.wordsPerMinute,b));
    }

    IEnumerator GameOverSequence(int WPM, bool isWin)
    {
        yield return new WaitForSeconds(.5f);
        playerPool.GetComponent<PlayerPoolManager>().DestroyPlayers();
        SceneManager.EnterGameOver(WPM, isWin);
    }

    public void PlayAgain(bool b)
    {
        sw.WriteLine(JsonUtility.ToJson(new ClientPlayAgain(b)));
        if(!b) Disconnected();
    }

    public void Disconnected() {
        isConnected = false;
        loggedIn = false;
        inGameplay = false;
        s.Close();
    }
}
