// © 2023 Dominik Kowalkowski
// All rights reserved.
// This source code may not be copied, modified, distributed, or used in any way without prior written permission from the author.
// Permission is granted to HR personnel to review this code for recruitment purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameManager gameManager;
    public PieceEnumType.Piece pieceTypeOn;
    [SerializeField] private Color firstColor, secondColor, firstHighlight, secondHighlight;
    public int number;
    public int color;
    private bool offset;
    [SerializeField]public bool moveable;
    public float x;
    public float y;

    public void Update()
    {
        if(moveable)
        {
            var renderer = GetComponent<SpriteRenderer>();
            renderer.color = Color.yellow;
        }
        else
        {
            var renderer = GetComponent<SpriteRenderer>();
            renderer.color = offset ? firstColor : secondColor;
        }

        if(pieceTypeOn == PieceEnumType.Piece.empty)
            GetComponent<BoxCollider>().enabled = true;
        else if(pieceTypeOn != PieceEnumType.Piece.empty)
            GetComponent<BoxCollider>().enabled = false;

        x = transform.position.x;
        y = transform.position.y;
    }

    public void Init(bool isOffset, int num, GameManager gm)
    {
        gameManager = gm;
        number = num;
        moveable = false;
        color = 2;
        offset = isOffset;
        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = offset ? firstColor : secondColor;
        
    }
    
    public void AttachPiece(PieceEnumType.Piece type)
    {
        pieceTypeOn = type;
        ColorOfPiece();
    }

    public void RemovePiece()
    {
        pieceTypeOn = PieceEnumType.Piece.empty;
        ColorOfPiece();
    }

    public void OnMouseDown()
    {
        gameManager.TilePicked(this);
    }

    public void ColorOfPiece()
    {
        if(pieceTypeOn == PieceEnumType.Piece.empty)
            color = 2;
        else if(pieceTypeOn == PieceEnumType.Piece.whiteKing || pieceTypeOn == PieceEnumType.Piece.whiteQueen || 
                pieceTypeOn == PieceEnumType.Piece.whiteBishop || pieceTypeOn == PieceEnumType.Piece.whiteKnight || 
                pieceTypeOn == PieceEnumType.Piece.whiteRook || pieceTypeOn == PieceEnumType.Piece.whitePawn)
            color = 0;
        else if(pieceTypeOn == PieceEnumType.Piece.blackKing || pieceTypeOn == PieceEnumType.Piece.blackQueen || 
                pieceTypeOn == PieceEnumType.Piece.blackBishop || pieceTypeOn == PieceEnumType.Piece.blackKnight || 
                pieceTypeOn == PieceEnumType.Piece.blackRook || pieceTypeOn == PieceEnumType.Piece.blackPawn)
            color = 1;
    }
}
