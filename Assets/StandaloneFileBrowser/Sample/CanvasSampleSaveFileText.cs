using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

[RequireComponent(typeof(Button))]
public class CanvasSampleSaveFileText : MonoBehaviour
{
    public Text output;
    LoadSave loadSave;
    GameManager gameManager;

    // Sample text data
    private string _data = "";

    public void Awake()
    {
        loadSave = GameObject.Find("Manager").GetComponent<LoadSave>();
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        if (loadSave.userjson != string.Empty)
        {
            _data = loadSave.userjson;
        }
        else
        {
            _data = loadSave.loadedFromImportText;
        }
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    // Broser plugin should be called in OnPointerDown.
    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.SaveData();
        string thisFileName;
        if (loadSave.nameOfFile != string.Empty)
        {
            thisFileName = loadSave.nameOfFile;
        }
        else
        {
            thisFileName = "yourfile";
        }
        if (loadSave.userjson != string.Empty)
        {
            _data = loadSave.userjson;
        }
        else
        {
            _data = loadSave.loadedFromImportText;
        }
        var bytes = Encoding.UTF8.GetBytes(_data);
        DownloadFile(gameObject.name, "OnFileDownload", thisFileName + ".json", bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload()
    {
        output.text = "File Successfully Downloaded";
    }
   
#else



    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {

        string thisFileName;
        if (loadSave.nameOfFile != string.Empty)
        {
            thisFileName = loadSave.nameOfFile;
        }
        else
        {
            thisFileName = "yourfile";
        }
        var path = StandaloneFileBrowser.SaveFilePanel("Title", "", thisFileName, "json");
        if (loadSave.userjson != string.Empty)
        {
            _data = loadSave.userjson;
        }
        else
        {
            _data = loadSave.loadedFromImportText;
        }
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, _data);
        }
    }
#endif
}