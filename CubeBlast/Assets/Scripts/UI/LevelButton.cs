using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject InnerButton;
    private GameData gameData;
    public TextMeshProUGUI levelText;

    void Start()
    {
        gameData = FindObjectOfType<GameData>();      
        if (gameData.saveData.finished)
        {
            levelText.text = "Finished";
        }
        else
        {
            levelText.text = "Level " + gameData.saveData.currentLevel.ToString();
        }
    }

    public void OnEnable()
    {
        //levelText.text = "Level "+gameData.saveData.currentLevel.ToString();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        InnerButton.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        InnerButton.transform.localScale = Vector3.one;
        if (!gameData.saveData.finished)
        {
            PlayerPrefs.SetInt("Level", gameData.saveData.currentLevel - 1);
            SceneManager.LoadScene("GameScene");
        }
            
    }
}
