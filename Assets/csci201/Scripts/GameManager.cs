using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Random = System.Random;


public class GameManager : MonoBehaviour
{
    // singleton
    public static GameManager ins;

    // words typed
    int wordCount = 0;

    // current word
    private string currword = "";

    // ui elements
    public TMP_Text t_inp;
    public TMP_Text t_score;
    public TMP_Text t_target;
    

    private void OnEnable() {
        currword = "debug";
        t_score.text = "0";
        t_target.text = "";
        t_inp.text = "";
    }

    void Awake()
    {
        if(ins == null){
            ins = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        // get text component of the input display
        currword = "debug";
        t_score.text = "0";
        t_target.text = "";
        t_inp.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(!ServerManager.ins.inGameplay) return;
        foreach(char c in Input.inputString)
        {
            if(c == '\b') // backspace
            {
                if(t_inp.text.Length != 0)
                {
                    t_inp.text = t_inp.text.Substring(0,t_inp.text.Length-1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                if(t_inp.text != "" && checkWord(t_inp.text)) // if typed correctly
                {
                    t_inp.text = ""; // empty the text box
                    // perform actions on success word typing.
                    completedWord();
                }
                else
                {
                    // shake the text box to show incorrect spelling?
                }

            }
            else if(Char.IsLetter(c)) //typing letters
            {
                t_inp.text += c;
            }
        }
    }

    public bool checkWord(string s) 
    {
        return s.Equals(currword, StringComparison.OrdinalIgnoreCase);
    }

    public void completedWord()
    {
        ServerManager.ins.CompleteWord();
        wordCount++;
        t_score.text = ""+wordCount;        
        // temp word generation handled within gamemanager, replace with call to servermanager
        // System.Random random = new Random();
        // List<string> words = new List<string>{"computer","science","coding","bored","more","words"};
        // int index = random.Next(words.Count);
        // setWord(words[index]);
    }

    public static void setWord(string w) {
        ins.currword = w;
        ins.t_target.text = w;
    }
}
