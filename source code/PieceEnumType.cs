// © 2023 Dominik Kowalkowski
// All rights reserved.
// This source code may not be copied, modified, distributed, or used in any way without prior written permission from the author.
// Permission is granted to HR personnel to review this code for recruitment purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceEnumType : MonoBehaviour
{
    public enum Piece
    {
        whiteKing, whiteQueen, whiteBishop, whiteKnight, whiteRook, whitePawn,
        blackKing, blackQueen, blackBishop, blackKnight, blackRook, blackPawn, empty
    };
}
