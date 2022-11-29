using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject bossPrefab;

    public List<Sprite> costumes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InstantiatePlayer(string[] usernames, int startingPlayerHealth, int startingBossHealth, string[] startingWord,int[] costumeID)
    {
        for(int i = 0; i < usernames.Length; i++){
            GameObject playerObject = Instantiate(playerPrefab,new Vector3(-i*2f,0f,0f),Quaternion.identity);
            Player player = playerObject.GetComponent<Player>();
            player.transform.SetParent(gameObject.transform);
            player.playerInfo = new PlayerInfo(usernames[i],i,startingPlayerHealth,false,costumeID[i]);
            player.SetCurWord(startingWord[i]);
            player.UpdatePlayerHealth(startingPlayerHealth);
            player.UpdateCostumeSprite();
            player.t_username.text = usernames[i];
        }


        GameObject boss = Instantiate(bossPrefab, new Vector3(5f,0f,0f),Quaternion.identity);
        boss.transform.SetParent(gameObject.transform);
        boss.GetComponent<Enemy>().enemyInfo = new EnemyInfo(startingBossHealth,false);
        boss.GetComponent<Enemy>().UpdateEnemyHealth(startingBossHealth);
    }

    public void InstantiatePlayer()
    {
        GameObject boss = Instantiate(bossPrefab, new Vector3(5f,0f,0f),Quaternion.identity);
        boss.transform.SetParent(gameObject.transform);
        boss.GetComponent<Enemy>().enemyInfo = new EnemyInfo(100,false);
        boss.GetComponent<Enemy>().UpdateEnemyHealth(100);

        for(int i = 0; i < 4; i++){
            GameObject playerObject = Instantiate(playerPrefab,new Vector3(-i*2f,0f,0f),Quaternion.identity);
            Player player = playerObject.GetComponent<Player>();
            player.transform.SetParent(gameObject.transform);
            player.playerInfo = new PlayerInfo("Debug "+i,i,60,false,i);
            player.SetCurWord("testing");
            player.UpdatePlayerHealth(60);
            player.UpdateCostumeSprite();
            player.t_username.text = "Debug "+i;
        }
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
