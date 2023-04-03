using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Board : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tileObject;
    
    public float cameraSizeOffset;        //camerasize offset es para hacer zoom in o zoom out numeros  negativos en el inspector hacen zoom in, positivos zoom out.
    public float cameraVerticalOffset;    // el cameraVerticaloffset es para resolver el problema entre unity y nuestras coordenadas. Nros negativos suben y postivos bajan.
      

    public GameObject[] availablePieces; //arreglo de piezas disponibles

    Tile[,] Tiles; //un array de 2 dimensiones para los espacios de las cuadriculas
    Piece[,] Pieces; //un array de 2 dimensiones para los las piezas

    Tile startTile;
    Tile endTile;

    bool swappingPieces = false;

    // Start is called before the first frame update
    void Start()
    {


        Tiles = new Tile[width, height];
        Pieces = new Piece[width, height];

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
                Tiles[x, y] = o.GetComponent<Tile>();
                Tiles[x,y]?.Setup(x, y, this);

                //o.GetComponent<Tile>()?.Setup(x, y, this); //this es porque este script es de tipo board.
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

    // private void SetupPieces()
    // {
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)]; //numero inicial exclusivo y nro final inclusivo
    //             var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity); //setup pieces selecciona una pieza del nuevo arreglo y la pone en el tablero.
    //             o.transform.parent = transform;
    //             Pieces[x, y] = o.GetComponent<Piece>();
    //             Pieces[x, y].Setup(x, y, this);
    //         }
    //     }
    // }

   private void SetupPieces()
    {
        int maxIterations = 50;
        int currentIteration = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                currentIteration = 0;
                var newPiece = CreatePieceAt(x, y);
                while(HasPreviousMatches(x, y))
                {
                    ClearPieceAt(x, y);
                    newPiece = CreatePieceAt(x, y);
                    currentIteration++;
                    if (currentIteration > maxIterations)
                    {
                        break;
                    }
                }
            }
        }
    }

    private void ClearPieceAt(int x, int y)
    {
        var pieceToClear = Pieces[x, y];
        Destroy(pieceToClear.gameObject);
        Pieces[x, y] = null;
    }

    bool HasPreviousMatches(int posx, int posy)
    {
        var downMatches = GetMatchByDirection(posx, posy, new Vector2(0, -1), 2);
        var leftMatches = GetMatchByDirection(posx, posy, new Vector2(-1, 0), 2);

        if (downMatches == null) downMatches = new List<Piece>();
        if (leftMatches == null) leftMatches = new List<Piece>();

        return (downMatches.Count > 0 || leftMatches.Count > 0);

    }

        private Piece CreatePieceAt(int x, int y)
    {
        var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)];
        var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity);
        o.transform.parent = transform;
        Pieces[x, y] = o.GetComponent<Piece>();
        Pieces[x, y].Setup(x, y, this);
        return Pieces[x, y];
    }


    //funciones que permiten mover las piezas

    public void TileDown(Tile tile_) //cuando clickeamos
    {
        startTile = tile_;
    }

    public void TileOver(Tile tile_) //arrastro el mouse sobre otro espacio de la cuadricula
    {
        endTile = tile_;
    }

    // public void TileUp(Tile tile_) //levantar el click
    // {
    //     //if(startTile!=null && endTile != null && IsCloseTo(startTile, endTile)) //verifica que tenemos un start tile y un entile para swapear y si no no hago nada.
    //     if(startTile!=null && endTile != null && IsCloseTo(startTile, endTile)) //verifica que tenemos un start tile y un entile para swapear y si no no hago nada.

    //     {
    //           StartCoroutine(SwapTiles());
    //     }
    //     startTile = null; //limpio
    //     endTile = null;
    // }

    public void TileUp(Tile tile_)
    {
        if (startTile != null && endTile != null && IsCloseTo(startTile, endTile))
        {
            StartCoroutine(SwapTiles());
        }
    }

    // private void SwapTiles() //debo conseguir una referencia a las pieza del estado incial y final
    // {
    //     var StarPiece = Pieces[startTile.x, startTile.y];
    //     var EndPiece = Pieces[endTile.x, endTile.y];

    //     StarPiece.Move(endTile.x, endTile.y);
    //     EndPiece.Move(startTile.x, startTile.y);

    //     Pieces[startTile.x, startTile.y] = EndPiece; //actualizo el sistema de coordenadas
    //     Pieces[endTile.x, endTile.y] = StarPiece;
    // }

 IEnumerator SwapTiles()
    {
        var StarPiece = Pieces[startTile.x, startTile.y];
        var EndPiece = Pieces[endTile.x, endTile.y];

        StarPiece.Move(endTile.x, endTile.y);
        EndPiece.Move(startTile.x, startTile.y);

        Pieces[startTile.x, startTile.y] = EndPiece;
        Pieces[endTile.x, endTile.y] = StarPiece;

        yield return new WaitForSeconds(0.6f);

        bool foundMatch = false;
        var startMatches = GetMatchByPiece(startTile.x, startTile.y, 3);
        var endMatches = GetMatchByPiece(endTile.x, endTile.y, 3);

        startMatches.ForEach(piece =>
        {
            foundMatch = true;
            ClearPieceAt(piece.x, piece.y);
        });

        endMatches.ForEach(piece =>
        {
            foundMatch = true;
            Pieces[piece.x, piece.y] = null;
            ClearPieceAt(piece.x, piece.y);
        });

        if (!foundMatch)
        {
            StarPiece.Move(startTile.x, startTile.y);
            EndPiece.Move(endTile.x, endTile.y);
            Pieces[startTile.x, startTile.y] = StarPiece;
            Pieces[endTile.x, endTile.y] = EndPiece;
        }

        startTile = null;
        endTile = null;
        swappingPieces = false;

        yield return null;
    }

public bool IsCloseTo(Tile start, Tile end)
    {
        if (Math.Abs((start.x - end.x)) == 1 && start.y == end.y) //eje horizontal
        {
            return true;
        }
        if (Math.Abs((start.y - end.y)) == 1 && start.x == end.x) //eje vertical
        {
            return true;
        }
        return false;
    }

     public List<Piece> GetMatchByPiece(int xpos, int ypos, int minPieces = 3)
    {
        var upMatchs = GetMatchByDirection(xpos, ypos, new Vector2(0, 1), 2);
        var downMatchs = GetMatchByDirection(xpos, ypos, new Vector2(0, -1), 2);
        var rightMatchs = GetMatchByDirection(xpos, ypos, new Vector2(1, 0), 2);
        var leftMatchs = GetMatchByDirection(xpos, ypos, new Vector2(-1, 0), 2);

        if (upMatchs == null) upMatchs = new List<Piece>();
        if (downMatchs == null) downMatchs = new List<Piece>();
        if (rightMatchs == null) rightMatchs = new List<Piece>();
        if (leftMatchs == null) leftMatchs = new List<Piece>();

        var verticalMatches = upMatchs.Union(downMatchs).ToList();
        var horizontalMatches = leftMatchs.Union(rightMatchs).ToList();

        var foundMatches = new List<Piece>();

        if (verticalMatches.Count >= minPieces)
        {
            foundMatches = foundMatches.Union(verticalMatches).ToList();
        }
        if (horizontalMatches.Count >= minPieces)
        {
            foundMatches = foundMatches.Union(horizontalMatches).ToList();
        }

        return foundMatches;
    }

public List<Piece> GetMatchByDirection(int xpos, int ypos, Vector2 direction, int minPieces = 3)
    {
        List<Piece> matches = new List<Piece>();
        Piece startPiece = Pieces[xpos, ypos];
        matches.Add(startPiece);

        int nextX;
        int nextY;
        int maxVal = width > height ? width : height;

        for (int i = 1; i < maxVal; i++)
        {
            nextX = xpos + ((int)direction.x * i);
            nextY = ypos + ((int)direction.y * i);
            if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height)
            {
                var nextPiece = Pieces[nextX, nextY];
                if (nextPiece != null && nextPiece.pieceType == startPiece.pieceType)
                {
                    matches.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
        }

        if (matches.Count >= minPieces)
        {
            return matches;
        }

        return null;
    }
}
