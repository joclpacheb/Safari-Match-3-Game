// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening; //libreria dotween


// public class Piece : MonoBehaviour
// {
//     public int x;
//     public int y;
//     public Board board;


//     public enum type //enums son selecciones que podria usar para arrays
//     {
//         elephant,
//         giraffe,
//         hippo,
//         monkey,
//         panda,
//         parrot,
//         penguin,
//         pig,
//         rabbit,
//         snake
//     };

//     public type pieceType; //guarda el tipo de la pieza actual

//     public void Setup(int x_, int y_, Board board_)
//     {
//         x = x_;
//         y = y_;
//         board = board_;
//     }

//         public void Move(int desX, int desY) //des es destinox o destino y . Ease inout y set ease tiene easing en el tween animation. On complete lo que hace es concatenar la accion de actualizar x y y y mover las piezas.
//     {
//         transform.DOMove(new Vector3(desX, desY, -5f), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
//         {
//             x = desX;
//             y = desY;
//         };
//     }

//  [ContextMenu("Test Move")] //esto es un decorador para funciones que no reciban parametros y es para probar dandole a los 3 puntos del componente script como se ve la animacion
//     public void MoveTest()
//     {
//         Move(0, 0);
//     }
// }



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour
{
    public int x;
    public int y;
    public Board board;


    public enum type
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

    public type pieceType;

    public void Setup(int x_, int y_, Board board_)
    {
        x = x_;
        y = y_;
        board = board_;

        transform.localScale = Vector3.one * 0.35f;
        transform.DOScale(Vector3.one, 0.35f);
    }

    public void Move(int desX, int desY)
    {
        transform.DOMove(new Vector3(desX, desY, -5f), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = desX;
            y = desY;
        };
    }

    public void Remove(bool animated)
    {
        if (animated)
        {
            transform.DORotate(new Vector3(0, 0, -120f), 0.12f);
            transform.DOScale(Vector3.one * 1.2f, 0.085f).onComplete = () =>
            {
                transform.DOScale(Vector3.zero, 0.1f).onComplete = () =>
                {
                    Destroy(gameObject);
                };
            };
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Test Move")]
    public void MoveTest()
    {
        Move(0, 0);
    }
}
