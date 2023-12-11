using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour
{
    public GameObject backPanel, dialogBox,importPanel, questionSizeUI, questionDataUI, listParent, uiParent, topButtons, titlePanel, mainPanel, tipsPanel, alertPanel, jsonNameUI;
    public Button questionSizeBtn, answerSizeBtn;
   public TMP_InputField questionSizeField, questionTextField, answerDataSizeField,nameField;
  
    List<GameObject> genericButtonList = new List<GameObject>();
    List<GameObject> answerList = new List<GameObject>();
   public int questionSize, answerSize, questionNumber;
    public SimpleObjectPool gamePool, answerPool;
    LoadSave loadSave;
   public GameData gameData;
   QuestionData[] questionData;
    AnswerData[] answerData;
    public int secondsToWait = 3;
    public TextMeshProUGUI outputText;

    private void Awake()
    {
        loadSave = GetComponent<LoadSave>();
        titlePanel.SetActive(true); 
        mainPanel.SetActive(false);
        tipsPanel.SetActive(false);
        CheckInput(questionSizeField.text, questionSizeBtn);
        questionSizeField.onEndEdit.AddListener(delegate { CheckInput(questionSizeField.text, questionSizeBtn); });
        answerDataSizeField.onEndEdit.AddListener(delegate { CheckInput(answerDataSizeField.text, answerSizeBtn); });
        questionDataUI.SetActive(false);
        jsonNameUI.SetActive(false);
        backPanel.SetActive(false);
      
      

    }

    public IEnumerator MoveToTips(GameObject old, GameObject active)
    {
        yield return new WaitForSeconds(secondsToWait);
        active.SetActive(true);
        old.GetComponent<Animation>().Play("PanelBackward");

    }

   public void PlayGame()
    {
        secondsToWait = 1;
        StartCoroutine(MoveToTips(titlePanel, tipsPanel));
    }
    public void CheckInput(string t, Button b)
    {
        int s = GetInt(t);
        if (s > 0)
        {
            b.interactable = true;
        }
        else
        {
            b.interactable = false;
        }
       if(b == answerSizeBtn && s <=0)
        {
            RemoveListButtons(answerPool, answerList);
        }
    }
    public void Load()
    {
       
        if (loadSave.gameData != null)
        {
            gameData = new GameData();
            gameData = loadSave.gameData;
            questionData = new QuestionData[gameData.questionData.Length];
            for (int i = 0; i < gameData.questionData.Length; i ++)
            {
                questionData[i] = new QuestionData();
                questionData[i].questionText = gameData.questionData[i].questionText;
                questionData[i].answers = new AnswerData[gameData.questionData[i].answers.Length];
                for(int j =0; j < gameData.questionData[i].answers.Length; j ++)
                {
                    questionData[i].answers[j] = new AnswerData();
                    questionData[i].answers[j].answerText = gameData.questionData[i].answers[j].answerText;
                    questionData[i].answers[j].isCorrect = gameData.questionData[i].answers[j].isCorrect;

                }
            }
           
        }
       
   
    }
    public void BringUpFirstUI()
    {
        Load();
        //loading from streaming assets;
        if (questionSizeUI)
        {
          questionSizeUI.SetActive(true);  //loading;
        }
        
        if (questionSizeField)
        {
            if (questionData != null)
            {
                questionSizeField.text = questionData.Length.ToString();
                CheckInput(questionSizeField.text, questionSizeBtn);
            }
        }
        RemoveListButtons(answerPool, answerList);
        RemoveListButtons(gamePool, genericButtonList);
        if(questionDataUI)
        {
            questionDataUI.SetActive(false);
        }
       
    }

    public void GoToQuestionUI(TMP_InputField s)
    {
        //get size of questionData array;
        questionSize = GetInt(s.text);
        if (questionSize > 0)
        {
            CreateDataUI(1);
        }
    }

    public void RemoveAllUI()
    {
       if(questionDataUI)
        {
            questionDataUI.SetActive(false);
        }
       if(questionSizeUI)
        {
            questionSizeUI.SetActive(false);
        }
       if(backPanel)
        {
            backPanel.SetActive(false);
        }
       if(nameField)
        {
            nameField.text = string.Empty;
        }
        RemoveListButtons(answerPool, answerList);
        RemoveListButtons(gamePool, genericButtonList);
    }


    public void GoToStart()
    {
       
        RemoveListButtons(answerPool, answerList);
       
        if(questionDataUI)
        {
            questionDataUI.SetActive(false);
        }
        if (questionSizeUI)
        {
            questionSizeUI.SetActive(true);
            questionSizeUI.GetComponentInChildren<TMP_InputField>().text = questionSize.ToString();
        }
        RemoveListButtons(gamePool, genericButtonList);
     
    }
    public void CreateDataUI(int Level)
    {
       
        RemoveListButtons(gamePool, genericButtonList);
        //create buttons on the left;
       
            //create back btn
            GameObject p = GetObject();
            genericButtonList.Add(p);
            p.transform.SetParent(topButtons.transform, false);
            p.name = "-1";
            p.GetComponentInChildren<TextMeshProUGUI>().text = "<< Set Question Number";
            for (int i = 0; i < questionSize; i++)
            {
                GameObject s = GetObject();
                genericButtonList.Add(s);
                s.name = i.ToString();
                s.GetComponentInChildren<TextMeshProUGUI>().text = "Question " + (i + 1).ToString();
            }
            //switch off questionSize and turn on questionDataUI;
            if (questionDataUI)
            {
                questionDataUI.SetActive(true);
            }
            SwitchData("0");
            if (questionSizeUI)
            {
                questionSizeUI.SetActive(false);
            }
        
    }


    public void CreateAnswerUI(TMP_InputField s)
    {
        
        RemoveListButtons(answerPool, answerList);
        answerSize = GetInt(s.text);


        if (questionNumber < questionData.Length && questionNumber >= 0)
        {
            
                   
            for (int i = 0; i < answerSize; i++)
            {
                //create answer ui;
                GameObject n = answerPool.GetObject();
                n.transform.SetParent(uiParent.transform, false);
                answerList.Add(n);
                TextMeshProUGUI answerTitle = n.GetComponentInChildren<TextMeshProUGUI>();
                TMP_InputField answerField = n.GetComponentInChildren<TMP_InputField>();
              answerField.onEndEdit.AddListener(delegate { SaveData(); });
                Toggle answerToggle = n.GetComponentInChildren<Toggle>();
              answerToggle.onValueChanged.AddListener(delegate { SaveData(); });

                answerTitle.text = "Answer " + (i + 1).ToString();
               
                if(questionData[questionNumber].answers != null)
                {
                    if (i < questionData[questionNumber].answers.Length)
                    {

                        if (questionData[questionNumber].answers[i] != null)
                        {
                            answerField.text = questionData[questionNumber].answers[i].answerText;
                            answerToggle.isOn = questionData[questionNumber].answers[i].isCorrect;
                        }
                    }
                    else
                    {
                        answerField.text = "";
                        answerToggle.isOn = false;

                    }
                }   
                

 
            }
        }

        //create answer ui from answerpool;
   
    }
    public void SwitchData(string s)
    {
       
        if (questionSizeUI)
        {
            questionSizeUI.SetActive(false);
        }
        questionNumber = int.Parse(s);
       
            if (questionNumber >= 0)
            {
                if (questionNumber < questionData.Length)
                {
                    questionTextField.text = questionData[questionNumber].questionText;
                if (questionData[questionNumber].answers != null)
                {
                    answerDataSizeField.text = questionData[questionNumber].answers.Length.ToString();
                }
                else
                {
                    answerDataSizeField.text = "0";
                }
                }
                else
                {
                    questionTextField.text = "";
                    answerDataSizeField.text = "";

                }

                RemoveListButtons(answerPool, answerList);
            if (GetInt(answerDataSizeField.text)> 0)
            {
                CreateAnswerUI(answerDataSizeField);
            }
                GetText("Question " + (questionNumber + 1).ToString());
            }
            else
            {
            //-1;
            Debug.Log("Saving data");
                SaveData();
                GoToStart();
            }
        
 
    }
    public GameObject GetObject()
    {
        GameObject newObject = gamePool.GetObject();
        newObject.transform.SetParent(listParent.transform);
        return newObject;
    }
  public void RemoveListButtons(SimpleObjectPool p, List<GameObject> l)
    {
        while(l.Count > 0)
        {
            p.ReturnObject(l[l.Count - 1]);
            l.Remove(l[l.Count - 1]);
        }
    }

    public void GetText(string t)
    {
        GameObject[] genericObjects = GameObject.FindGameObjectsWithTag("genericButton");
        if (genericObjects.Length > 0)
        {
            foreach(GameObject g in genericObjects)
            {
                TextMeshProUGUI txt = g.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
                if (txt)
                {
                    if (txt.text == t)
                    {
                        g.GetComponent<GenericButton>().HighlightText(txt);
                    }
                    else
                    {
                        g.GetComponent<GenericButton>().NormalSize(txt);
                    }
                }
            }
        }
    }

    public void SaveData()
    {
        //save questionNumber
        Debug.Log("Saving Data");
        gameData = new GameData();
        gameData.questionData = new QuestionData[GetInt(questionSizeField.text)];
        questionSize = GetInt(questionSizeField.text);
        for (int c = 0; c < questionSize; c ++)
        {
            gameData.questionData[c] = new QuestionData();
        }
      
        if(questionData !=null)
        { 
        if (questionData.Length <= gameData.questionData.Length) //user has increased length;
        {
          
            questionData.CopyTo(gameData.questionData, 0);
        }
        else
        {
            //user has reduced length;
           for(int a = 0; a < gameData.questionData.Length; a ++)
            {
                gameData.questionData[a] = new QuestionData();
                gameData.questionData[a] = questionData[a];
            }
        }
        if (questionNumber >=0 && questionNumber < gameData.questionData.Length)
        {
           
            gameData.questionData[questionNumber] = new QuestionData();
            gameData.questionData[questionNumber].questionText = questionTextField.text;
            gameData.questionData[questionNumber].answers = new AnswerData[GetInt(answerDataSizeField.text)];

            //check if you had previous answers
            if (questionNumber < questionData.Length)
            {
                if (questionData[questionNumber].answers != null)
                {
                    if (questionData[questionNumber].answers.Length <= gameData.questionData[questionNumber].answers.Length) //user either increased or left length the way it was
                    {
                        questionData[questionNumber].answers.CopyTo(gameData.questionData[questionNumber].answers, 0);
                    }
                    else
                    {
                        for (int b = 0; b < gameData.questionData[questionNumber].answers.Length; b++)
                        {
                            gameData.questionData[questionNumber].answers[b] = new AnswerData();
                            gameData.questionData[questionNumber].answers[b] = questionData[questionNumber].answers[b];
                        }
                    }
                }
            }
                for (int c = 0; c < answerList.Count; c++)
                {
                    TMP_InputField answerField = answerList[c].GetComponentInChildren<TMP_InputField>();
                    Toggle answerToggle = answerList[c].GetComponentInChildren<Toggle>();

                    if (c < gameData.questionData[questionNumber].answers.Length)
                    {
                        if (gameData.questionData[questionNumber].answers[c] != null)
                        {
                            if (answerField)
                            {

                                gameData.questionData[questionNumber].answers[c].answerText = answerField.text;
                            }
                            if (answerToggle)
                            {
                                gameData.questionData[questionNumber].answers[c].isCorrect = answerToggle.isOn;
                            }
                        }
                    }
                }

            }

        }

      
        questionData = gameData.questionData;
        
   
        if (File.Exists(loadSave.filePath))
        {
            loadSave.SaveFile(gameData);
        }
    }

 

    public int GetInt(string t)
    {
        int s;
        if (t != string.Empty)
        {
            s= int.Parse(t);
        }
        else { s = 0; }
        return s;
    }
    

    public void EndRound()
    {
       loadSave. filePath = loadSave.dir + "/" + loadSave.nameOfFile + ".json";
        loadSave.SaveFile(gameData);
        if (!File.Exists(loadSave.filePath))
        {
            File.Create(loadSave.filePath);

        }
       
        if (backPanel)
        {
            backPanel.SetActive(true);
        }
    }

    public void QuitApp()
    {
        if (File.Exists(loadSave.filePath))
        {
            File.Delete(loadSave.filePath);
        }

      
        //SaveData();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.OpenURL("https://flamationstudios.com");
        }
        else
        {
            Application.Quit();
        }
    }

    private void OnApplicationQuit()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (Directory.Exists(loadSave.dir))
            {
                Directory.Delete(loadSave.dir);
            }
        }
        }
}