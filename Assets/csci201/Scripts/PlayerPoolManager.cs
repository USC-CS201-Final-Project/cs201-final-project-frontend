using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject bossPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InstantiatePlayer(string[] usernames, int startingPlayerHealth, int startingBossHealth, string[] startingWord,int[] costumeID)
    {
        for(int i = 0; i < usernames.Length; i++){
            GameObject playerObject = Instantiate(playerPrefab,new Vector3(0f,i,0f),Quaternion.identity);
            Player player = playerObject.GetComponent<Player>();
            player.transform.SetParent(gameObject.transform);
            player.playerInfo = new PlayerInfo(usernames[i],i,startingPlayerHealth,false,costumeID[i]);
            player.SetCurWord(startingWord[i]);
            player.UpdatePlayerHealth(startingPlayerHealth);
            player.UpdateCostumeSprite();
            player.t_username.text = usernames[i];
        }


        GameObject boss = Instantiate(bossPrefab);
        boss.transform.SetParent(gameObject.transform);
        boss.GetComponent<Enemy>().enemyInfo = new EnemyInfo(startingBossHealth,false);
        boss.GetComponent<Enemy>().UpdateEnemyHealth(startingBossHealth);
    }

    public void DestroyPlayers()
    {
        Transform[] players = GetComponentsInChildren<Transform>();
        foreach (var player in players)
        {
            Destroy(player.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
