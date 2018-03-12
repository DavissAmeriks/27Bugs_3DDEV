// Lai izveidotu līmeni
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{// Masīvs ar tilePrefabs, tos izmantos izveidojot tiles spēlē
    [SerializeField]
    private GameObject[] tilePrefabs; // Lai izveidotu Tile
    [SerializeField]
    private CameraMovement cameraMovement;

    public Dictionary<Point, TileScript> Tiles { get; set; }
    // Lai atgrieztu tile size
    public float TileSize
    {
        get {return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

	// Use this for initialization
	void Start ()
    {
        CreateLevel(); // Izpilda create level funkciju
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    private void CreateLevel() // Funkcija Lai izveidotu līmeni
    {

        Tiles = new Dictionary<Point, TileScript>();
        string[] mapData = ReadLevelText();// tmp priekš tile map
        int mapX = mapData[0].ToCharArray().Length;// Aprēķina x map size
        int mapY = mapData.Length; // Aprēķina y map size

        Vector3 maxTile = Vector3.zero;
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));//Aprēķina kartes sākuma punktu, top left corner
        //float tileSize = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x; //cik lielas ir tiles, lai pareizi pozicionētu tiles

        for (int y = 0; y < mapY; y++)// y pozicijai
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)// x pozicijai
            {
                PlaceTile(newTiles[x].ToString(),x,y,worldStart);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>(); //Izveido tile,Reference izveidotajam Tile
        // lai mainītu tile poziciju
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0));

        Tiles.Add(new Point(x, y), newTile);

       // return newTile.transform.position;
    }

    private string [] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

}
