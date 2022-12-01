using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTesting : MonoBehaviour
{
    public Enemy boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")) boss.enemyInfo.isAttacking=true;
        if(Input.GetKeyDown("a")) boss.takingDamage=true;
        if(Input.GetKeyDown("b")) boss.UpdateEnemyHealth(0);
    }
}
