using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{
    public Animator panelAnim;
    
    public void GameOver()
    {
        panelAnim.SetBool("GameOver", true);
    }
}
