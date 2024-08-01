// © 2023 Dominik Kowalkowski
// All rights reserved.
// This source code may not be copied, modified, distributed, or used in any way without prior written permission from the author.
// Permission is granted to HR personnel to review this code for recruitment purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public Tile[] tiles;
    public Piece[] pieces;
    public GameManager gameManager;
    [SerializeField] private Tile tilePrefab;
    public GameObject[] gamePieces = new GameObject[12];
    public Tile lastUsedTile;
    public Piece lastUsedPiece;
    [SerializeField]public Piece lastDestroyedPiece;
    public Piece piecePicked;
    public Tile tilePicked;
    public Tile startTile;
    public Tile endTile;
    public float tmpX;
    public float tmpY;
    public float pawnFirstX;
    public float pawnSecondX;
    public float pawnFirstY;
    public float pawnSecondY;
    public string chessBoardString;
    public int pieceCount;
    public void GenerateChessBoard(GameManager gm)
    {
        tiles = new Tile[64];
        gameManager = gm;
        chessBoardString = "";
        for(int y=0; y < 8; y++)
        {
            for(int x=0; x < 8; x++)
            {
                tiles[8*y + x] = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                tiles[8*y + x].Init((x+y) % 2 == 0, 8*y + x, gameManager);
                tiles[8*y + x].name = $"Tile {8*y + x}";
            }
        }
    }
    public void DestroyChessBoard()
    {
        for(int x = 0; x < pieceCount; x++)
        {
            pieces[x].GetComponent<BoxCollider>().enabled = false;
            pieces[x].GetComponent<SpriteRenderer>().enabled = false;
            Destroy(pieces[x].gameObject);
        }
        pieces = new Piece[1];
        for(int x = 0; x < 64; x++)
        {
            tiles[x].GetComponent<BoxCollider>().enabled = false;
            tiles[x].GetComponent<SpriteRenderer>().enabled = false;
            Destroy(tiles[x].gameObject);
        }
        tiles = new Tile[1];
        Destroy(this.gameObject);
    }
    public void SetupPieces(string fen, int dst)
    {
        pieces = new Piece[dst];
        pieceCount = dst;
        GameObject piece;
        int control = 0;

        for(int y = 0; y < 8; y++)
        {
            for(int x=0; x < 8; x++)
            {
                switch(FenNotation(fen[8*y + x])) 
                {
                    case PieceEnumType.Piece.whitePawn:
                        piece = Instantiate(gamePieces[0], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.whitePawn, 0, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.whitePawn}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.whitePawn);
                        if(pieces[8*y + x - control].transform.position.y != 1.0f)
                            pieces[8*y + x - control].firstMoveForPawn = true;
                        break;

                    case PieceEnumType.Piece.whiteRook:
                        piece = Instantiate(gamePieces[1], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.whiteRook, 0, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.whiteRook}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.whiteRook);
                        break;

                    case PieceEnumType.Piece.whiteKnight:
                        piece = Instantiate(gamePieces[2], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.whiteKnight, 0, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.whiteKnight}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.whiteKnight);
                        break;

                    case PieceEnumType.Piece.whiteBishop:
                        piece = Instantiate(gamePieces[3], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.whiteBishop, 0, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.whiteBishop}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.whiteBishop);
                        break;
                        
                    case PieceEnumType.Piece.whiteQueen:
                        piece = Instantiate(gamePieces[4], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.whiteQueen, 0, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.whiteQueen}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.whiteQueen);
                        break;

                    case PieceEnumType.Piece.whiteKing:
                        piece = Instantiate(gamePieces[5], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.whiteKing, 0, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.whiteKing}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.whiteKing);
                        break;

                    case PieceEnumType.Piece.blackPawn:
                        piece = Instantiate(gamePieces[6], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.blackPawn, 1, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.blackPawn}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.blackPawn);
                        if(pieces[8*y + x - control].transform.position.y != 6.0f)
                            pieces[8*y + x - control].firstMoveForPawn = true;
                        break;

                    case PieceEnumType.Piece.blackRook:
                        piece = Instantiate(gamePieces[7], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.blackRook, 1, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.blackRook}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.blackRook);
                        break;

                    case PieceEnumType.Piece.blackKnight:
                        piece = Instantiate(gamePieces[8], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.blackKnight, 1, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.blackKnight}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.blackKnight);
                        break;

                    case PieceEnumType.Piece.blackBishop:
                        piece = Instantiate(gamePieces[9], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.blackBishop, 1, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.blackBishop}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.blackBishop);
                        break;

                    case PieceEnumType.Piece.blackQueen:
                        piece = Instantiate(gamePieces[10], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.blackQueen, 1, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.blackQueen}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.blackQueen);
                        break;

                    case PieceEnumType.Piece.blackKing:
                        piece = Instantiate(gamePieces[11], new Vector3(x, y), Quaternion.identity);
                        pieces[8*y + x - control] = piece.GetComponent<Piece>();
                        pieces[8*y + x - control].Init(PieceEnumType.Piece.blackKing, 1, tiles[8*y + x], gameManager);
                        pieces[8*y + x - control].name = $"{PieceEnumType.Piece.blackKing}";
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.blackKing);
                        break;

                    case PieceEnumType.Piece.empty:
                        control++;
                        tiles[8*y + x].AttachPiece(PieceEnumType.Piece.empty);
                    break;
                }
            }
        }
    }

    public void PreviousLocatioon()
    {
        for(int x = 0; x < pieceCount; x++)
        {
            for(int y = 0; y < 64; y++)
            {
                if(pieces[x].transform.position.x == tiles[y].transform.position.x && pieces[x].transform.position.y == tiles[y].transform.position.y)
                {
                    pieces[x].AddCurrentTile(tiles[y]);
                }
            }
        }
    }

    public void SetToNotMoveable()
    {
        for(int x = 0; x < 64; x++)
        {
            tiles[x].moveable = false;
        }
    }

    public string ChessBoardStan()
    {
        string fenNo = "";
        for(int x = 0; x < 64; x++)
        {

            switch(tiles[x].pieceTypeOn) 
                    {
                        case PieceEnumType.Piece.whitePawn:
                            fenNo += "p";
                            break;

                        case PieceEnumType.Piece.whiteRook:
                            fenNo += "r";
                            break;

                        case PieceEnumType.Piece.whiteKnight:
                            fenNo += "n";
                            break;

                        case PieceEnumType.Piece.whiteBishop:
                            fenNo += "b";
                            break;
                            
                        case PieceEnumType.Piece.whiteQueen:
                            fenNo += "q";
                            break;

                        case PieceEnumType.Piece.whiteKing:
                            fenNo += "k";
                            break;

                        case PieceEnumType.Piece.blackPawn:
                            fenNo += "P";
                            break;

                        case PieceEnumType.Piece.blackRook:
                            fenNo += "R";
                            break;

                        case PieceEnumType.Piece.blackKnight:
                            fenNo += "N";
                            break;

                        case PieceEnumType.Piece.blackBishop:
                            fenNo += "B";
                            break;

                        case PieceEnumType.Piece.blackQueen:
                            fenNo += "Q";
                            break;

                        case PieceEnumType.Piece.blackKing:
                            fenNo += "K";
                            break;
                        case PieceEnumType.Piece.empty:
                            fenNo += ".";
                        break;
                    }
        }
        return fenNo;
    }
    public int pieceValue(PieceEnumType.Piece pc)
    {
        int pieceValue = 0;
        switch(pc) 
            {
                 case PieceEnumType.Piece.whitePawn:
                pieceValue -= 100;// VALUE TO PUNKTY PIONKA ZA STANIE NA KONKRETNEJ POZYCJI, UWZGLEDNIONO TU NAJBARDZIEJ POZYTECZNE POLA DLA DANEJ FIGURY
                    break;

                case PieceEnumType.Piece.whiteRook:
                pieceValue -= 500;
                    break;

                case PieceEnumType.Piece.whiteKnight:
                pieceValue -= 300;
                    break;

                case PieceEnumType.Piece.whiteBishop:
                pieceValue -= 300;
                    break;
                    
                case PieceEnumType.Piece.whiteQueen:
                pieceValue -= 900;
                    break;

                case PieceEnumType.Piece.whiteKing:
                pieceValue -= 10000;
                    break;

                case PieceEnumType.Piece.blackPawn:
                pieceValue += 100;
                    break;

                case PieceEnumType.Piece.blackRook:
                pieceValue += 500;
                    break;

                case PieceEnumType.Piece.blackKnight:
                pieceValue += 300;
                    break;

                case PieceEnumType.Piece.blackBishop:
                pieceValue += 300;
                    break;

                case PieceEnumType.Piece.blackQueen:
                pieceValue += 900;
                    break;

                case PieceEnumType.Piece.blackKing:
                pieceValue += 10000;
                    break;

                case PieceEnumType.Piece.empty:
                break;
            }
        
        return pieceValue;
    }
    public void PreviousStateOfBoard(string fen)
    {
        for(int x = 0; x < 64; x++)
        {
            tiles[x].AttachPiece(FenNotation(fen[x]));
        }
        for(int x = 0; x < pieceCount; x++)
        {
            //pieces[x].BackToPreviousTile();
            if(pieces[x].type == PieceEnumType.Piece.whitePawn || pieces[x].type == PieceEnumType.Piece.blackPawn)
            {
                pieces[x].firstMoveForPawn = pieces[x].mxfirstMoveForPawn;
            }
        }
    }
    PieceEnumType.Piece FenNotation(char fen)
    {
        var figureNameFromSymbol = new Dictionary<char, PieceEnumType.Piece> ()
        {
            ['p'] = PieceEnumType.Piece.whitePawn, ['r'] = PieceEnumType.Piece.whiteRook, ['n'] = PieceEnumType.Piece.whiteKnight, ['b'] = PieceEnumType.Piece.whiteBishop, 
            ['q'] = PieceEnumType.Piece.whiteQueen, ['k'] = PieceEnumType.Piece.whiteKing, ['P'] = PieceEnumType.Piece.blackPawn, ['R'] = PieceEnumType.Piece.blackRook,
            ['N'] = PieceEnumType.Piece.blackKnight, ['B'] = PieceEnumType.Piece.blackBishop, ['Q'] = PieceEnumType.Piece.blackQueen, ['K'] = PieceEnumType.Piece.blackKing,
            ['.'] = PieceEnumType.Piece.empty
        };
        
        return figureNameFromSymbol[fen];
    }

    char FenNotationChar(PieceEnumType.Piece fen)
    {
        var figureNameFromSymbol = new Dictionary<PieceEnumType.Piece, char> ()
        {
            [PieceEnumType.Piece.whitePawn] = 'p', [PieceEnumType.Piece.whiteRook] = 'r', [PieceEnumType.Piece.whiteKnight] = 'n', [PieceEnumType.Piece.whiteBishop] = 'b', 
            [PieceEnumType.Piece.whiteQueen] = 'q', [PieceEnumType.Piece.whiteKing] = 'k', [PieceEnumType.Piece.blackPawn] = 'P', [PieceEnumType.Piece.blackRook] ='R',
            [PieceEnumType.Piece.blackKnight] = 'N', [PieceEnumType.Piece.blackBishop] = 'B', [PieceEnumType.Piece.blackQueen] = 'Q', [PieceEnumType.Piece.blackKing]= 'K',
            [PieceEnumType.Piece.empty] ='.'
        };
        
        return figureNameFromSymbol[fen];
    }
    public void ShowTiles()
    {
        for(int x = 0; x < 64; x++)
        {
            Debug.Log(tiles[x].number + " with " + tiles[x].pieceTypeOn);
        }
    }
    public void SetupMoves(Piece chosenPiece)
    {
        SetToNotMoveable();
        piecePicked = chosenPiece;
        //Debug.Log("yes im here for " + piecePicked.type);
        for(int x = 0; x < 64; x++)
        {
            if(tiles[x].transform.position.x == piecePicked.transform.position.x && tiles[x].transform.position.y == piecePicked.transform.position.y && tiles[x].pieceTypeOn == piecePicked.type)
            {
                //if(piecePicked.color == 1)
                  //  Debug.Log("My start tile is " + tiles[x].number );
                startTile = tiles[x];
            }
        }
        for(int x = 0; x < piecePicked.sizeOfMoves; x++)
        {
            if(piecePicked.type == PieceEnumType.Piece.whitePawn || piecePicked.type == PieceEnumType.Piece.blackPawn)
            {   
                pawnFirstX = piecePicked.transform.position.x + piecePicked.legalMoves[2, 0];
                pawnFirstY = piecePicked.transform.position.y + piecePicked.legalMoves[2, 1];
                pawnSecondX = piecePicked.transform.position.x + piecePicked.legalMoves[3, 0];
                pawnSecondY = piecePicked.transform.position.y + piecePicked.legalMoves[3, 1];
                
                if(piecePicked.firstMoveForPawn == true)
                {
                    tmpX = piecePicked.transform.position.x + piecePicked.legalMoves[0, 0];
                    tmpY = piecePicked.transform.position.y + piecePicked.legalMoves[0, 1];
                }
                else
                {
                    tmpX = piecePicked.transform.position.x + piecePicked.legalMoves[x, 0];
                    tmpY = piecePicked.transform.position.y + piecePicked.legalMoves[x, 1];
                }

                for(int y = 0; y < 64; y++)
                {
                    if(tiles[y].transform.position.x == tmpX && tiles[y].transform.position.y == tmpY && tiles[y].pieceTypeOn == PieceEnumType.Piece.empty)
                    {
                        tiles[y].moveable = true;
                        SetupBlockedMoves(tiles[y]);
                    }
                    if(tiles[y].transform.position.x == pawnFirstX && tiles[y].transform.position.y == pawnFirstY && tiles[y].pieceTypeOn != PieceEnumType.Piece.empty && piecePicked.color != tiles[y].color)
                    {
                        tiles[y].moveable = true;
                        SetupBlockedMoves(tiles[y]);
                    }
                    if(tiles[y].transform.position.x == pawnSecondX && tiles[y].transform.position.y == pawnSecondY && tiles[y].pieceTypeOn != PieceEnumType.Piece.empty && piecePicked.color != tiles[y].color)
                    {
                        tiles[y].moveable = true;
                        SetupBlockedMoves(tiles[y]);
                    }
                }

            }
            else
            {
                tmpX = piecePicked.transform.position.x + piecePicked.legalMoves[x, 0];
                tmpY = piecePicked.transform.position.y + piecePicked.legalMoves[x, 1];

                for(int y = 0; y < 64; y++)
                {
                    if(tiles[y].transform.position.x == tmpX && tiles[y].transform.position.y == tmpY && tiles[y].color != piecePicked.color)
                    {
                        tiles[y].moveable = true;
                        SetupBlockedMoves(tiles[y]);
                    }
                }
            }
        }
        
        for(int x = 0; x < 64; x++)
        {
            if(tiles[x].moveable)
            {
                piecePicked.AddMove(tiles[x]);
            }
        }
    }
    //blokuje ruch jesli na drodze pionka do chosenTile jest inny pionek
    public void SetupBlockedMoves(Tile chosenTile)
    {
        tilePicked = chosenTile;
        // BLOKOWANIE RUCHU W PRZYPADKU PRZESZKODY NA OSI X-Y do poprawienie dla konia
            if (tilePicked.transform.position.x != piecePicked.transform.position.x && tilePicked.transform.position.y != piecePicked.transform.position.y && 
                piecePicked.type != PieceEnumType.Piece.whiteKnight && piecePicked.type != PieceEnumType.Piece.blackKnight) 
            {
                if(tilePicked.transform.position.y - piecePicked.transform.position.y > 0 && tilePicked.transform.position.x - piecePicked.transform.position.x > 0)
                {
                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.y; x <= (int)tilePicked.transform.position.y - 1; x++ )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index - 9;
                    }
                }
                else if(tilePicked.transform.position.y - piecePicked.transform.position.y > 0 && tilePicked.transform.position.x - piecePicked.transform.position.x < 0)
                {

                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.y; x <= (int)tilePicked.transform.position.y - 1; x++ )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index - 7;
                    }

                }
                else if(tilePicked.transform.position.y - piecePicked.transform.position.y < 0 && tilePicked.transform.position.x - piecePicked.transform.position.x < 0)
                {

                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.y; x >= (int)tilePicked.transform.position.y + 1; x-- )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index + 9;
                    }

                }
                else if(tilePicked.transform.position.y - piecePicked.transform.position.y < 0 && tilePicked.transform.position.x - piecePicked.transform.position.x > 0)
                {

                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.y; x >= (int)tilePicked.transform.position.y + 1; x-- )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index + 7;
                    }

                }
            }
            // BLOKOWANIE RUCHU W PRZYPADKU PRZESZKODY NA OSI Y
            else if(tilePicked.transform.position.x == piecePicked.transform.position.x)
            {
                if(tilePicked.transform.position.y - piecePicked.transform.position.y > 0)
                {
                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.y; x <= (int)tilePicked.transform.position.y - 1; x++ )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index - 8;
                    }
                }
                else if(tilePicked.transform.position.y - piecePicked.transform.position.y < 0)
                {
                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.y; x >= (int)tilePicked.transform.position.y + 1; x-- )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index + 8;
                    }
                }
            }
            // BLOKOWANIE RUCHU W PRZYPADKU PRZESZKODY NA OSI X
            else if(tilePicked.transform.position.y == piecePicked.transform.position.y)
            {
                if(tilePicked.transform.position.x - piecePicked.transform.position.x > 0)
                {
                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.x; x <= (int)tilePicked.transform.position.x - 1; x++ )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index - 1;
                    }
                }
                else if(tilePicked.transform.position.x - piecePicked.transform.position.x < 0)
                {
                    int index = tilePicked.number;
                    for(int x = (int)piecePicked.transform.position.x; x >= (int)tilePicked.transform.position.x + 1; x-- )
                    {
                        if(tiles[index].pieceTypeOn != PieceEnumType.Piece.empty && index != tilePicked.number)
                        {
                            tilePicked.moveable = false;
                        }
                        index = index + 1;
                    }
                }
            }
    }

    public void UnDoMove()
    {
        lastUsedPiece.UpdateTransform(lastUsedTile.transform.position.x, lastUsedTile.transform.position.y);
        lastUsedTile.pieceTypeOn = lastUsedPiece.type;
        lastUsedPiece.currentTile.pieceTypeOn = PieceEnumType.Piece.empty;
        lastUsedPiece.AddCurrentTile(lastUsedTile);
        lastUsedPiece.ReActiveMoves();
        lastUsedPiece.pieceTakeoverValue = 0;
        if(lastDestroyedPiece != null)
        {
            lastDestroyedPiece.GetComponent<SpriteRenderer>().enabled = false;
            lastDestroyedPiece.GetComponent<BoxCollider>().enabled = false;
            lastDestroyedPiece.BackToPreviousState();
        }
        //Debug.Log("cofnieto ruch");
    }
    public void Move(Tile chosenTile, Piece chosenPiece, bool isMinimax)
    {
        chessBoardString = ChessBoardStan();
        piecePicked = chosenPiece;
        if(chosenTile.moveable)
        {
            if((piecePicked.type == PieceEnumType.Piece.whitePawn || piecePicked.type == PieceEnumType.Piece.blackPawn) && piecePicked.firstMoveForPawn == false)
                piecePicked.firstMoveForPawn = true;

            if(chosenTile.pieceTypeOn != PieceEnumType.Piece.empty && piecePicked.color != chosenTile.color)
            {
                for(int x = 0; x < pieceCount; x++)
                {
                    if(pieces[x].transform.position.x == chosenTile.transform.position.x && pieces[x].transform.position.y == chosenTile.transform.position.y && pieces[x].type != PieceEnumType.Piece.empty)
                    {
                        chosenPiece.pieceTakeoverValue = pieceValue(pieces[x].type);
                        Debug.Log(piecePicked.type + " destroyed " + pieces[x].type + " at " + chosenTile.number);
                        pieces[x].GetComponent<SpriteRenderer>().enabled = false;
                        pieces[x].GetComponent<BoxCollider>().enabled = false;
                        lastUsedTile = piecePicked.currentTile;
                        lastUsedPiece = piecePicked;
                        piecePicked.AddCurrentTile(chosenTile);
                        piecePicked.UpdateTransform(pieces[x].transform.position.x, pieces[x].transform.position.y);
                        if(pieces[x].type == PieceEnumType.Piece.whiteKing || pieces[x].type == PieceEnumType.Piece.blackKing)
                        {
                            if(isMinimax == false)
                                gameManager.ResetGame();
                        }
                        pieces[x].AddPreviousState(pieces[x].type);
                        chosenPiece.pieceTakeoverValue = pieceValue(pieces[x].type);
                        pieces[x].type = PieceEnumType.Piece.empty;
                        lastDestroyedPiece = pieces[x];
                        chosenTile.AttachPiece(piecePicked.type);
                        startTile.RemovePiece();
                        
                        piecePicked.CopyMoves();
                        piecePicked.ClearMoves();

                        /////
                        /////
                        piecePicked = null;
                        SetToNotMoveable();
                        //turn = 0;
                        //TourControl(true);
                        break;
                    }
                }
                
            }
            else if(chosenTile.pieceTypeOn == PieceEnumType.Piece.empty)
            { 
                //Debug.Log(piecePicked.type + " moved at " + chosenTile.number);
                lastUsedTile = piecePicked.currentTile;
                lastUsedPiece = piecePicked;
                piecePicked.AddCurrentTile(chosenTile);
                piecePicked.UpdateTransform(chosenTile.transform.position.x, chosenTile.transform.position.y);
                chosenTile.AttachPiece(piecePicked.type);
                startTile.RemovePiece();
                
                piecePicked.CopyMoves();
                piecePicked.ClearMoves();
                SetToNotMoveable();
                //Debug.Log(piecePicked.type + " at " + piecePicked.transform.position.x + " " +  piecePicked.transform.position.y);
                //turn = 0;
                //TourControl(true);
                piecePicked = null;
            }
            
        }
        else
        {
            piecePicked.ClearMoves();
            SetToNotMoveable();
            piecePicked = null;
        }
    }
}
