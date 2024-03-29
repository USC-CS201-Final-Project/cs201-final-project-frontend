using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClientAuthentication
{
    public string username;
    public string password;
    public bool isGuest;
    public bool registering;

    public ClientAuthentication(string u, string p, bool g, bool r)
    {
        username = u;
        password = p;
        isGuest = g;
        registering = r;
    }
}

[System.Serializable]
public class ServerAuthentication
{
    public bool isValid;

    public ServerAuthentication(bool v) {
        isValid = v;
    }
}

[System.Serializable]
public class ServerGameStart
{
    public string[] usernames;
    public int startingPlayerHealth;
    public int startingBossHealth;
    public string[] startingWord;
    public int[] startingCostumeID;
    public int bossCostumeID;
    public int playerID;

    public ServerGameStart(string[] u, int sph, int sbh, string[] sw, int[] scid, int bcid)
    {
        usernames = u;
        startingPlayerHealth = sph;
        startingBossHealth = sbh;
        startingWord = sw;
        startingCostumeID = scid;
        bossCostumeID = bcid;
    }

}

[System.Serializable]
public class ServerGameplay
{
    public int packetID;
    public int bossHP;
    public int playerHP;
    public string newWord;
    public int playerID;
    public int[] costumeID;

    public ServerGameplay(int pid, int bhp, int php, string w, int p, int[] cid) {
        packetID = pid;
        bossHP = bhp;
        playerHP = php;
        newWord = w;
        playerID = p;
        costumeID = cid;
    }
}

[System.Serializable]
public class ClientGameplay
{
    public bool completedWord;
    public int costumeID;
    public int packetID;
    public ClientGameplay(bool c, int cid)
    {
        completedWord = c;
        costumeID = cid;
        if(c) packetID = 6;
        else packetID = 5;
    }
}

[System.Serializable]
public class ServerGameOver
{
    public int wordsPerMinute;
    public ServerGameOver(int wpm)
    {
        wordsPerMinute = wpm;
    }
}

[System.Serializable]
public class ClientPlayAgain
{
    public bool playAgain;
    public int packetID;
    public ClientPlayAgain(bool p,int PID=4)
    {
        playAgain = p;
        packetID = PID;
    }
}
