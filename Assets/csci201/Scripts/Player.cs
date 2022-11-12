using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Player : MonoBehaviour
{

    int playerHealth;
    int playerID;
    int playerDamage;
    int playerNumWords;
    public Animator animation;
    //public Animation animation;

    PlayerInfo playerInfo;
    int playerHP;

    GameObject enemy;
    [SerializeField]HealthComponent healthBar;

    enum State{
        Idle,
        Attack,
        Dead
    }

    State mCurState = State.Idle;
    //UnUsed variable for later use, update when have attack animation
    bool isInAttackAnimation = false;
    
    string filePath = "D:/cs201-final-project-frontend/Assets/csci201/Scripts";
    void Start()
    {
        playerInfo = PlayerInfo.CreateFromJSON(filePath,"sampleJson.json");
        //animation = gameObject.GetComponent<Animation>();
        /*intialize enemy Uncomment this after enemy is implemented*/
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    void Update()
    {
        playerInfo = PlayerInfo.CreateFromJSON(filePath,"sampleJson.json");
        UpdatePlayerHealth(playerInfo.health);
        UpdatePlayerState();
        UpdatePlayerAnimation();
    }

    void UpdatePlayerState(){
        if(playerHP <= 0){
            mCurState = State.Dead;
        }
        else if(playerInfo.isAttacking){
            mCurState = State.Attack;
        }else if(!isInAttackAnimation){
            mCurState = State.Idle;
        }
    }

    void UpdatePlayerAnimation(){
        switch(mCurState){
            case State.Idle:
                UpdateIdle();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Dead:
                UpdateDeath();
                break;
        }
    }

    void UpdateIdle(){
        animation.Play("idle");
    }
    void UpdateAttack(){

    }

    void UpdateDeath(){

    }

    void UpdatePlayerHealth(int curHealth){
        playerHP = curHealth;
        healthBar.SetHealth(curHealth/100);
    }


}

public class PlayerInfo{
    [HideInInspector]
    public string name;
    [HideInInspector]
    public int playerID;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public bool isAttacking;

    public static PlayerInfo CreateFromJSON(string fileFolder, string fileName)
    {
        string fullPath = Path.Combine(fileFolder, fileName);
        PlayerInfo loadedData = null;
        if (File.Exists(fullPath)) {
            string loadData = "";
            using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                using (StreamReader reader = new StreamReader(stream)) {
                    loadData = reader.ReadToEnd();
                }
            }
            loadedData = JsonUtility.FromJson<PlayerInfo>(loadData);
        }
        return loadedData;
    }

    public static PlayerInfo CreateFromJSON(string jsonString){
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }

}
