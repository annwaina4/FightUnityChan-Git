using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    GameObject winText;
    GameObject loseText;
    void Start()
    {
        winText = GameObject.Find("winText");

        loseText = GameObject.Find("loseText");
        
        GameObject.Find("start").GetComponent<Text>().text = "FIGHT";
    }
    void Update()
    {

    }
    
    //�����\��
    public void gameWin()
    {
        winText.GetComponent<Text>().text = "YOU  WIN";
    }

    //�s�k�\��
    public void gameLose()
    {
        loseText.GetComponent<Text>().text = "YOU  LOSE";
    }
}
