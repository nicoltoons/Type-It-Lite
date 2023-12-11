using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using TMPro;
public class LoadSave : MonoBehaviour
{

    public string filePath, dir, nameOfFile;
   
    public string dataAsJson;
    public GameData gameData;
    public bool Imported = false;
    public string loadedFromImportText;
    public string userjson;
    GameManager gameManager;
    public GameObject backPanel, nameUI;
    public TextMeshProUGUI errorText;

    private void Awake()
    {
       dir = (Path.Combine(Application.persistentDataPath, "JSON"));
      if (!Directory.Exists (dir))
        {
            Directory.CreateDirectory(dir);
        }
     if (nameOfFile == string.Empty)
        {
            filePath = dir + "/" + "yourFile.json";
        }
     
    }

   
    public string ReadFromFile(string fileName)
    {

       
        string json = string.Empty;
        FileStream filestream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        bool readFromBinary = true;
        if (readFromBinary)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filestream))
                {
                    json = (string)(reader.ReadToEnd());
                }
            }

        }
        else
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    json = reader.ReadToEnd();
                }
            }
            else
            {
                File.Create(filePath);
            }
        }

        return json;
    }

    private bool VerifyHash(string json)
    {
        string defaultValue = "NO HASH GENERATED";
        string hashValue = SecureHelper.Hash(json);
        string hashStored = PlayerPrefs.GetString("HASH", defaultValue);

        return hashValue == hashStored || hashStored == defaultValue;

    }

    public void CheckName(TMP_InputField p)
    {
        if (errorText)
        {
            errorText.text = string.Empty;
        }
        if (p.text != string.Empty)
        {
            nameOfFile = p.text ;
            filePath = dir +"/" + nameOfFile;
            if (!File.Exists(filePath))
            {

                CreateNewJson();
                nameUI.SetActive(false);
            }
            else
            {
                if (errorText)
                {
                    errorText.text = "Name already exists! Please choose another name";
                }
            }
        }
        else
        {
            gameManager.outputText.text = "Please enter a name.";
        }
    }

public void CreateFile()
    {
        filePath = dir + "/" + nameOfFile;
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        Load();
       
    }


    public void CreateNewJson()
    {
        gameManager = GetComponent<GameManager>();
        Imported = false;
        gameManager.questionTextField.text = string.Empty;
        gameManager.answerDataSizeField.text = string.Empty;
      
            gameData = new GameData();

            SaveFile(gameData);

            Load();
            gameManager.Load();
            gameManager.BringUpFirstUI();
            if (backPanel)
            {
                backPanel.SetActive(false);
            }
    }
    public void Load()
    {
        filePath = dir + "/" + nameOfFile +".json";
        if (!File.Exists(filePath))
        {
             File.Create(filePath);
          
        }

        else
        {
           
            if (!Imported)
            {

                dataAsJson = ReadFromFile(filePath);
                if (VerifyHash(dataAsJson))
                {
                   gameData = JsonUtility.FromJson<GameData>(dataAsJson);
                    
                   

                }
            }
            else
            {
                if(loadedFromImportText != string.Empty)
                {
                   gameData = JsonUtility.FromJson<GameData>(loadedFromImportText);
                    gameManager.Load();
                }
            }
        } 
    }

  
    public void SaveFile(GameData data)
    {
        filePath = dir + "/" + nameOfFile + ".json";
        string json = JsonUtility.ToJson(data);
        SaveHash(json);
        userjson = json;
        //WriteToFile(dataFile, data);
        WriteToFile(filePath, json);
      
    }

    public void SaveHash(string json)
    {
        string hashValue = SecureHelper.Hash(json);
        PlayerPrefs.SetString("Hash", hashValue);
    }
    public void WriteToFile(string fileName, string json)
    {

        string path = Path.Combine(Application.persistentDataPath, fileName);
        FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

        using (StreamWriter writer = new StreamWriter(filestream))
        {
            writer.Write(json);
        };
        filestream.Close();
    }

  

  

}
