using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //libreria dotween


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

        public void Move(int desX, int desY) //des es destinox o destino y . Ease inout y set ease tiene easing en el tween animation. On complete lo que hace es concatenar la accion de actualizar x y y y mover las piezas.
    {
        transform.DOMove(new Vector3(desX, desY, -5f), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = desX;
            y = desY;
        };
    }

 [ContextMenu("Test Move")] //esto es un decorador
    public void MoveTest()
    {
        Move(0, 0);
    }
}
