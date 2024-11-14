using UnityEngine;

public class Nodo {
    public Vector2Int posicion;
    public Nodo padre;
    public int g; // Costo desde el inicio hasta este nodo
    public int h; // Heurística: estimación de la distancia al destino

    //Costo Total
    public int F {
        get { return g + h; } 
    }

    public Nodo(Vector2Int posicion, Nodo padre, int g, int h) {
        this.posicion = posicion;
        this.padre = padre;
        this.g = g;
        this.h = h;
    }
}