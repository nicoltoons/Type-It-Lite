using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class DataManager : MonoBehaviour
{
    public PlayerData data;
    private string file = "TyingApp.json";



    void Awake()
    { 
        Screen.orientation = ScreenOrientation.Portrait;
        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        SaveHash(json);
        //WriteToFile(dataFile, data);
        WriteToFile(file, json);
    }
    public void Load()
    {
        // Seye's Serialisation code
        //data = ReadFromFileSeye(dataFile);

        data = new PlayerData();
        string json = ReadFromFile(file);// ReadFromFile(file);
        if (!VerifyHash(json))
        {

        }
        else
        {
            JsonUtility.FromJsonOverwrite(json, data);
        }
    }

    private void SaveHash(string json)
    {
        string hashValue = SecureHelper.Hash(json);
        PlayerPrefs.SetString("Hash", hashValue);
    }

    private bool VerifyHash(string json)
     {
        string defaultValue = "NO HASH GENERATED";
        string hashValue = SecureHelper.Hash(json);
        string hashStored = PlayerPrefs.GetString("HASH", defaultValue);

        return hashValue == hashStored || hashStored == defaultValue;

    }

    private void WriteToFile(string fileName, string json)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        FileStream filestream = new FileStream(path, FileMode.Create);
        bool saveToBinary = true;
        if (saveToBinary)
        {
            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(MemoryStreamSerialize(json));
            };
        }
        else
        {
            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(json);
            };
        }
        filestream.Close();
    }

    private void WriteToFile(string fileName, PlayerData data)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        FileStream filestream = new FileStream(path, FileMode.Create);
        bool saveToBinary = true;
        if (saveToBinary)
        {
            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(MemoryStreamSerialize(data));
            };
        }
        else
        {
            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(JsonUtility.ToJson(data));
            };
        }
        filestream.Close();
    }

    private string GetFilePath(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        return path;
    }

    private string ReadFromFile(string fileName)
    {
        string json = string.Empty;
        string path = Path.Combine(Application.persistentDataPath, fileName);
        FileStream filestream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        bool readFromBinary = true;
        if (readFromBinary)
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(filestream))
                {
                    json = (string)MemoryStreamDeserialize(reader.ReadToEnd());
                }
            }
           
        }
        else
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    json = reader.ReadToEnd();
                }
            }
            else
            {
                File.Create(path);
            }
        }

        return json;
    }

    private PlayerData ReadFromFileSeye(string fileName)
    {
        PlayerData playerData = new PlayerData();
        string path = Path.Combine(Application.persistentDataPath, fileName);
        bool readFromBinary = true;
        if (readFromBinary)
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string data = reader.ReadToEnd();
                    playerData = (PlayerData)MemoryStreamDeserialize(data);
                }
            }
        }
        else
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string json = reader.ReadToEnd();
                    playerData = JsonUtility.FromJson<PlayerData>(json);
                }
            }
            else
            {
                File.Create(path);
            }
        }

        return playerData;
    }

    private string MemoryStreamSerialize(object obj)
    {
        MemoryStream memorystream = new MemoryStream();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(memorystream, obj);
        byte[] mStream = memorystream.ToArray();
        return System.Convert.ToBase64String(mStream);
    }

    public object MemoryStreamDeserialize(string str)
    {
        if (str != "")
        {
            byte[] mData = System.Convert.FromBase64String(str);
            MemoryStream memorystream = new MemoryStream(mData);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(memorystream);
        }
        else
        {
            return "";
        }
    }
}