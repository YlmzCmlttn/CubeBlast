using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePiece : Piece
{
    public override PieceType Type => PieceType.CUBE;
    [SerializeField] private GameObject m_DestroyParticle;

    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite rocketHintSprite;
    [SerializeField] public Sprite tntHintSprite;


    protected override void Clicked()
    {
        List<GameObject> matches = FindMatches.FindAdjancents(m_Board, position.Vector, this.gameObject.tag);
        if (matches != null)
        {
            if (matches.Count >= 2)
            {
                m_EndGameManager.DecreaseCounterValue();
                DestroyMatches(matches);
                if (matches.Count >= 5)
                {
                    m_Board.CreateTntAt(position);
                }
                else if (matches.Count >= 3)
                {
                    m_Board.CreateRocketAt(position);
                }
                m_Board.DecreaseRow();
            }
            else
            {
                base.Clicked();
            }
        }
        else
        {
            base.Clicked();
        }
    }

    public void MakeDefaultSprite(){ 
        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
    public void MakeTntHint(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tntHintSprite;
    }
    public void MakeRocketHint(){
        this.gameObject.GetComponent<SpriteRenderer>().sprite = rocketHintSprite;
    }

    void HitAdjancents(Position position, List<GameObject> hitted)
    {
        HitPieceAt(position.Vector + Vector2.left,hitted);
        HitPieceAt(position.Vector + Vector2.right, hitted);
        HitPieceAt(position.Vector + Vector2.down, hitted);
        HitPieceAt(position.Vector + Vector2.up, hitted);
    }
    public override void DamagedBy(PieceType pieceType)
    {
        if ((pieceType == PieceType.SPECIAL))
        {
            DestroyPiece();
        }
    }


    void HitPieceAt(Vector2 position, List<GameObject> hitted)
    {
        if (m_Board.isInsideTheBoardAndNotNull((int)position.x, (int)position.y))
        {
            if(!hitted.Contains(m_Board.allPieces[(int)position.x, (int)position.y])) {
                m_Board.allPieces[(int)position.x, (int)position.y].GetComponent<Piece>().DamagedBy(PieceType.CUBE);
                hitted.Add(m_Board.allPieces[(int)position.x, (int)position.y]);
            }
        }
    }
    private IEnumerator DestroyPieceCo()
    {
        this.gameObject.transform.localScale = Vector3.zero;
        Vector3 currentPosition = this.transform.position;
        currentPosition.z = -10;
        GameObject destroyParticle = Instantiate(m_DestroyParticle, currentPosition, Quaternion.identity);
        yield return new WaitForSeconds(destroyParticle.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
        Destroy(destroyParticle);
        m_Board.PushObject(this.gameObject);

    }
    public override void DestroyPiece()
    {
        if (m_Board.allPieces[position.X, position.Y] != null)
        {
            m_Board.allPieces[position.X, position.Y] = null;
        }
        StartCoroutine(DestroyPieceCo());
    }
    void DestroyMatches(List<GameObject> matches)
    {
        List<GameObject> hitted =new List<GameObject>();
        foreach (GameObject gameObject in matches)
        {
            Piece cube = gameObject.GetComponent<Piece>();
            HitAdjancents(cube.position, hitted);
            cube.DestroyPiece();
        }
    }

}
