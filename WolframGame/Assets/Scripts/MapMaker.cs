using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMaker : MonoBehaviour {
    public Tile tierra;
    public Tile agua;
    public Tile piedra;
    public Tile lava;
    public Tile nieve;
    public Tile baseRoja;
    public Tile inicioJugadorRojo;
    public Tile baseAzul;
    public Tile inicioJugadorAzul;
    public Tile caminoRojo;
    public Tile caminoAzul;
    public Tilemap tilemap;

    public int mapWidth;
    public int mapHeight;
    public GameObject rojoWin;
    public GameObject azulWin;

    private int[,] mapData;
    public CellularData cell;
    private bool hayGanador = false;

    void Start() {
        mapData = cell.GenerateData(mapWidth, mapHeight);
        GenerateTiles();
    }

    void GenerateTiles() {
        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                Vector3Int position = new Vector3Int(i, j, 0);
                int tileType = mapData[i, j];

                if (tileType == 1) {
                    tilemap.SetTile(position, tierra);
                }
                else if (tileType == 2) {
                    tilemap.SetTile(position, agua);
                }
                else if (tileType == 3) {
                    tilemap.SetTile(position, piedra);
                }
                else if (tileType == 4) {
                    tilemap.SetTile(position, lava);
                }
                else if (tileType == 5) {
                    tilemap.SetTile(position, nieve);
                }
                else if (tileType == 6) {
                    tilemap.SetTile(position, baseRoja);
                }
                else if (tileType == 7) {
                    tilemap.SetTile(position, inicioJugadorRojo);
                }
                else if (tileType == 8) {
                    tilemap.SetTile(position, baseAzul);
                }
                else if (tileType == 9) {
                    tilemap.SetTile(position, inicioJugadorAzul);
                }
            }
        }
    }

    public void PintarCaminoDesdeInicioJugadorRojo() {
        Vector2Int inicioRojo = EncontrarPosicionDeTile(7);
        Vector2Int baseRojaPos = EncontrarPosicionDeTile(6); 
        List<Vector2Int> camino = cell.CalcularCaminoAStar(mapData, inicioRojo, baseRojaPos);

        if (camino != null) {
            StartCoroutine(PintarCaminoGradualRojo(camino));
        }
        else {
            Debug.Log("No se encontró un camino desde el inicio del jugador rojo a la base roja.");
        }
    }
    public void PintarCaminoDesdeInicioJugadorAzul() {
        Vector2Int inicioAzul = EncontrarPosicionDeTile(9);
        Vector2Int baseAzulPos = EncontrarPosicionDeTile(8); 
        List<Vector2Int> camino = cell.CalcularCaminoAStar(mapData, inicioAzul, baseAzulPos);

        if (camino != null) {
            StartCoroutine(PintarCaminoGradualAzul(camino));
            Debug.Log("hola");
            rojoWin.SetActive(true);
        }
        else {
            Debug.Log("No se encontró un camino desde el inicio del jugador rojo a la base roja.");
        }
    }

    IEnumerator PintarCaminoGradualRojo(List<Vector2Int> camino) {
        foreach (Vector2Int pos in camino) {
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), caminoRojo);
            yield return new WaitForSeconds(0.5f); 
        }
        if (!hayGanador) {
            hayGanador = true;
            rojoWin.SetActive(true);
        }
    }

    IEnumerator PintarCaminoGradualAzul(List<Vector2Int> camino) {
        foreach (Vector2Int pos in camino) {
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), caminoAzul);
            yield return new WaitForSeconds(0.5f); 
        }
        if (!hayGanador) {
            hayGanador = true;
            azulWin.SetActive(true); 
        }
    }
    Vector2Int EncontrarPosicionDeTile(int tileCode) {
        for (int i = 0; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                if (mapData[i, j] == tileCode) {
                    return new Vector2Int(i, j);
                }
            }
        }
        return Vector2Int.zero;
    }
}
