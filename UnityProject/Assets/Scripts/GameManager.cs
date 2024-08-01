// © 2023 Dominik Kowalkowski
// All rights reserved.
// This source code may not be copied, modified, distributed, or used in any way without prior written permission from the author.
// Permission is granted to HR personnel to review this code for recruitment purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private Transform cam;
    public GameObject chessBoardPrefab;
    public ChessBoard chessBoard;
    public Tile lastUsedTile;
    public Piece lastUsedPiece;
    public string ChessBoard;
    public bool isWhiteTurn;
    public bool isGameOver;
    public int turn;
    public int helpX;
    public int helpY;
    public int destroyCount;

    void Start()
    {
        GameObject cb = Instantiate(chessBoardPrefab);
        chessBoard = cb.GetComponent<ChessBoard>();
        chessBoard.GenerateChessBoard(this);
        chessBoard.SetupPieces("rnbqkbnrpppppppp................................PPPPPPPPRNBQKBNR", 32);
        isWhiteTurn = true;
        isGameOver = false;
        turn = 0;
        TourControl(true);
        cam.transform.position = new Vector3(3.5f, 3.5f, -10);
        ChessBoard = "";
        
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void AiMove()
    {
        int score;
        int tileIndex;
        PieceEnumType.Piece pieceToMove;
        Piece pc = null;
        float xPos, yPos;
        (score, tileIndex, pieceToMove, xPos, yPos) = Minimax(2, chessBoard.ChessBoardStan(), true);
        if(chessBoard.piecePicked != null)
        {
            chessBoard.piecePicked.ClearMoves();
            chessBoard.SetToNotMoveable();
            chessBoard.piecePicked = null;
        }
        for(int x = 16; x < 32; x++)
        {
            if(chessBoard.pieces[x].type == pieceToMove && chessBoard.pieces[x].transform.position.x == xPos && chessBoard.pieces[x].transform.position.y == yPos)
                pc = chessBoard.pieces[x];
        }
        Debug.Log(pc.type +" at " +chessBoard.tiles[tileIndex] );
        chessBoard.SetupMoves(pc);
        pc.ShowMoves();
        chessBoard.Move(chessBoard.tiles[tileIndex], pc, false);
        turn = 0;
        TourControl(true);
    }

    public void PiecePicked(Piece chosenPiece)
    {
        if(chessBoard.piecePicked == null && chosenPiece.colorActive)
        {
            chessBoard.SetToNotMoveable();
            chessBoard.SetupMoves(chosenPiece);
            if(chosenPiece.activeMoves.Count == 0)
            {
                chessBoard.piecePicked.ClearMoves();
                chessBoard.SetToNotMoveable();
                chessBoard.piecePicked = null;
            }
        }
        if(chessBoard.piecePicked != null && chosenPiece.color == 1)
        {   
            for(int x = 0; x < 64; x++)
            {
                if(chessBoard.tiles[x].x == chosenPiece.x && chessBoard.tiles[x].y == chosenPiece.y)
                {
                    if(chessBoard.tiles[x].moveable == true)
                    {
                        if((chessBoard.piecePicked.type == PieceEnumType.Piece.whitePawn 
                            || chessBoard.piecePicked.type == PieceEnumType.Piece.blackPawn) 
                            && chessBoard.piecePicked.firstMoveForPawn == false)
                        {
                            chessBoard.piecePicked.firstMoveForPawn = true;
                            chessBoard.piecePicked.mxfirstMoveForPawn = true;
                        }

                        if(chessBoard.tiles[x].x == chosenPiece.x && chessBoard.tiles[x].y == chosenPiece.y)
                        {
                            chessBoard.tiles[x].AttachPiece(chessBoard.piecePicked.type);
                        }

                        chosenPiece.GetComponent<SpriteRenderer>().enabled = false;
                        chosenPiece.GetComponent<BoxCollider>().enabled = false;
                        chessBoard.piecePicked.AddCurrentTile(chessBoard.tiles[x]);
                        chessBoard.piecePicked.UpdateTransform(chessBoard.tiles[x].x, chessBoard.tiles[x].y);
                        if(chosenPiece.type == PieceEnumType.Piece.whiteKing 
                            || chosenPiece.type == PieceEnumType.Piece.blackKing)
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                        chosenPiece.SetInActive();    
                        chessBoard.startTile.RemovePiece();
                        chessBoard.piecePicked.ClearMoves();
                        chessBoard.SetToNotMoveable();
                        chessBoard.piecePicked = null;
                        ChessBoard = chessBoard.ChessBoardStan();
                        chessBoard.PreviousLocatioon();
                        turn = 1;
                        TourControl(false);
                        break;
                    }
                    else
                    {
                        chessBoard.piecePicked.ClearMoves();
                        chessBoard.SetToNotMoveable();
                        chessBoard.piecePicked = null;
                    }
                }
            }
        }
    }
    public void TilePicked(Tile chosenTile)
    {
        if(chessBoard.piecePicked != null && turn % 2 == 0)
        {
            if(chosenTile.moveable)
            {
                if(chessBoard.piecePicked.type == PieceEnumType.Piece.whitePawn 
                    && chessBoard.piecePicked.firstMoveForPawn == false)
                {
                    chessBoard.piecePicked.firstMoveForPawn = true;
                    chessBoard.piecePicked.mxfirstMoveForPawn = true;
                }
                if(chosenTile.pieceTypeOn == PieceEnumType.Piece.empty)
                {
                    chessBoard.piecePicked.AddCurrentTile(chosenTile);
                    chessBoard.piecePicked.UpdateTransform(chosenTile.x, chosenTile.y);
                    chosenTile.AttachPiece(chessBoard.piecePicked.type);
                    chessBoard.startTile.RemovePiece();
                    
                    chessBoard.piecePicked.ClearMoves();
                    chessBoard.SetToNotMoveable();
                    chessBoard.piecePicked = null;
                    ChessBoard = chessBoard.ChessBoardStan();
                    chessBoard.PreviousLocatioon();
                    turn = 1;
                    TourControl(false);
                }
            }
            else
            {
                chessBoard.piecePicked.ClearMoves();
                chessBoard.SetToNotMoveable();
                chessBoard.piecePicked = null;
            }
        }
    }
    public int CountPieces(string board)
    {
        int count = 0;
        for(int x = 0; x < 64; x++)
        {
            if(board[x] != '.')
                count++;
        }
        return count;
    }
    private (int, int, PieceEnumType.Piece, float, float ) Minimax(int depth, string board, bool maximizingPlayer)
    {
        if (depth == 0 || isGameOver)
        {
            return (Evaluate(), 0, PieceEnumType.Piece.empty, 0.0f, 0.0f);
        }
        if (maximizingPlayer)
        {
            int minScore = int.MaxValue;
            PieceEnumType.Piece bestPiece = PieceEnumType.Piece.empty;
            float pieceX = -1.0f, pieceY = -1.0f; 
            int TileIndex = -1;

            for(int x = 0; x < CountPieces(board); x++)
            {
                GameObject tmp = Instantiate(chessBoardPrefab);
                ChessBoard newBoard = tmp.GetComponent<ChessBoard>();
                newBoard.GenerateChessBoard(this);
                newBoard.SetupPieces(board,  CountPieces(board));
                if(newBoard.pieces[x].color == 1)
                {
                    newBoard.SetupMoves(newBoard.pieces[x]);
                    if(newBoard.pieces[x].type != PieceEnumType.Piece.empty)
                    {
                        foreach (Tile move in newBoard.piecePicked.activeMoves)
                        {
                            newBoard.piecePicked = newBoard.pieces[x];
                            newBoard.Move(move, newBoard.pieces[x], true);
                            int score = newBoard.pieces[x].UpdateValue() + Minimax(depth - 1,  newBoard.ChessBoardStan(), false).Item1;
                            newBoard.UnDoMove();
                            if(score < minScore)
                            {
                                bestPiece = newBoard.pieces[x].type;
                                pieceX = newBoard.pieces[x].transform.position.x;
                                pieceY = newBoard.pieces[x].transform.position.y;
                                TileIndex = move.number;
                                minScore = score;
                            }
                        }
                    }
                        newBoard.DestroyChessBoard();
                }
                else
                    newBoard.DestroyChessBoard();
            }
            return (minScore, TileIndex, bestPiece, pieceX, pieceY);
        } 
        else
        {
            int maxScore = int.MinValue;
            PieceEnumType.Piece bestPiece = PieceEnumType.Piece.empty;
            float pieceX = -1.0f, pieceY = -1.0f; 
            int TileIndex = -1;

            for(int x = 0; x < CountPieces(board); x++)
            {
                    GameObject tmp = Instantiate(chessBoardPrefab);
                    ChessBoard newBoard = tmp.GetComponent<ChessBoard>();
                    newBoard.GenerateChessBoard(this);
                    newBoard.SetupPieces(board, CountPieces(board));
                if(newBoard.pieces[x].color == 0)
                {
                    newBoard.SetupMoves(newBoard.pieces[x]);
                    if(newBoard.pieces[x].type != PieceEnumType.Piece.empty)
                    {
                        foreach (Tile move in newBoard.piecePicked.activeMoves)
                        {
                            newBoard.piecePicked = newBoard.pieces[x];
                            newBoard.Move(move, newBoard.pieces[x], true);
                            int score = newBoard.pieces[x].UpdateValue() + Minimax(depth - 1,  newBoard.ChessBoardStan(), false).Item1;
                            newBoard.UnDoMove();

                            if(score > maxScore)
                            {
                                bestPiece = newBoard.pieces[x].type;
                                pieceX = newBoard.pieces[x].transform.position.x;
                                pieceY = newBoard.pieces[x].transform.position.y;
                                TileIndex = move.number;
                                maxScore = score;
                            }
                        }
                    }
                        newBoard.DestroyChessBoard();
                }
                else
                    newBoard.DestroyChessBoard();
            }
            return (maxScore, TileIndex, bestPiece, pieceX, pieceY);
        }
    }
    
    public int Evaluate()
    {
        int value = 0;

        for(int x = 0; x < 32; x++)
        {
            int pieceValue = 0;
            switch(chessBoard.pieces[x].type)
            {
                case PieceEnumType.Piece.whitePawn:
                pieceValue += 100;
                    break;

                case PieceEnumType.Piece.whiteRook:
                pieceValue += 500;
                    break;

                case PieceEnumType.Piece.whiteKnight:
                pieceValue += 300;
                    break;

                case PieceEnumType.Piece.whiteBishop:
                pieceValue += 300;
                    break;
                    
                case PieceEnumType.Piece.whiteQueen:
                pieceValue += 900;
                    break;

                case PieceEnumType.Piece.whiteKing:
                pieceValue += 10000;
                    break;

                case PieceEnumType.Piece.blackPawn:
                pieceValue += -100;
                    break;

                case PieceEnumType.Piece.blackRook:
                pieceValue += -500;
                    break;

                case PieceEnumType.Piece.blackKnight:
                pieceValue += -300;
                    break;

                case PieceEnumType.Piece.blackBishop:
                pieceValue += -300;
                    break;

                case PieceEnumType.Piece.blackQueen:
                pieceValue += -900;
                    break;

                case PieceEnumType.Piece.blackKing:
                pieceValue += -10000;
                    break;

                case PieceEnumType.Piece.empty:
                break;
            }
            value += pieceValue;
        }
        return value;
    }
    public void TourControl(bool isWhiteTurn)
    {
        if(isWhiteTurn)
        {
            for(int x = 0; x < 32; x++)
            {
                if(chessBoard.pieces[x].color == 0)
                    chessBoard.pieces[x].colorActive = true;
                else
                    chessBoard.pieces[x].colorActive = false;
            }
        }
        else
        {
            for(int x = 0; x < 32; x++)
            {
                if(chessBoard.pieces[x].color == 1)
                    chessBoard.pieces[x].colorActive = true;
                else
                    chessBoard.pieces[x].colorActive = false;
            }
            AiMove();
        }
    }
    public bool isTheSamePosition(Tile t, Piece p)
    {
        if(t.x == p.x && t.y == p.y)
            return true;
        else
            return false;
    }
    
}


