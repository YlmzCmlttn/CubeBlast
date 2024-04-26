using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePiece : Piece
{
    public override PieceType Type => PieceType.OBSTACLE;
    public Sprite[] damagedSprite;

    public PieceType[] gettingDamage;
    public int hp;
    [SerializeField] private GameObject m_DestroyParticle;

    public override void DamagedBy(PieceType pieceType)
    {
        if (Array.IndexOf(gettingDamage, pieceType) != -1)
        {
            hp--;
            if (hp <= 0)
            {
                DestroyPiece();
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damagedSprite[hp - 1];
            }
        }
    }

    private void DestroyParticle()
    {
        this.gameObject.transform.localScale = Vector3.zero;
        Vector3 currentPosition = this.transform.position;
        currentPosition.z = -10;
        GameObject destroyParticle = Instantiate(m_DestroyParticle, currentPosition, Quaternion.identity);
        Destroy(destroyParticle, 1.0f);
    }
    public override void DestroyPiece()
    {
        if (Mathf.Abs(transform.position.y) < m_Board.Size.Height)
        {
            FindObjectOfType<GoalManager>().CompareGoal(this.gameObject.tag);
            DestroyParticle();
            base.DestroyPiece();
        }
    }

}
