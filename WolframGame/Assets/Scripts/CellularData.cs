using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularData : MonoBehaviour{

    public float fillPercent = 0.5f;
    public int iterations = 1;

    public int[,] GenerateData(int w, int h) {
        int[,] mapData = new int[w, h];

        for (int i = 0; i < w; i++) {
            for (int j = 0; j < h; j++) {
                float chance = Random.Range(0f, 1f);

                if (chance < fillPercent) {
                    mapData[i, j] = 1; // Tierra
                }
                else if (chance < fillPercent + 0.15f) {
                    mapData[i, j] = 2; // Agua
                }
                else if (chance < fillPercent + 0.3f) {
                    mapData[i, j] = 3; // Piedra
                }
                else if (chance < fillPercent + 0.45f) {
                    mapData[i, j] = 4; // Lava
                }
                else {
                    mapData[i, j] = 5; // Nieve
                }
            }
        }

        int[,] buffer = new int[w, h];

        for (int x = 0; x < iterations; x++) {
            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {
                    if (i == 0 || i == w - 1 || j == 0 || j == h - 1) {
                        buffer[i, j] = 1;
                        continue;
                    }

                    int countTierra = 0, countAgua = 0, countPiedra = 0, countLava = 0, countNieve = 0;

                    for (int dx = -1; dx <= 1; dx++) {
                        for (int dy = -1; dy <= 1; dy++) {
                            if (dx == 0 && dy == 0) continue;

                            int neighbor = mapData[i + dx, j + dy];
                            switch (neighbor) {
                                case 1: countTierra++; break;
                                case 2: countAgua++; break;
                                case 3: countPiedra++; break;
                                case 4: countLava++; break;
                                case 5: countNieve++; break;
                            }
                        }
                    }

                    if (countTierra >= countAgua && countTierra >= countPiedra && countTierra >= countLava && countTierra >= countNieve) {
                        buffer[i, j] = 1;
                    }
                    else if (countAgua >= countTierra && countAgua >= countPiedra && countAgua >= countLava && countAgua >= countNieve) {
                        buffer[i, j] = 2;
                    }
                    else if (countPiedra >= countTierra && countPiedra >= countAgua && countPiedra >= countLava && countPiedra >= countNieve) {
                        buffer[i, j] = 3;
                    }
                    else if (countLava >= countTierra && countLava >= countAgua && countLava >= countPiedra && countLava >= countNieve) {
                        buffer[i, j] = 4;
                    }
                    else {
                        buffer[i, j] = 5;
                    }
                }
            }

            for (int i = 0; i < w; i++) {
                for (int j = 0; j < h; j++) {
                    mapData[i, j] = buffer[i, j];
                }
            }
        }
        PlaceSpecialTiles(mapData, w, h);
        return mapData;
    }

    void PlaceSpecialTiles(int[,] mapData, int w, int h) {
        List<Vector2Int> allPositions = GetAllPositions(w, h);

        if (allPositions.Count >= 4) {
            PlaceTileRandomly(mapData, allPositions, 6); // Base roja
            PlaceTileRandomly(mapData, allPositions, 7); // Inicio jugador rojo
            PlaceTileRandomly(mapData, allPositions, 8); // Base azul
            PlaceTileRandomly(mapData, allPositions, 9); // Inicio jugador azul
        }
        else {
            Debug.LogWarning("No hay suficientes posiciones para colocar los tiles especiales.");
        }
    }

    void PlaceTileRandomly(int[,] mapData, List<Vector2Int> allPositions, int tileValue) {
        // Selecciona una posición al azar y coloca el tile especial
        int randomIndex = Random.Range(0, allPositions.Count);
        Vector2Int position = allPositions[randomIndex];

        // Establece el valor del tile especial en la posición seleccionada
        mapData[position.x, position.y] = tileValue;

        // Elimina la posición de la lista para evitar duplicados
        allPositions.RemoveAt(randomIndex);
    }

    List<Vector2Int> GetAllPositions(int w, int h) {
        List<Vector2Int> allPositions = new List<Vector2Int>();
        for (int i = 0; i < w; i++) {
            for (int j = 0; j < h; j++) {
                allPositions.Add(new Vector2Int(i, j));
            }
        }
        return allPositions;
    }
}
