using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int x;
    public int y;
    public Board board;


    public enum type //enums son selecciones que podria usar para arrays
    {
        elephant,
        giraffe,
        hippo,
        monkey,
        panda,
        parrot,
        penguin,
        pig,
        rabbit,
        snake
    };

    public type pieceType; //guarda el tipo de la pieza actual

    public void Setup(int x_, int y_, Board board_)
    {
        x = x_;
        y = y_;
        board = board_;
    }
}
