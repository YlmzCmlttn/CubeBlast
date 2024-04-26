using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UIElements;

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    [SerializeField] public SaveData saveData;
    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.OpenWrite(Application.persistentDataPath + "/player.dat");
        SaveData copySaveData = new SaveData();
        copySaveData = saveData;
        formatter.Serialize(file, copySaveData);
        file.Close();
        Debug.Log("Saved");
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.OpenRead(Application.persistentDataPath + "/player.dat");
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Loaded");
        }
        else
        {
            saveData = new SaveData();
            saveData.currentLevel = 1;
            saveData.finished = false;
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDisable()
    {
        Save();
    }
}
