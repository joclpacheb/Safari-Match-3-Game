using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tileObject;
    
    public float cameraSizeOffset;        //camerasize offset es para hacer zoom in o zoom out numeros  negativos en el inspector hacen zoom in, positivos zoom out.
    public float cameraVerticalOffset;    // el cameraVerticaloffset es para resolver el problema entre unity y nuestras coordenadas. Nros negativos suben y postivos bajan.
      

    public GameObject[] availablePieces; //arreglo de piezas disponibles

    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
        PositionCamera();
        SetupPieces(); //para el ancho y alto le pone una pieza en esas posiciones
    }

    private void SetupBoard()
    {
        for(int x=0; x<width; x++)
        {
            for (int y = 0; y<height; y++)
            {
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity); //objeto temporal , la rotacion es con quaternion
                o.transform.parent = transform; //el parent es el objeto board que tiene el script, aqui se igual a que sus componente transform sea igual a la del objeto temporal
                
                o.transform.parent = transform;
                o.GetComponent<Tile>()?.Setup(x, y, this); //this es porque este script es de tipo board.
            }
        }
    }

  private void PositionCamera()
    {
        float newPosX = (float)width / 2f;
        float newPosY = (float)height / 2f;

        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + cameraVerticalOffset, -10f); //resto 0.5 unidades para quitar el offset que tiene el centrado
        
        //queremos que dependiendo del height y el width sea responsive y no se corte.

        float horizontal = width+1; //+1 es un margen que le agregamos.
        float vertical = (height / 2) + 1;

        Camera.main.orthographicSize = horizontal > vertical ? horizontal + cameraSizeOffset : vertical + cameraSizeOffset; //if resumido en una sola linea
    
    }

    private void SetupPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)]; //numero inicial exclusivo y nro final inclusivo
                var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity); //setup pieces selecciona una pieza del nuevo arreglo y la pone en el tablero.
                o.transform.parent = transform;
                o.GetComponent<Piece>()?.Setup(x, y, this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
