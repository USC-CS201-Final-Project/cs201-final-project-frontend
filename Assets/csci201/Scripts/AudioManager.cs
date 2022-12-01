using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager ins;
    public AudioSource source;
    public AudioClip bossHit;
    public AudioClip playerHit;
    public AudioClip playerTyped;
    // Start is called before the first frame update
    void Awake()
    {
        if(ins == null){
            ins = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayPlayerHit()
    {
        ins.source.PlayOneShot(ins.playerHit);
    }

    public static void PlayBossHit()
    {
        ins.source.PlayOneShot(ins.bossHit);
    }

    public static void PlayPlayerTyped()
    {
        ins.source.PlayOneShot(ins.playerTyped);
    }


}
