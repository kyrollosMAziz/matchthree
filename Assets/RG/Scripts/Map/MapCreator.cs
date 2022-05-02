using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType {
    Cube = 0,
    sphere = 1,
    cylinder = 2,
    shape = 3,
}

public class MapCreator : MonoBehaviour
{
    private Tile[,] m_GameMap = new Tile[10,10];
    private int numberOfColumns = 10;
    public int NumberOfColumns {
        get => numberOfColumns;
        set => numberOfColumns = value;
    }
    private int numberOfRows = 10;
    public int NumberOfRows {
        get => numberOfRows;
        set => numberOfRows = value;
    }

    [SerializeField]
    private TileData[] tilesData;
    [SerializeField]
    private GameObject m_TilePrefab;
    [SerializeField]
    private Transform m_MapHolder;

    public float distanceBetweenTiles;

    public void CreateMap() {
        m_GameMap = new Tile[NumberOfRows, numberOfColumns];
        for (int rows = 0; rows < numberOfRows; rows++) {
            for (int cols = 0; cols < numberOfColumns; cols++) {
                CreateTile(rows,cols);
            }
        }
    }
    private void CreateTile(int rowIndex, int colIndex) {
        GameObject tileGo = Instantiate(m_TilePrefab, m_MapHolder);
        Vector2 tilePos = tileGo.transform.position;
        tilePos += new Vector2(colIndex, rowIndex);
        tileGo.transform.position = tilePos;
        Tile tile = tileGo.GetComponent<Tile>();
        tile.name = "Tile"+rowIndex+""+colIndex ;
        SetTileData(rowIndex, colIndex, tile);
        SetTilePosition(rowIndex,colIndex,tile);
    }
    private void SetTilePosition(int rowIndex, int colIndex , Tile tile) {
        m_GameMap[rowIndex, colIndex] = tile;
    }
    private void SetTileData(int rowIndex, int colIndex,Tile tile) {
        int tileDataIndex = GameData.Instance.ReadElementTypeData(rowIndex, colIndex);
        tile.TileData = tilesData[tileDataIndex];
        tile.rowIndex = rowIndex;
        tile.colIndex = colIndex;
        tile.elementType = tile.TileData.Type;
        tile.SetElementContainerValue();
    }
    public Tile[,] GetGameMap() {
        return m_GameMap;
    }
}