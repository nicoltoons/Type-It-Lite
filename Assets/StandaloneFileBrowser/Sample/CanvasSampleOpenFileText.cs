using System.Text;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using UnityEngine.Networking;
using TMPro;
[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileText : MonoBehaviour, IPointerDownHandler {
    GameManager gameManager;
    LoadSave loadSave;
    public GameObject panel, backPanel;
    public TextMeshProUGUI outputText;
    public bool addToJson;

    //#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);
    
    public void Awake()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        loadSave = GameObject.Find("Manager").GetComponent<LoadSave>();
        if (outputText)
        {
            outputText.text = "";
        }
    }
   //Webgl
   
    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".json", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
         loadSave.gameData = new GameData();
        loadSave.gameData = gameManager.gameData;
        gameManager.SaveData();
        loadSave.Imported = true;
        StartCoroutine(OutputRoutine(url));

    }
#endif


#if !UNITY_WEBGL && UNITY_EDITOR

    public void OnPointerDown(PointerEventData eventData) { }

    void Start() {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        loadSave = GameObject.Find("Manager").GetComponent<LoadSave>();
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        gameManager.SaveData();
        loadSave.gameData = new GameData();
        loadSave.gameData = gameManager.gameData;
        var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "json", false);
        if (paths.Length > 0) {
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }
#endif

    private IEnumerator OutputRoutine(string url)
    {
        UnityWebRequest loader = UnityWebRequest.Get(url);
        yield return loader.SendWebRequest();

        if (loader.isDone)
        {
            Debug.Log("it is done");
            Debug.Log("loadSave: " + loader.downloadHandler.text);
       
           loadSave.loadedFromImportText = loader.downloadHandler.text;
           

            loadSave.nameOfFile = "yourFile";
            if (loadSave.gameData == null  || !addToJson)
            {
                loadSave.gameData = new GameData();
                loadSave.gameData = JsonUtility.FromJson<GameData>(loader.downloadHandler.text);
            }
           
                if (loadSave.gameData != null)
                {
                if (addToJson)
                {
                    //create a new game data to store the new JSON information
                    GameData newGameData = new GameData();
                    newGameData = JsonUtility.FromJson<GameData>(loader.downloadHandler.text);

                    //combine the old JSON and the new JSON information;
                    GameData allGameData = new GameData();
                    allGameData.questionData = new QuestionData[newGameData.questionData.Length + loadSave.gameData.questionData.Length];
                    loadSave.gameData.questionData.CopyTo(allGameData.questionData, 0);
                    newGameData.questionData.CopyTo(allGameData.questionData, loadSave.gameData.questionData.Length);

                    //clear your game data and equate it to the combination above
                    loadSave.gameData = new GameData();
                    loadSave.gameData.questionData = new QuestionData[allGameData.questionData.Length];
                    allGameData.questionData.CopyTo(loadSave.gameData.questionData, 0);
                        
                }
                   //load ui and input texts in ui.
                    if (loadSave.gameData.questionData != null)
                    {

                        QuestionData[] qData = new QuestionData[loadSave.gameData.questionData.Length];
                        loadSave.gameData.questionData.CopyTo(qData, 0);
                        gameManager.outputText.text = string.Empty;
                        gameManager.Load();
                        gameManager.BringUpFirstUI();
                        outputText.text = "";

                        if (gameManager.jsonNameUI)
                        {
                            gameManager.jsonNameUI.SetActive(false);
                        }

                        if (backPanel)
                        {
                            backPanel.SetActive(false);
                        }
                        if (panel)
                        {
                            panel.SetActive(false);
                        }
                    }
                    else
                    {
                        if (outputText) { outputText.text = "Invalid Format! Please try again."; }
                    }
                }
                else
                {
                    if (outputText) { outputText.text = "Invalid Format! Please try again."; }
                }

            


        }
    }
       
    }



  
