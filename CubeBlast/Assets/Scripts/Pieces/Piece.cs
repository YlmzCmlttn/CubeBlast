using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Piece : MonoBehaviour
{
    public abstract PieceType Type { get; }

    protected Board m_Board;
    [SerializeField] public Position position;
    [SerializeField] public bool moveable;

    private EffectBuilder takeDamageEffect;



    protected EndGameManager m_EndGameManager;

    private void OnMouseDown()
    {
        if(m_Board.gameState == GameState.MOVE)
        {
            Clicked();
        }
    }
    protected virtual void Clicked()
    {
        takeDamageEffect.ExecuteEffects();
    }

    protected virtual void Awake()
    {
        float maxShakeRotation = 10;
        float shakeSpeed = 50;
        float scaleSpeed = 40f;
        Vector3 maxSize = new Vector3(0.95f, 0.95f, 0.95f);
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        takeDamageEffect = new EffectBuilder(this);
        takeDamageEffect
            .AddEffect(new ShakeEffect(this.gameObject.GetComponent<Transform>(), maxShakeRotation, shakeSpeed))
            .AddEffect(new ScaleEffect(this.gameObject.GetComponent<Transform>(), maxSize, scaleSpeed, wait));
    }

    protected virtual void Start()
    {
        m_Board = FindObjectOfType<Board>();
        m_EndGameManager = FindObjectOfType<EndGameManager>();
    }
    public virtual void DestroyPiece()
    {
        if (m_Board.allPieces[position.X, position.Y] != null)
        {
            m_Board.allPieces[position.X, position.Y] = null;
        }
        Destroy(this.gameObject);
    }


    public virtual void DamagedBy(PieceType piecetype)
    {
        DestroyPiece();
    }

    void Update()
    {
        if (moveable)
        {
            if (Mathf.Abs(position.Y - transform.position.y) > .01)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.X, position.Y, -1 * position.Y), 30f * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(position.X, position.Y, -1 * position.Y);                
            }
        }        
    }

}
