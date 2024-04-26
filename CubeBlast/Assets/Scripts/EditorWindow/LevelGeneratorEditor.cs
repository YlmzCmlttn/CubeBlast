using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System;
[System.Serializable]
public class LevelData
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public string[] grid;
}
public class LevelGeneratorEditor : EditorWindow
{
    [MenuItem("Tools/Generate Level Scriptable Object")]
    public static void GenerateLevelScriptableObject()
    {
        string levelFilePath = EditorUtility.OpenFilePanel("Select Level JSON File", "", "json");
        if (string.IsNullOrEmpty(levelFilePath))
            return;

        
        string json = File.ReadAllText(levelFilePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        
        Level levelScriptableObject = ScriptableObject.CreateInstance<Level>();
        levelScriptableObject.levelNumber = levelData.level_number;
        levelScriptableObject.gridWidth = levelData.grid_width;
        levelScriptableObject.gridHeight = levelData.grid_height;
        levelScriptableObject.moveCount = levelData.move_count;
        levelScriptableObject.grid = levelData.grid;


        string fileName = "Level" + levelData.level_number + ".asset";
        string outputPath = Path.Combine("Assets", fileName);

        int count = 1;
        while (File.Exists(outputPath))
        {
            fileName = "Level" + levelData.level_number + "_" + count + ".asset";
            outputPath = Path.Combine("Assets", fileName);
            count++;
        }
        AssetDatabase.CreateAsset(levelScriptableObject, outputPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Level Scriptable Object generated: " + outputPath);
    }
}