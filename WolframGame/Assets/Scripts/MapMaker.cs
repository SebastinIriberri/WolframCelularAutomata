using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMaker : MonoBehaviour{

    public Tile tierra;
    public Tile agua;
    public Tile piedra;
    public Tile lava;
    public Tile nieve;
    public Tile baseRoja;
    public Tile inicioJugadorRojo;
    public Tile baseAzul;
    public Tile inicioJugadorAzul;
    public Tilemap tilemap;

    public int mapWidth;
    public int mapHeight;

    private int[,] mapData;
    public CellularData cell;

    void Start() {
        mapData = cell.GenerateData(mapWidth,mapHeight);
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
}
