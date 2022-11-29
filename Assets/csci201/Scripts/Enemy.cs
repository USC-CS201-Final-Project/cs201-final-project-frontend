using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int enemyHealth=100;
    int enemyDamage;
    public Animator enemyAnimatior;
    AnimatorStateInfo animatorInfo;
    GameObject playerPool;
    State enemyCurState = State.Idle;
    int targetPlayerID;
    [SerializeField] HealthComponent healthBar;
    public EnemyInfo enemyInfo;
    bool isInAttackAnimation = false;
    public bool takingDamage = false;

    //call this function to update the enemy's target (should be passed in json package)

    public enum State
    {
        Attack,
        Idle,
        Death,
        Damage
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Enemy";
        enemyInfo = new EnemyInfo(100,false);
        enemyAnimatior = GetComponent<Animator>();
        animatorInfo = enemyAnimatior.GetCurrentAnimatorStateInfo(0);
        enemyAnimatior.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateEnemyHealth(enemyInfo.health);
        UpdateEnemyState();
        UpdateEnemyAnimation();
    }

    void UpdateEnemyState()
    {
        if (enemyHealth <= 0)
        {
            enemyCurState = State.Death;
        }
        else if (enemyInfo.isAttacking)
        {
            enemyCurState = State.Attack;
        }
        else if (takingDamage)
        {
            enemyCurState = State.Damage;
        }
        else if (!isInAttackAnimation)
        {
            enemyCurState = State.Idle;
        }

    }

    void UpdateEnemyAnimation()
    {
        switch (enemyCurState)
        {
            case State.Attack:
                UpdateAttack();
                break;
            case State.Idle:
                UpdateIdle();
                break;
            case State.Death:
                UpdateDeath();
                break;
            case State.Damage:
                UpdateDamage();
                break;
        }
    }

    void UpdateAttack()
    {
        
        //play enemy attack animation
        enemyAnimatior.Play("Attack");
        // Player[] players = playerPool.GetComponentsInChildren<Player>();
        // foreach(Player attackedPlayer in players){
        //     Animator playerAnimator = attackedPlayer.GetComponent<Animator>();
        // }
        //playerAnimator play damaged animation
        animatorInfo = enemyAnimatior.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime-1f > 0f && animatorInfo.IsTag("Attack"))
        {
            enemyCurState = State.Idle;
            enemyInfo.isAttacking = false;
            Debug.Log("test");
        }

    }

    void UpdateIdle()
    {
        //play enemy Idle animation
        enemyAnimatior.Play("Idle");
        
    }

    void UpdateDeath()
    {
        //play enemy death animation (set animation loop time to none)
        enemyAnimatior.Play("Death");
        animatorInfo = enemyAnimatior.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime-1f > 0f && animatorInfo.IsTag("Death"))
        {
            gameObject.SetActive(false);
        }
    }

    void UpdateDamage()
    {
        enemyAnimatior.Play("Hurt");
        animatorInfo = enemyAnimatior.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime-1f > 0f && animatorInfo.IsTag("Hurt"))
        {
            enemyCurState = State.Idle;
            takingDamage= false;
            Debug.Log("test");
        }
    }

    public void UpdateEnemyHealth(int curHealth)
    {
        enemyHealth = curHealth;
        healthBar.SetHealth((float)curHealth / (float)enemyInfo.health);
    }

    public void SetCurState(State state)
    {
        enemyCurState = state;
    }
}

public class EnemyInfo
{
    [HideInInspector]
    public int health;
    [HideInInspector]
    public bool isAttacking;

    public static EnemyInfo CreateFromJSON(string fileFolder, string fileName)
    {
        string fullPath = Path.Combine(fileFolder, fileName);
        EnemyInfo loadedData = null;
        if (File.Exists(fullPath))
        {
            string loadData = "";
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    loadData = reader.ReadToEnd();
                }
            }
            loadedData = JsonUtility.FromJson<EnemyInfo>(loadData);
        }
        return loadedData;
    }

    public static EnemyInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<EnemyInfo>(jsonString);
    }
    public EnemyInfo(int hp,bool i)
    {
        health = hp;
        isAttacking = i;
    }
}
