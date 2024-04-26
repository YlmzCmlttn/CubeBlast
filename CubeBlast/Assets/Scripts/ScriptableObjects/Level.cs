using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
public class Level : ScriptableObject
{
    public int levelNumber;
    public int gridWidth;
    public int gridHeight;
    public int moveCount;
    public string[] grid;
}