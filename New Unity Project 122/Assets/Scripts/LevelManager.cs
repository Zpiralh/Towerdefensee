using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Prefab yhdelle tilelle
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

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
	void Update () {
		
	}
    // Tekee levelin
    private void CreateLevel()
    {

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
                maxTile = PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
    }

    private Vector3 PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        // Tekee uuden tilen ja viittaa siihen newTile muuttujassa
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);

        // Käyttää newTile muuttujaa muuttaakseen tilen sijaintia
        newTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);
        return newTile.transform.position;
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Levell.txt") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }
}
