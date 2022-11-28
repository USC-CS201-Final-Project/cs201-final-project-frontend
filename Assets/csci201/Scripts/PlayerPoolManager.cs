using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InstantiatePlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.SetParent(gameObject.transform);
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
