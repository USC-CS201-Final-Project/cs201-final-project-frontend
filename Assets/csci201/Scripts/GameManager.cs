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
    int wordCount = -1;

    // current word
    string currword = "";

    // ui elements
    public GameObject g_inp;
    private TMP_Text t_inp;

    public GameObject g_score;
    private TMP_Text t_score;

    public GameObject g_target;
    private TMP_Text t_target;
    // Start is called before the first frame update
    void Start()
    {
        if(ins == null)
            ins = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // get text component of the input display

        t_inp = g_inp.GetComponent<TMP_Text>();
        t_score = g_score.GetComponent<TMP_Text>();
        t_target = g_target.GetComponent<TMP_Text>();
        currword = completedWord();
    }

    // Update is called once per frame
    void Update()
    {
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
                if(checkWord(t_inp.text)) // if typed correctly
                {
                    t_inp.text = ""; // empty the text box
                    // perform actions on success word typing.
                    currword = completedWord();
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

    public string completedWord()
    {
        wordCount++;
        t_score.text = ""+wordCount;        
        // temp word generation handled within gamemanager, replace with call to servermanager
        System.Random random = new Random();
        List<string> words = new List<string>{"computer","science","coding"};
        int index = random.Next(words.Count);
        t_target.text = words[index];
        return words[index];
    }
}
