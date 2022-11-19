using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System;


public class ServerManager : MonoBehaviour
{
    [SerializeField] GameObject playerPool;
    public static ServerManager ins;
    private StreamReader sr;
    private StreamWriter sw;
    private NetworkStream s;

    private bool isConnected = false;
    private bool inGameplay = false;
    private bool loggedIn = false;

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
    void FixedUpdate()
    {
        if (isConnected && loggedIn && s.DataAvailable)
        {
            if(!inGameplay)
            {
                StartGame();
                inGameplay = true;
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
            TcpClient client = new TcpClient(ip, 8080);
            Debug.Log("connected to " +ip);
            s = client.GetStream();
            sr = new StreamReader(s);
            sw = new StreamWriter(s);
            Debug.Log("streams created");
            sw.AutoFlush = true;
        }
        catch (SocketException e) {
            Debug.Log("Failed to connect");
            Debug.Log(e);
            return false;
        }
        return true;
    }


    // Log in functionality

    public bool Register(string username, string password)
    {
        ClientAuthentication c = new ClientAuthentication(username,password,false,true);
        return Authenticate(c);
    }

    public bool LogIn(string username, string password)
    {
        ClientAuthentication c = new ClientAuthentication(username,password,false,false);
        return Authenticate(c);
    }

    public bool PlayGuest()
    {
        ClientAuthentication c = new ClientAuthentication("","",true,false);
        return Authenticate(c);
    }

    private bool Authenticate(ClientAuthentication c) {
        // Write to Server
        sw.WriteLine(JsonUtility.ToJson(c));
        // Receive from Server
        ServerAuthentication sa = JsonUtility.FromJson<ServerAuthentication>(sr.ReadLine());
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
    }

    // Client Gameplay functionality

    public void ChangeCostume()
    {
        sw.WriteLine(JsonUtility.ToJson(new ClientGameplay(false,0)));
    }

    public void CompleteWord()
    {
        sw.WriteLine(JsonUtility.ToJson(new ClientGameplay(true,0)));
    }

    // Server Gameplay functionality

    public void HandleGameplay()
    {
        ServerGameplay s = JsonUtility.FromJson<ServerGameplay>(sr.ReadLine());
        if(s.packetID==0)
        {
            BossAttack(s.playerHP);
        }
        else if(s.packetID==1) 
        {
            CostumeChange(s.costumeID);
        }
        else if(s.packetID==2)
        {
            PlayerAttack(s.playerID,s.bossHP,s.newWord);
        }
    }

    public void BossAttack(int playerHP)
    {

    }

    public void CostumeChange(int[] CostumeChange)
    {

    }

    public void PlayerAttack(int playerID, int bossHP, string newWord)
    {
        //Find the corresponding player that is attacking
        Player[] players = playerPool.GetComponentsInChildren<Player>();
        Player attackingPlayer = Array.Find(players, element => element.ComparePlayer(playerID));

        attackingPlayer.SetCurState(Player.State.Attack);
        attackingPlayer.SetCurWord(newWord);
    }


    // Game over functionality

    public void GameOver()
    {
        inGameplay = false;
        ServerGameOver g = JsonUtility.FromJson<ServerGameOver>(sr.ReadLine());
    }
}
