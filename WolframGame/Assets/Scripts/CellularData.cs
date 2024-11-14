using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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
            PlaceTileRandomly(mapData, allPositions, 6); 
            PlaceTileRandomly(mapData, allPositions, 7); 
            PlaceTileRandomly(mapData, allPositions, 8); 
            PlaceTileRandomly(mapData, allPositions, 9); 
        }
        else {
            Debug.Log("No hay suficientes posiciones para colocar los tiles especiales.");
        }
    }

    void PlaceTileRandomly(int[,] mapData, List<Vector2Int> allPositions, int tileValue) {
        int randomIndex = Random.Range(0, allPositions.Count);
        Vector2Int position = allPositions[randomIndex];
        mapData[position.x, position.y] = tileValue;
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
    //Se obtiene un peso 
    public int ObtenerPesoTile(int tipoTile) {
        switch (tipoTile) {
            case 1: return 1;
            case 2: return 2; 
            case 3: return 3; 
            case 4: return 4; 
            case 5: return 1; 
            case 6: return 1; 
            case 7: return 1; 
            case 8: return 1; 
            case 9: return 1; 
            default: return int.MaxValue; 
        }
    }

    // Método A* para calcular el camino
    public List<Vector2Int> CalcularCaminoAStar(int[,] mapa, Vector2Int inicio, Vector2Int objetivo) {
        List<Nodo> abiertos = new List<Nodo>();
        HashSet<Vector2Int> cerrados = new HashSet<Vector2Int>();
        Nodo nodoInicial = new Nodo(inicio, null, 0, CalcularHeuristica(inicio, objetivo));
        abiertos.Add(nodoInicial);

        while (abiertos.Count > 0) {
            Nodo actual = ObtenerNodoConMenorF(abiertos);
            if (actual.posicion == objetivo) {
                return ReconstruirCamino(actual);
            }

            abiertos.Remove(actual);
            cerrados.Add(actual.posicion);

            foreach (Vector2Int vecino in ObtenerVecinos(actual.posicion, mapa)) {
                if (cerrados.Contains(vecino)) continue;

                int pesoTile = ObtenerPesoTile(mapa[vecino.x, vecino.y]);
                if (pesoTile == int.MaxValue) continue;

                int gNuevo = actual.g + pesoTile;

                Nodo vecinoNodo = abiertos.Find(n => n.posicion == vecino);
                if (vecinoNodo == null) {
                    int h = CalcularHeuristica(vecino, objetivo);
                    abiertos.Add(new Nodo(vecino, actual, gNuevo, h));
                }
                else if (gNuevo < vecinoNodo.g) {
                    vecinoNodo.g = gNuevo;
                    vecinoNodo.padre = actual;
                }
            }
        }
        return null; 
    }

    int CalcularHeuristica(Vector2Int pos, Vector2Int objetivo) {
        return Mathf.Abs(pos.x - objetivo.x) + Mathf.Abs(pos.y - objetivo.y);
    }

    Nodo ObtenerNodoConMenorF(List<Nodo> nodos) {
        return nodos.OrderBy(n => n.F).First();
    }

    List<Vector2Int> ObtenerVecinos(Vector2Int pos, int[,] mapa) {
        List<Vector2Int> vecinos = new List<Vector2Int>();
        Vector2Int[] direcciones = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1)
        };

        foreach (Vector2Int dir in direcciones) {
            Vector2Int vecino = pos + dir;
            if (vecino.x >= 0 && vecino.x < mapa.GetLength(0) &&
                vecino.y >= 0 && vecino.y < mapa.GetLength(1)) {
                vecinos.Add(vecino);
            }
        }
        return vecinos;
    }

    List<Vector2Int> ReconstruirCamino(Nodo nodo) {
        List<Vector2Int> camino = new List<Vector2Int>();
        while (nodo != null) {
            camino.Add(nodo.posicion);
            nodo = nodo.padre;
        }
        camino.Reverse();
        return camino;
    }
}
