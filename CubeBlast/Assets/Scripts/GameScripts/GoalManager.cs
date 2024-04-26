using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public Goal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();

    public Sprite boxSprite;
    public Sprite vaseSprite;
    public Sprite stoneSprite;

    public GameObject goalPrefab;
    public GameObject goalGameParent;
    // Start is called before the first frame update
    void Start()
    {
        SetupGoals();
        SetupPrefabs();
    }

    void SetupPrefabs()
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {

            GameObject goalGame = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            goalGame.transform.SetParent(goalGameParent.transform);
            goalGame.transform.localScale = Vector3.one;
            GoalPanel goalPanel = goalGame.GetComponent<GoalPanel>();
            currentGoals.Add(goalPanel);
            goalPanel.sprite = levelGoals[i].goalSprite;
            goalPanel.text = levelGoals[i].numberRemained.ToString();
        }
    }

    void SetupGoals()
    {

        Board board = FindObjectOfType<Board>();
        Level currentLevel = board.GetLevel();

        Dictionary<string, int> levelGoalsDict = new Dictionary<string, int>();

        for (int i = 0; i < currentLevel.grid.Length; i++)
        {
            if (currentLevel.grid[i] == "bo" || currentLevel.grid[i] == "v" || currentLevel.grid[i] == "s")
            {
                if (levelGoalsDict.ContainsKey(currentLevel.grid[i]))
                {
                    levelGoalsDict[currentLevel.grid[i]] += 1;
                }
                else
                {
                    levelGoalsDict[currentLevel.grid[i]] = 1;
                }
            }
        }
        levelGoals = new Goal [levelGoalsDict.Count];
        int index =0;
        foreach (var pair in levelGoalsDict)
        {
            if(pair.Key == "bo")
            {
                levelGoals[index].goalSprite = boxSprite;
                levelGoals[index].numberRemained = pair.Value;
                levelGoals[index].goalTag = "Box";
            }else if (pair.Key == "v")
            {
                levelGoals[index].goalSprite = vaseSprite;
                levelGoals[index].numberRemained = pair.Value;
                levelGoals[index].goalTag = "Vase";
            }
            else if(pair.Key == "s")
            {
                levelGoals[index].goalSprite = stoneSprite;
                levelGoals[index].numberRemained = pair.Value;
                levelGoals[index].goalTag = "Stone";
            }
            index++;
        }
    }

    public void UpdateGoals()
    {
        int goalsCompleted = 0;
        for (int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].UpdateGoal(levelGoals[i].numberRemained.ToString());
            if (levelGoals[i].numberRemained <= 0)
            {
                goalsCompleted++;
                currentGoals[i].Completed();
            }
        }
        if (goalsCompleted >= levelGoals.Length)
        {
            FindObjectOfType<EndGameManager>().WinGame();
        }

    }

    public void CompareGoal(string goalToCompare)
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {
            if (goalToCompare == levelGoals[i].goalTag)
            {
                levelGoals[i].numberRemained--;
            }
        }
        UpdateGoals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
