using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelTransition : MonoBehaviour
{
    // Start is called before the first frame update
    List<string> msg = new List<string>();
    public TextMeshProUGUI countText, msgText;
    public Button nextBtn, skipBtn, previousBtn;
    int n;
    GameManager gameManager;
    

    private void Awake()
    {
        if(previousBtn)
        {
            previousBtn.interactable = false;
        }
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        msg.Add("Please go through these instructions before you use the app.");
        msg.Add("Type It Lite is a question and answer typing app that compiles everything you type in a compact and versatile json format.\n It is perfect for Educational and Trivia apps!");
        msg.Add("You can import json files created in Type It and update them, or start from scratch!");
        msg.Add("You can use the <b>rich text format</b> in Type it. ");
        msg.Add("Hover around the <b>question marks</b> for help and tips on how to use the fields provided.");
        msg.Add("Type it saves as you type so be mindful when changing the information on your file.");
        msg.Add("Reducing a question or answer number deletes the last entry so please be careful when doing this.");
        msg.Add("Please click on the options button on the left to download your work or you will lose it!");
     
        msg.Add("If you encounter any difficulty or you need a similar app with a different architecture, please send a message to <b>support@flamationstudios.com</b> and we will be happy to help.");
        if (countText)
        {
            countText.text = (n + 1).ToString() + "/" + (msg.Count).ToString();
        }

        if (msgText)
        {
            msgText.text = msg[0];
        }

    }
   

    public void ShowTexts()
    {
      if (n < (msg.Count-1))
        {
            if (previousBtn)
            {
                previousBtn.interactable = true;
            }
            n++;
            msgText.text = msg[n];
            countText.text = (n+1).ToString() + "/" + (msg.Count).ToString();
            if (n == msg.Count -1)
            {
                nextBtn.interactable = false;
              
            }
            else
            {

            }
        }
    }

    public void GoBack()
    {
        if (n > 0)
        {
            n--;

            nextBtn.interactable = true;
            msgText.text = msg[n];
            countText.text = (n + 1).ToString() + "/" + (msg.Count).ToString();
            if (n == 0)
            {
                previousBtn.interactable = false;
            }
            else
            {
                previousBtn.interactable = true;
            }
        }
    }

  

  
}
