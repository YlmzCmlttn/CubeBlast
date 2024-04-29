using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpecialPiece : Piece
{
    public override PieceType Type => PieceType.SPECIAL;
    public SpecialItemType specialItemType;

    void Trigger()
    {
        if (specialItemType == SpecialItemType.HORIZONTAL_ROCKET)
        {
            TriggerHorizontalRocket();
        }
        else if (specialItemType == SpecialItemType.VERTICAL_ROCKET)
        {
            TriggerVerticalRocket();
        }
        else
        {
            TriggerTnt();
        }
    }
    void TriggerComboTnt()
    {
        DestroyPiece();
        m_Board.TriggerComboTntAt(position);
    }
    void Trigger_Rocket_Tnt_Combo()
    {

        DestroyPiece();
        for(int i = -1; i< 2; i++)
        {
            if (m_Board.isInsideTheBoard(new Position(position.X + i, position.Y)))
            {
                m_Board.TriggerVerticalRocketAt(new Position(position.X + i, position.Y));

            }
            if (m_Board.isInsideTheBoard(new Position(position.X, position.Y + i)))
            {
                m_Board.TriggerHorizontalRocketAt(new Position(position.X, position.Y + i));
            }
        }        

    }
    void Trigger_Rocket_Rocket_Combo()
    {
        DestroyPiece();
        m_Board.TriggerHorizontalRocketAt(position);
        m_Board.TriggerVerticalRocketAt(position);
    }
    void TriggerHorizontalRocket()
    {
        DestroyPiece();
        m_Board.TriggerHorizontalRocketAt(position);
    }


    void TriggerVerticalRocket()
    {
        DestroyPiece();
        m_Board.TriggerVerticalRocketAt(position);
    }

    void TriggerTnt()
    {
        DestroyPiece();
        m_Board.TriggerTntAt(position);
    }

    public override void DamagedBy(PieceType pieceType)
    {
        if ((pieceType == PieceType.SPECIAL)&&(Mathf.Abs(transform.position.y) < m_Board.Size.Height))
        {
            Trigger();
        }
    }

    protected override void Clicked()
    {
        m_EndGameManager.DecreaseCounterValue();
        List<GameObject> matches = FindMatches.FindAdjancents(m_Board, position.Vector, this.gameObject.tag);
        int numberOfTnt = 0;
        int numberOfRocket = 0;
        foreach (GameObject gameObject in matches)
        {
            SpecialPiece specialItem = gameObject.GetComponent<SpecialPiece>();
            if (specialItem != null)
            {
                if (specialItem.specialItemType == SpecialItemType.TNT)
                {
                    numberOfTnt++;
                }
                else
                {
                    numberOfRocket++;
                }
            }
        }
        if (matches.Count > 1)
        {

            foreach (GameObject gameObject in matches)
            {
                if(gameObject != this.gameObject)
                {
                    gameObject.GetComponent<Piece>().DestroyPiece();
                }
            }
            if (numberOfTnt >= 2)
            {
                TriggerComboTnt();
            }
            else
            {
                if (numberOfTnt == 1 && numberOfRocket >= 1)
                {
                    Trigger_Rocket_Tnt_Combo();
                }
                else if (numberOfRocket >= 2)
                {
                    Trigger_Rocket_Rocket_Combo();
                }
            }
        }
        else
        {
            Trigger();
        }
    }

}
