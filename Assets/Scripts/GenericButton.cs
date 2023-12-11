using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class GenericButton : MonoBehaviour
{
    GameManager gameManager;
    public int Level;
    public TextMeshProUGUI aText;


    public void Start()
    {

        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { ChangeData(aText); });
    }


    public void ChangeData(TextMeshProUGUI t)
    {
         gameManager.SwitchData(gameObject.name);
        
    }

    public void NormalSize(TextMeshProUGUI t)
    {
        t.color = new Color(0, 0, 0, 1); //black
        t.fontSize = 45;
        t.text = t.text.Replace(">>", "");
    }

    public void ChangeBack()
    {
        GameObject[] allG = GameObject.FindGameObjectsWithTag("genericButton");
        if (allG.Length > 0)
        {
            for (int i = 0; i < allG.Length; i++)
            {
                GenericButton[] f = allG[i].GetComponentsInChildren<GenericButton>();
                foreach (GenericButton g in f)
                {
                    TextMeshProUGUI b = g.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
                    NormalSize(b);

                }
            }
        }
    }
    public void HighlightText(TextMeshProUGUI text)
    {
        text.color = Color.blue;
        if (text.text.Contains("<<"))
        {
            text.text = text.text.Replace("<<", "");
        }
        if (!text.text.Contains(">>"))
        {
            if (!text.text.Contains("Back To"))
            {
                text.text += ">>";
            }
            else
            {
                text.text = "<<" + text.text;
            }
        }
    }
    public void GetText()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();

       if (buttonText.text == "Question " + (gameManager.questionNumber + 1).ToString())
        {
            HighlightText(buttonText);
        }
        else
        {
            NormalSize(buttonText);
        }
        
    
    }
}