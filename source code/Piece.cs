// © 2023 Dominik Kowalkowski
// All rights reserved.
// This source code may not be copied, modified, distributed, or used in any way without prior written permission from the author.
// Permission is granted to HR personnel to review this code for recruitment purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameManager gameManager;
    public PieceEnumType.Piece type;
    public PieceEnumType.Piece previousType;
    public int sizeOfMoves;
    public int[,] legalMoves;
    public int[] values;
    public List<Tile> activeMoves;
    public List<Tile> copiedActiveMoves;
    [SerializeField]public Tile currentTile;
    [SerializeField]public Tile previousPosition;
    public int color;
    public int number;
    public bool firstMoveForPawn;
    public bool mxfirstMoveForPawn;
    public bool colorActive;
    [SerializeField]public int value;
    [SerializeField]public int pieceTakeoverValue;

    public float x;
    public float y;

    public void Init(PieceEnumType.Piece tp, int col, Tile curTile, GameManager gm)
    {
        type = tp;
        previousType = tp;
        color = col;
        gameManager = gm;
        SetupMoves();
        firstMoveForPawn = false;
        mxfirstMoveForPawn = false;
        EvaluateValues();
        currentTile = curTile;
        value = 0;
        pieceTakeoverValue = 0;
        copiedActiveMoves = new List<Tile>();
    }
    public void CopyMoves()
    {
        copiedActiveMoves.AddRange(activeMoves);
    }
    public void ReActiveMoves()
    {
        activeMoves.AddRange(copiedActiveMoves);
        foreach (Tile move in activeMoves) 
             move.moveable = true;
    }
    public int returnValue(int index)
    {
        return values[index];
    }
    public void UpdateTransform(float tmpx, float tmpy)
    {
        transform.position = new Vector3(tmpx, tmpy);
        x = tmpx;
        y = tmpy;
    }
    public void SetInActive()
    {
        type = PieceEnumType.Piece.empty;
        previousType = PieceEnumType.Piece.empty;
    }
    public void OnMouseDown()
    {   
        gameManager.PiecePicked(this);
    }

    public void BackToPreviousState()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        type = previousType;
    }
    public void AddPreviousState(PieceEnumType.Piece newType)
    {
        previousType = newType;
    }
    public void AddCurrentTile(Tile tile)
    {
        currentTile = tile;
    }

    public void AddMove(Tile tile) 
    {
        activeMoves.Add(tile);
    }

    public void ClearMoves() 
    {
        activeMoves = new List<Tile>();
    }

    public void ShowMoves()
    {
        foreach (Tile move in activeMoves) 
        {
             Debug.Log(move.x + " " + move.y);
        }
    }

    public int UpdateValue()
    {
        value = values[(int)(transform.position.x + 8*transform.position.y)] + pieceTakeoverValue;
        return value;
    }
    public void Update()
    {
        x = transform.position.x;
        y = transform.position.y;
        if(type == PieceEnumType.Piece.empty)
            ClearMoves();
    }

    public void SetupMoves()
    {
        switch(type)
        {
            case PieceEnumType.Piece.whitePawn:
            sizeOfMoves = 2;
                activeMoves = new List<Tile>();
                legalMoves = new int[4,2]
                {{0,1}, {0,2}, {-1,1}, {1,1}};
                break;

            case PieceEnumType.Piece.whiteRook:
            sizeOfMoves = 28;
                activeMoves = new List<Tile>();
                legalMoves = new int[28,2] 
                {{0,1}, {0,2} ,{0,3} ,{0,4} ,{0,5} ,{0,6} ,{0,7},
                {0,-1} ,{0,-2} ,{0,-3} ,{0,-4} ,{0,-5} ,{0,-6} ,{0,-7},
                {1,0} ,{2,0} ,{3,0} ,{4,0} ,{5,0} ,{6,0} ,{7,0},
                {-1,0} ,{-2,0} ,{-3,0} ,{-4,0} ,{-5,0} ,{-6,0} ,{-7,0} };
                break;

            case PieceEnumType.Piece.whiteKnight:
            sizeOfMoves = 8;
                activeMoves = new List<Tile>();
                legalMoves = new int[8,2]
                {{-1,2}, {1,2}, {-2,1}, {2,1}, {-1,-2}, {1,-2}, {-2,-1}, {2,-1}}; 
                break;

            case PieceEnumType.Piece.whiteBishop:
            sizeOfMoves = 28;
                activeMoves = new List<Tile>();
                legalMoves = new int[28,2] 
                {{1,1} ,{2,2} ,{3,3}, {4,4}, {5,5}, {6,6}, {7,7},
                 {-1,-1} ,{-2,-2} ,{-3,-3}, {-4,-4}, {-5,-5}, {-6,-6}, {-7,-7},
                 {-1,1} ,{-2,2} ,{-3,3}, {-4,4}, {-5,5}, {-6,6}, {-7,7},
                 {1,-1} ,{2,-2} ,{3,-3}, {4,-4}, {5,-5}, {6,-6}, {7,-7} };
                break;
                
            case PieceEnumType.Piece.whiteQueen:
            sizeOfMoves = 56;
                activeMoves = new List<Tile>();
                legalMoves = new int[56,2] 
                {{0,1}, {0,2} ,{0,3} ,{0,4} ,{0,5} ,{0,6} ,{0,7},
                 {0,-1} ,{0,-2} ,{0,-3} ,{0,-4} ,{0,-5} ,{0,-6} ,{0,-7},
                 {1,0} ,{2,0} ,{3,0} ,{4,0} ,{5,0} ,{6,0} ,{7,0},
                 {-1,0} ,{-2,0} ,{-3,0} ,{-4,0} ,{-5,0} ,{-6,0} ,{-7,0},
                 {1,1} ,{2,2} ,{3,3}, {4,4}, {5,5}, {6,6}, {7,7},
                 {-1,-1} ,{-2,-2} ,{-3,-3}, {-4,-4}, {-5,-5}, {-6,-6}, {-7,-7},
                 {-1,1} ,{-2,2} ,{-3,3}, {-4,4}, {-5,5}, {-6,6}, {-7,7},
                 {1,-1} ,{2,-2} ,{3,-3}, {4,-4}, {5,-5}, {6,-6}, {7,-7} 
                };
                break;

            case PieceEnumType.Piece.whiteKing:
            sizeOfMoves = 8;
                activeMoves = new List<Tile>();
                legalMoves = new int[8,2]
                {{-1,1}, {0,1}, {1,1}, {-1,0}, {1,0}, {-1,-1}, {0,-1}, {1,-1}}; 
                break;

            case PieceEnumType.Piece.blackPawn:
            sizeOfMoves = 2;
                activeMoves = new List<Tile>();
                legalMoves = new int[4,2]
                {{0,-1}, {0,-2}, {-1,-1}, {1,-1}};
                break;

            case PieceEnumType.Piece.blackRook:
            sizeOfMoves = 28;
                activeMoves = new List<Tile>();
                legalMoves = new int[28,2] 
                {{0,1}, {0,2} ,{0,3} ,{0,4} ,{0,5} ,{0,6} ,{0,7},
                {0,-1} ,{0,-2} ,{0,-3} ,{0,-4} ,{0,-5} ,{0,-6} ,{0,-7},
                {1,0} ,{2,0} ,{3,0} ,{4,0} ,{5,0} ,{6,0} ,{7,0},
                {-1,0} ,{-2,0} ,{-3,0} ,{-4,0} ,{-5,0} ,{-6,0} ,{-7,0} };
                break;

            case PieceEnumType.Piece.blackKnight:
            sizeOfMoves = 8;
                activeMoves = new List<Tile>();
                legalMoves = new int[8,2]
                {{-1,2}, {1,2}, {-2,1}, {2,1}, {-1,-2}, {1,-2}, {-2,-1}, {2,-1}}; 
                break;

            case PieceEnumType.Piece.blackBishop:
            sizeOfMoves = 28;
                activeMoves = new List<Tile>();
                legalMoves = new int[28,2] 
                {{1,1} ,{2,2} ,{3,3}, {4,4}, {5,5}, {6,6}, {7,7},
                 {-1,-1} ,{-2,-2} ,{-3,-3}, {-4,-4}, {-5,-5}, {-6,-6}, {-7,-7},
                 {-1,1} ,{-2,2} ,{-3,3}, {-4,4}, {-5,5}, {-6,6}, {-7,7},
                 {1,-1} ,{2,-2} ,{3,-3}, {4,-4}, {5,-5}, {6,-6}, {7,-7} };
                break;

            case PieceEnumType.Piece.blackQueen:
            sizeOfMoves = 56;
                activeMoves = new List<Tile>();
                legalMoves = new int[56,2] 
                {{0,1}, {0,2} ,{0,3} ,{0,4} ,{0,5} ,{0,6} ,{0,7},
                 {0,-1} ,{0,-2} ,{0,-3} ,{0,-4} ,{0,-5} ,{0,-6} ,{0,-7},
                 {1,0} ,{2,0} ,{3,0} ,{4,0} ,{5,0} ,{6,0} ,{7,0},
                 {-1,0} ,{-2,0} ,{-3,0} ,{-4,0} ,{-5,0} ,{-6,0} ,{-7,0},
                 {1,1} ,{2,2} ,{3,3}, {4,4}, {5,5}, {6,6}, {7,7},
                 {-1,-1} ,{-2,-2} ,{-3,-3}, {-4,-4}, {-5,-5}, {-6,-6}, {-7,-7},
                 {-1,1} ,{-2,2} ,{-3,3}, {-4,4}, {-5,5}, {-6,6}, {-7,7},
                 {1,-1} ,{2,-2} ,{3,-3}, {4,-4}, {5,-5}, {6,-6}, {7,-7} 
                };
                break;

            case PieceEnumType.Piece.blackKing:
            sizeOfMoves = 8;
                activeMoves = new List<Tile>();
                legalMoves = new int[8,2]
                {{-1,1}, {0,1}, {1,1}, {-1,0}, {1,0}, {-1,-1}, {0,-1}, {1,-1}}; 
                break;

            case PieceEnumType.Piece.empty:
            break;
        }
    }
    public void EvaluateValues()
    {
        switch(type)
        {
            case PieceEnumType.Piece.whitePawn:
                       values = new int[]
                       { 0, 0, 0, 0, 0, 0, 0, 0,
                        5, 10, 10, -20, -20, 10, 10, 5,
                        5, -5, -10, 0, 0, -10, -5, 5,
                        0, 0, 0, 20, 20, 0, 0, 0,
                        5, 5, 10, 25, 25, 10, 5, 5,
                        10, 10, 20, 30, 30, 20, 10, 10,
                        50, 50, 50, 50, 50, 50, 50, 50,
                        0, 0, 0, 0, 0, 0, 0, 0 };
                break;

            case PieceEnumType.Piece.whiteRook:
                        values = new int[]
                        { 0, 0, 0, 5, 5, 0, 0, 0,
                        -5, 0, 0, 0, 0, 0, 0, -5,
                        -5, 0, 0, 0, 0, 0, 0, -5,
                        -5, 0, 0, 0, 0, 0, 0, -5,
                        -5, 0, 0, 0, 0, 0, 0, -5,
                        -5, 0, 0, 0, 0, 0, 0, -5,
                        5, 10, 10, 10, 10, 10, 10, 5,
                        0, 0, 0, 0, 0, 0, 0, 0};
                break;

            case PieceEnumType.Piece.whiteKnight:
            values = new int[]
                        { -50, -40, -30, -30, -30, -30, -40, -50,
                        -40, -20, 0, 5, 5, 0, -20, -40,
                        -30, 5, 10, 15, 15, 10, 5, -30,
                        -30, 0, 15, 20, 20, 15, 0, -30,
                        -30, 5, 15, 20, 20, 15, 5, -30,
                        -30, 0, 10, 15, 15, 10, 0, -30,
                        -40, -20, 0, 0, 0, 0, -20, -40,
                        -50, -40, -30, -30, -30, -30, -40, -50 };
                break;

            case PieceEnumType.Piece.whiteBishop:
            values = new int[]
                        {-20,-10,-10,-10,-10,-10,-10,-20,
                        -10,  5,  0,  0,  0,  0,  5,-10,
                        -10, 10, 10, 10, 10, 10, 10,-10,
                        -10,  0, 10, 10, 10, 10,  0,-10,
                        -10,  5,  5, 10, 10,  5,  5,-10,
                        -10,  0,  5, 10, 10,  5,  0,-10,
                        -10,  0,  0,  0,  0,  0,  0,-10,
                        -20,-10,-10,-10,-10,-10,-10,-20};
                break;

            case PieceEnumType.Piece.whiteQueen:
            values = new int[]
                    {-20,-10,-10, -5, -5,-10,-10,-20,
                    -10,  0,  5,  0,  0,  0,  0,-10,
                    -10,  5,  5,  5,  5,  5,  0,-10,
                    0,  0,  5,  5,  5,  5,  0, -5,
                    -5,  0,  5,  5,  5,  5,  0, -5,
                    -10,  0,  5,  5,  5,  5,  0,-10,
                    -10,  0,  0,  0,  0,  0,  0,-10,
                    -20,-10,-10, -5, -5,-10,-10,-20};
                break;

            case PieceEnumType.Piece.whiteKing:
            values = new int[]
                        {20,30,10, 0, 0,10,30,20,
                        20,20, 0, 0, 0, 0,20,20,
                        -10,-20,-20,-20,-20,-20,-20,-10,
                        -20,-30,-30,-40,-40,-30,-30,-20,
                        -30,-40,-40,-50,-50,-40,-40,-30,
                        -30,-40,-40,-50,-50,-40,-40,-30,
                        -30,-40,-40,-50,-50,-40,-40,-30,
                        -30,-40,-40,-50,-50,-40,-40,-30};
                break;

            case PieceEnumType.Piece.blackPawn:
                        values = new int[]
                       { 0, 0, 0, 0, 0, 0, 0, 0,
                        -50, -50, -50, -50, -50, -50, -50, -50,
                        -10, -10, -20, -30, -30, -20, -10, -10,
                        -5, -5, -10, -25, -25, -10, -5, -5,
                        0, 0, 0, -20, -20, 0, 0, 0,
                        -5, 5, 10, 0, 0, 10, 5, -5,
                        -5, -10, -10, 20, 20, -10, -10, -5,
                        0, 0, 0, 0, 0, 0, 0, 0 };
                break;

            case PieceEnumType.Piece.blackRook:
                        values = new int[]
                        { 0,  0,  0,  0,  0,  0,  0,  0,
                        -5,-10,-10,-10,-10,-10,-10, -5,
                        5,  0,  0,  0,  0,  0,  0,  5,
                        5,  0,  0,  0,  0,  0,  0,  5,
                        5,  0,  0,  0,  0,  0,  0,  5,
                        5,  0,  0,  0,  0,  0,  0,  5,
                        5,  0,  0,  0,  0,  0,  0,  5,
                        0,  0,  0, -5, -5,  0,  0,  0 };
                break;

            case PieceEnumType.Piece.blackKnight:
            values = new int[]
                        { 50, 40, 30, 30, 30, 30, 40, 50,
                        40, 20,  0,  0,  0,  0, 20, 40,
                        30,  0,-10,-15,-15,-10,  0, 30,
                        30, -5,-15,-20,-20,-15, -5, 30,
                        30,  0,-15,-20,-20,-15,  0, 30,
                        30, -5,-10,-15,-15,-10, -5, 30,
                        40, 20,  0, -5, -5,  0, 20, 40,
                        50, 40, 30, 30, 30, 30, 40, 50 };
                break;

            case PieceEnumType.Piece.blackBishop:
            values = new int[]
                        { 20, 10, 10, 10, 10, 10, 10, 20,
                        10,  0,  0,  0,  0,  0,  0, 10,
                        10,  0, -5,-10,-10, -5,  0, 10,
                        10, -5, -5,-10,-10, -5, -5, 10,
                        10,  0,-10,-10,-10,-10,  0, 10,
                        10,-10,-10,-10,-10,-10,-10, 10,
                        10, -5,  0,  0,  0,  0, -5, 10,
                        20, 10, 10, 10, 10, 10, 10, 20 };
                break;
                
            case PieceEnumType.Piece.blackQueen:
            values = new int[]
                        {20, 10, 10, 5, 5, 10, 10, 20,
                        10, 0, 0, 0, 0, 0, 0, 10,
                        10, 0, -5, -5, -5, -5, 0, 10,
                        5, 0, -5, -5, -5, -5, 0, 5,
                        0, 0, -5, -5, -5, -5, 0, 5,
                        10, -5, -5, -5, -5, -5, 0, 10,
                        10, 0, -5, 0, 0, 0, 0, 10,
                        20, 10, 10, 5, 5, 10, 10, 20 };
                break;

            case PieceEnumType.Piece.blackKing:
            values = new int[]
                        {30, 40, 40, 50, 50, 40, 40, 30,
                        30, 40, 40, 50, 50, 40, 40, 30,
                        30, 40, 40, 50, 50, 40, 40, 30,
                        30, 40, 40, 50, 50, 40, 40, 30,
                        20, 30, 30, 40, 40, 30, 30, 20,
                        10, 20, 20, 20, 20, 20, 20, 10,
                        -20,-20,  0,  0,  0,  0,-20,-20,
                        -20,-30,-10,  0,  0,-10,-30,-20 };
                break;
            case PieceEnumType.Piece.empty:
            break;
        }
    }
}
