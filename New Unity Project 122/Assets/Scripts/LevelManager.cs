using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    // Prefab yhdelle tilelle
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform map;

    private Point blueSpawn, redSpawn;

    [SerializeField]
    private GameObject blueEndPrefab;

    [SerializeField]
    private GameObject redStartPrefab;

    public Dictionary<Point, TileScript> Tiles { get; set; }

    // Palauttaa tilen koon 
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

	// Use this for initialization
	void Start ()
    {
        CreateLevel();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    // Tekee levelin
    private void CreateLevel()
    {

        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        //laskee kentän x koon
        int mapX = mapData[0].ToCharArray().Length;
        //laskee kentän y koon
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++) // Y:n sijainti
        {

            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++) // X:n sijainti
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPoints();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        // Tekee uuden tilen ja viittaa siihen newTile muuttujassa
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        // Käyttää newTile muuttujaa muuttaakseen tilen sijaintia
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);

       

    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Levell.txt") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    //Käytetään "Tiles" dictionaryy jotta saadaan spawnpointti ja tarkka tilen sijainti ja laitetaan "redStart" siihen
    private void SpawnPoints()
    {
        redSpawn = new Point(0, 0);

        Instantiate(redStartPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        blueSpawn = new Point(13, 3);

        Instantiate(blueEndPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
}
