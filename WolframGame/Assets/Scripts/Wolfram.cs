using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CellularAutomatonTilemap : MonoBehaviour {
    public int sizeX;
    public int sizeY;
    public int rule;
    public bool randomStart;
    public bool stepped;
    public float stepTime = 0.1f; 

    public Button generateButton;
    public TMP_InputField inputX;
    public TMP_InputField inputY;
    public TMP_InputField inputRule;
    public Toggle toggleRandom;
    public Toggle toggleStepped;

    private bool[,] grid;
    private bool[] ruleSet = new bool[8]; 

    public Tilemap tilemap;
    public Tile activeTile; 
    public Tile inactiveTile;

    void Start() {
        generateButton.onClick.AddListener(GenerateAutomaton);
    }

    void GenerateAutomaton() {
        sizeX = int.Parse(inputX.text);
        sizeY = int.Parse(inputY.text);
        rule = int.Parse(inputRule.text);
        randomStart = toggleRandom.isOn;
        stepped = toggleStepped.isOn;

      
        grid = new bool[sizeY, sizeX];

       
        for (int i = 0; i < 8; i++) {
            ruleSet[i] = (rule & (1 << i)) != 0;
        }

       
        InitializeFirstGeneration();

        tilemap.ClearAllTiles();

       
        if (stepped) {
            StartCoroutine(StepSimulation());
        }
        else {
            RunSimulation();
        }
    }


    void InitializeFirstGeneration() {
        if (randomStart) {
            for (int i = 0; i < sizeX; i++) {
                grid[0, i] = Random.value > 0.5f;
            }
        }
        else {
            grid[0, sizeX / 2] = true; 
        }

      
        UpdateVisuals(0);
    }

    IEnumerator StepSimulation() {
        for (int y = 1; y < sizeY; y++) {
            GenerateNextGeneration(y);
            yield return new WaitForSeconds(stepTime);
        }
    }

    void RunSimulation() {
        for (int y = 1; y < sizeY; y++) {
            GenerateNextGeneration(y);
        }
    }

    void GenerateNextGeneration(int y) {
        for (int x = 0; x < sizeX; x++) {
            
            bool left = x == 0 ? grid[y - 1, sizeX - 1] : grid[y - 1, x - 1];
            bool center = grid[y - 1, x];
            bool right = x == sizeX - 1 ? grid[y - 1, 0] : grid[y - 1, x + 1];

            int index = (left ? 4 : 0) + (center ? 2 : 0) + (right ? 1 : 0);
            grid[y, x] = ruleSet[index];
        }

        
        UpdateVisuals(y);
    }

    void UpdateVisuals(int y) {
        for (int x = 0; x < sizeX; x++) {
            Vector3Int tilePosition = new Vector3Int(x, -y, 0); 

            if (grid[y, x]) {
                tilemap.SetTile(tilePosition, activeTile); 
            }
            else {
                tilemap.SetTile(tilePosition, inactiveTile);
            }
        }
    }
}

/*
 numero = 5
 for(int i = numero ; numero <= 0 ; numero --){
    numero % 2 ; 
    if(numeor = 1){
        impar 
        numrto add listainp 
    }else{
       impar 
        numrto add listainpares  
    }
} 

imprmier listas pares 

 * */