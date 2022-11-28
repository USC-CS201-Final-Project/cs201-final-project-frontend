using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    int playerHealth;
    int playerID;
    int playerDamage;
    int playerNumWords;

    string curWord;
    public Animator animation;
    //public Animation animation;

    PlayerInfo playerInfo;
    int playerHP;

    Enemy enemy;
    Animator animator;
    [SerializeField]HealthComponent healthBar;
    string jsonString;

    public enum State{
        Idle,
        Attack,
        Dead
    }

    State mCurState = State.Idle;
    //UnUsed variable for later use, update when have attack animation
    bool isInAttackAnimation = false;

    public bool ComparePlayer(int playerID)
    {
        return this.GetComponent<PlayerInfo>().playerID == playerID;
    }

    public void SetCurState(State state)
    {
        mCurState = state;
    }

    public State GetCurState()
    {
        return mCurState;
    }

    public void SetCurWord(string word)
    {
        curWord = word;
    }

    public string GetCurWord()
    {
        return curWord;
    }




    void Start()
    {
        jsonString = Resources.Load<TextAsset>("sampleJson").text;
        playerInfo = PlayerInfo.CreateFromJSON(jsonString);
        //animation = gameObject.GetComponent<Animation>();
        /*intialize enemy Uncomment this after enemy is implemented*/
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        
    }

    void Update()
    {
        playerInfo = PlayerInfo.CreateFromJSON(jsonString);
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
        //animation.Play("idle");
    }
    void UpdateAttack(){
        //play player attack animation (update isInAttackAnimation bool)

        //play Enemy damaged Animation
        //enemy.GetComponent<Animator>().Play("animation");

        mCurState=State.Idle;
    }

    void UpdateDeath(){

    }

    public void UpdatePlayerHealth(int curHealth){
        playerHP = curHealth;
        healthBar.SetHealth(curHealth/100);
    }

    public void UpdateCostumeSprite()
    {
        string hatName = "";
        //Get player costume sprite
        switch (playerInfo.ownedCustomes)
        {
            case -1:
                //-1 means no hat
                hatName = "";
                break;
            case 0:
                hatName = "hat0";
                break;
            case 1:
                hatName = "hat1";
                break;
            case 2:
                hatName = "hat2";
                break;
            case 3:
                hatName = "hat3";
                break;
            case 4:
                hatName = "hat4";
                break;
            default:
                hatName = "";
                break;
        }

        Sprite hatSprite = Resources.Load<Sprite>("hats/"+hatName);

        //Update costume sprite
        //playerHat.GetComponent<Image>().sprite = hatSprite;
    }

    public void NextCostume()
    {
        playerInfo.ownedCustomes = ((playerInfo.ownedCustomes+1) % 5) - 1;
        UpdateCostumeSprite();
        ServerManager.ins.ChangeCostume(playerInfo.ownedCustomes);
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
    public int ownedCustomes;

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
