using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using SFB;
using TMPro;

[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler {

    public TextMeshProUGUI output;
    LoadSave loadSave;
    GameManager gameManager;



#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

     public void Awake()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        loadSave = GameObject.Find("Manager").GetComponent<LoadSave>();
        if (output)
        {
            output.text = "";
        }
    }
    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".txt", true);
    }

    // Called from browser
    public void OnFileUpload(string urls) {
        StartCoroutine(OutputRoutine(urls.Split(',')));
    }
#else
    //
    // Standalone platforms & editor
    //
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
        // var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", true);
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", true);
        if (paths.Length > 0) {
            var urlArr = new List<string>(paths.Length);
            for (int i = 0; i < paths.Length; i++) {
                urlArr.Add(new System.Uri(paths[i]).AbsoluteUri);
            }
            StartCoroutine(OutputRoutine(urlArr.ToArray()));
        }
    }
#endif

    private IEnumerator OutputRoutine(string[] urlArr) {
        var outputText = "";
        for (int i = 0; i < urlArr.Length; i++) {
           UnityWebRequest loader = UnityWebRequest.Get(urlArr[i]);
            yield return loader.SendWebRequest();

            if (loader.isDone)
            {
                Debug.Log("it is done");
                Debug.Log("loadSave: " + loader.downloadHandler.text);

                loadSave.loadedFromImportText = loader.downloadHandler.text;


                loadSave.nameOfFile = "yourFile";
                if (loadSave.gameData == null )
                {
                    loadSave.gameData = new GameData();
                    loadSave.gameData = JsonUtility.FromJson<GameData>(loader.downloadHandler.text);
                }

                if (loadSave.gameData != null)
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
                        outputText = "";

                        if (gameManager.jsonNameUI)
                        {
                            gameManager.jsonNameUI.SetActive(false);
                        }

                        if (gameManager.backPanel)
                        {
                            gameManager.backPanel.SetActive(false);
                        }
                       
                    }
                    else
                    {
                        if (output) { output.text = "Invalid Format! Please try again."; }
                    }
                }
                else
                {
                    if (output) { output.text = "Invalid Format! Please try again."; }
                }


            }
            output.text = outputText;
    }
}