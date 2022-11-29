using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UIElements;
using TMPro;

public class Player : MonoBehaviour
{
    int playerHealth;
    int playerID;
    int playerDamage;
    int playerNumWords;

    public TMP_Text t_username;

    string curWord;

    public PlayerInfo playerInfo;
    int playerHP;

    [SerializeField]GameObject playerHat;

    Enemy enemy;
    Animator animator;
    AnimatorStateInfo animatorInfo;
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
        // jsonString = Resources.Load<TextAsset>("sampleJson").text;
        playerInfo = new PlayerInfo("",0,60,false,-1);
        animator = GetComponent<Animator>();
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        //animation = gameObject.GetComponent<Animation>();
        /*intialize enemy Uncomment this after enemy is implemented*/
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        
    }

    void Update()
    {
        // playerInfo = PlayerInfo.CreateFromJSON(jsonString);
        // UpdatePlayerHealth(playerInfo.health);
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
        animator.Play("playerIdle");
    }
    void UpdateAttack(){
        //play player attack animation (update isInAttackAnimation bool)
        animator.Play("playerAttack");
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime > 0.99f && animatorInfo.IsTag("Attack"))
        {
            mCurState=State.Idle;
            playerInfo.isAttacking = false;
            enemy.takingDamage=true;
        }
        
    }

    void UpdateDeath(){
        animator.Play("playerDead");
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime > 0.99f && animatorInfo.IsTag("Death"))
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdatePlayerHealth(int curHealth){
        playerHP = curHealth;
        healthBar.SetHealth((float)curHealth/(float)playerInfo.health);
    }

    public void UpdateCostumeSprite()
    {
        //Update costume sprite
        playerHat.GetComponent<SpriteRenderer>().sprite = ServerManager.ins.playerPool.GetComponent<PlayerPoolManager>().costumes[playerInfo.ownedCustomes];
    }

    public void NextCostume()
    {
        playerInfo.ownedCustomes = ((playerInfo.ownedCustomes+1) % 8);
        UpdateCostumeSprite();
        ServerManager.ins.ChangeCostume(playerInfo.ownedCustomes);
        Debug.Log("Next Costume called");
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

    public PlayerInfo (string n, int id, int hp, bool attack, int costume)
    {
        name = n;
        playerID = id;
        health = hp;
        isAttacking = attack;
        ownedCustomes = costume;
    }

}
