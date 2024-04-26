using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private Board board;
    private int currentCounterValue;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;
    void Start()
    {
        board = FindObjectOfType<Board>();
        Level currentLevel = board.GetLevel();
        currentCounterValue = currentLevel.moveCount;
        counterText.text = currentCounterValue.ToString();
    }

    public void DecreaseCounterValue()
    {
        currentCounterValue--;
        if (currentCounterValue <= 0)
        {
            LoseGame();
            currentCounterValue = 0;
        }
        counterText.text = currentCounterValue.ToString();
    }
    public void WinGame()
    {
        board.gameState = GameState.WIN;
        FindObjectOfType<FadePanelController>().GameOver();
        youWinPanel.SetActive(true);
    }
    IEnumerator WaitForCheckCo()
    {
        while(board.gameState == GameState.WAIT) {
            yield return new WaitForSeconds(0.1f);        
        }
        board.gameState = GameState.LOSE;
        FindObjectOfType<FadePanelController>().GameOver();
        tryAgainPanel.SetActive(true);

    }
    public void LoseGame()
    {
        StartCoroutine(WaitForCheckCo());
    }
}
