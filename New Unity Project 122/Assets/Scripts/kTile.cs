using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class kTile : Tile
{

    public Sprite[] sprites;
    public Sprite s_full;
    public Sprite s_placeholder;
    public Sprite s_debug;
    
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        tilemap.RefreshTile(position);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                    if (IsNeighbour(location, tilemap))
                        tilemap.RefreshTile(location);
                }
            }
        }

    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        int neighbor = 0;
        bool[] neighbors = { false, false, false, false, false, false, false, false };

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    Vector3Int pos = new Vector3Int(position.x + x, position.y + y, position.z);
                    neighbors[neighbor] = IsNeighbour(pos, tilemap);
                    neighbor++;
                }
            }
        }

        tileData.sprite = s_debug;

        if (sprites.Length <= 45)
            return;

        int immediateNeighbors = TrueCount(neighbors[1], neighbors[3], neighbors[4], neighbors[6]);
        switch (immediateNeighbors)
        {
            case 0:
                tileData.sprite = sprites[0];
                break;
            case 1:
                if (neighbors[1])
                    tileData.sprite = sprites[1];
                else if (neighbors[3])
                    tileData.sprite = sprites[4];
                else if (neighbors[4])
                    tileData.sprite = sprites[33];
                else if (neighbors[6])
                    tileData.sprite = sprites[12];
                break;
            case 2:
                if (neighbors[1] && neighbors[6])
                    tileData.sprite = sprites[13];
                else if (neighbors[3] && neighbors[4])
                    tileData.sprite = sprites[36];
                else if (neighbors[1] && neighbors[3])
                    if (neighbors[0])
                        tileData.sprite = sprites[3];
                    else
                        tileData.sprite = sprites[5];
                else if (neighbors[1] && neighbors[4])
                    if (neighbors[2])
                        tileData.sprite = sprites[17];
                    else
                        tileData.sprite = sprites[34];
                else if (neighbors[6] && neighbors[3])
                    if (neighbors[5])
                        tileData.sprite = sprites[10];
                    else
                        tileData.sprite = sprites[15];
                else if (neighbors[6] && neighbors[4])
                    if (neighbors[7])
                        tileData.sprite = sprites[28];
                    else
                        tileData.sprite = sprites[41];
                break;
            case 3:
                if (!neighbors[1])
                    if (neighbors[5] && neighbors[7])
                        tileData.sprite = sprites[26];
                    else if (neighbors[5])
                        tileData.sprite = sprites[39];
                    else if (neighbors[7])
                        tileData.sprite = sprites[31];
                    else
                        tileData.sprite = sprites[44];
                else if (!neighbors[3])
                    if (neighbors[2] && neighbors[7])
                        tileData.sprite = sprites[6];
                    else if (neighbors[2])
                        tileData.sprite = sprites[22];
                    else if (neighbors[7])
                        tileData.sprite = sprites[29];
                    else
                        tileData.sprite = sprites[42];
                else if (!neighbors[4])
                    if (neighbors[0] && neighbors[5])
                        tileData.sprite = sprites[9];
                    else if (neighbors[0])
                        tileData.sprite = sprites[14];
                    else if (neighbors[5])
                        tileData.sprite = sprites[11];
                    else
                        tileData.sprite = sprites[16];
                else if (!neighbors[6])
                    if (neighbors[0] && neighbors[2])
                        tileData.sprite = sprites[18];
                    else if (neighbors[0])
                        tileData.sprite = sprites[35];
                    else if (neighbors[2])
                        tileData.sprite = sprites[19];
                    else
                        tileData.sprite = sprites[37];
                break;
            case 4:
                int diagonalNeighbors = TrueCount(neighbors[0], neighbors[2], neighbors[5], neighbors[7]);
                
                switch (diagonalNeighbors)
                {
                    case 0:
                        tileData.sprite = sprites[45];
                        break;
                    case 1:
                        if (neighbors[0])
                            tileData.sprite = sprites[43];
                        else if (neighbors[2])
                            tileData.sprite = sprites[24];
                        else if (neighbors[5])
                            tileData.sprite = sprites[40];
                        else if (neighbors[7])
                            tileData.sprite = sprites[32];
                        break;
                    case 2:
                        if (neighbors[0] && neighbors[7])
                            tileData.sprite = sprites[30];
                        else if (neighbors[2] && neighbors[5])
                            tileData.sprite = sprites[21];
                        else if (neighbors[0] && neighbors[5])
                            tileData.sprite = sprites[38];
                        else if (neighbors[0] && neighbors[2])
                            tileData.sprite = sprites[23];
                        else if (neighbors[2] && neighbors[7])
                            tileData.sprite = sprites[8];
                        else if (neighbors[7] && neighbors[5])
                            tileData.sprite = sprites[27];
                        break;
                    case 3:
                        if (!neighbors[0])
                            tileData.sprite = sprites[2];
                        else if (!neighbors[2])
                            tileData.sprite = sprites[25];
                        else if (!neighbors[5])
                            tileData.sprite = sprites[7];
                        else if (!neighbors[7])
                            tileData.sprite = sprites[20];
                        break;
                    case 4:
                        tileData.sprite = s_full;
                        break;
                }
                break;
        }

    }

    private bool IsNeighbour(Vector3Int position, ITilemap tilemap)
    {
        TileBase tile = tilemap.GetTile(position);
        return (tile != null && tile == this);
    }

    private int TrueCount(params bool[] arr)
    {
        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if(arr[i])
                count++;
        }
        return count;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/kTile")]
    public static void Create_kTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save kTile", "New kTile", "asset", "Save kTile", "Assets");
        if (path == "") { return; }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<kTile>(), path);
    }
#endif
}
