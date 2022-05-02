using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public UIManager uiManager;
    public float timeToWaitBeforeRemovingTiles = 1;

    [SerializeField]
    private MapCreator mapCreator;
    private int highestTile;

    //Keep Track of Highest row index for every coloumn after removing elements from the map
    private struct MapPeakData {
        public int columnIndex;
        public int highestRowIndex;
    }
    private List<MapPeakData> peaksData = new List<MapPeakData>();

    private void Start() {
        uiManager = UIManager.UIManagerInstance;
        if (uiManager && !uiManager.isShowMenuForEditing) {
            DrawGameMap();
        }
        Tile[,] currentGameMap = mapCreator.GetGameMap();

        int mapHeight = currentGameMap.GetLength(0) - 1;
        highestTile = mapHeight;

        int mapWidth = currentGameMap.GetLength(1) - 1;//Set Highest row index for each coloumn at the start of the game
        for (int col = 0; col <= mapWidth; col++) {
            peaksData.Add(new MapPeakData {
                columnIndex = col,
                highestRowIndex = highestTile,
            });
        }
    }
    public void DrawGameMap() {
        mapCreator.CreateMap();
        uiManager.OnCreateMap();
    }
    public void OnElementClick(int clickedTilerowIndex, int clickedTileColIndex) {
        var currentGameMap = mapCreator.GetGameMap();
        int mapHeight = currentGameMap.GetLength(0) - 1;
        int mapWidth = currentGameMap.GetLength(1) - 1;
        highestTile = peaksData[clickedTileColIndex].highestRowIndex;

        Tile tile = currentGameMap[clickedTilerowIndex, clickedTileColIndex];
        if (tile) {
            ElementContainer elementContainer = tile.ElementContainer;
            elementContainer?.gameObject.SetActive(false);
        }
        if (tile.rowIndex == highestTile) {
            Collider tileCollider = tile.GetComponent<Collider>();
            if (tileCollider != null) {
                tileCollider.enabled = false;
            }
            Destroy(tile);
            return;
        }
        for (int row = clickedTilerowIndex + 1; row <= highestTile; row++) {
            PushElementsDown(currentGameMap, row, clickedTileColIndex, mapWidth, mapHeight);
        }
        StartCoroutine(RemoveSimilarElementsInMapRoutine(currentGameMap, clickedTilerowIndex, mapWidth, mapHeight));
    }
    private void PushElementsDown(Tile[,] currentGameMap, int row, int clickedTileColIndex, int mapWidth, int mapHeight)
    {
        Tile currentElement = currentGameMap[row - 1, clickedTileColIndex];
        ElementContainer currentElementContainer = currentElement.ElementContainer;

        Tile previousElement = currentGameMap[row, clickedTileColIndex];
        ElementContainer previousElementContainer = previousElement.ElementContainer;
        Transform target = currentElement.elementPos;

        previousElementContainer.updateElement(currentElement.transform, target);
        currentElement.ElementContainer = previousElement.ElementContainer;
        currentElement.elementType = previousElement.elementType;

        //update highestRowIndex in peaks list
        if (row == highestTile) {
            for (int col = 0; col <= clickedTileColIndex; col++) {
                if (clickedTileColIndex == peaksData[col].columnIndex) {
                    peaksData[col] = new MapPeakData {
                        columnIndex = col,
                        highestRowIndex = row - 1,
                    };
                    previousElement.ElementContainer = null;
                }
            }
        }
    }
    IEnumerator RemoveSimilarElementsInMapRoutine(Tile[,] currentGameMap, int clickedTilerowIndex, int mapWidth, int mapHeight)
    {
        yield return new WaitForSeconds(timeToWaitBeforeRemovingTiles);
        RemoveSimilarAdjacentElementsInMap(currentGameMap, clickedTilerowIndex, mapWidth, mapHeight);
    }
    private void RemoveSimilarAdjacentElementsInMap(Tile[,] currentGameMap, int clickedTilerowIndex, int mapWidth, int mapHeight)
    {
        ElementType elementType = ElementType.cylinder;

        List<Tile> similarTiles = new List<Tile>();
        List<List<Tile>> similarTilesList = new List<List<Tile>>();

        for (int row = clickedTilerowIndex; row <= mapHeight; row++) {
            similarTiles.Clear();
            bool isEmptyElement = false;

            for (int col = 0; col <= mapWidth - 1; col++) {
                Tile currentTile = currentGameMap[row, col];
                Tile nextTile = currentGameMap[row, col + 1];

                //Update Element Type If next, or current tile is empty
                isEmptyElement = CheckForEmptyTiles(currentTile, nextTile, similarTiles ,elementType);
                if (isEmptyElement) {
                    elementType = UpdateElementTypeOnEmptyTileFound(currentTile,nextTile, elementType);
                    continue;
                }

                ElementType currentTileType = currentTile.elementType;
                ElementType nextTileType = nextTile.elementType;

                if (col == 0) {
                    elementType = currentTileType;
                }

                if (currentTileType == nextTileType && currentTileType == elementType) {
                    AddToSimilarTilesList(currentTile,nextTile,similarTiles);
                }
                else if (similarTiles.Count <= 2) {
                    similarTiles.Clear();
                    elementType = nextTileType;
                }
                else {
                    if (similarTiles.Count > 2) {
                        List<Tile> tiles = new List<Tile>(similarTiles);
                        similarTilesList.Add(tiles);
                    }
                    similarTiles.Clear();
                    elementType = nextTileType;
                }
                if (col == mapWidth - 1 && similarTiles.Count > 2) {
                    List<Tile> tiles = new List<Tile>(similarTiles);
                    AddToListsOfSimilarTilesOnLastColoumn(tiles, similarTilesList,similarTiles);
                }
            }
        }
        if (similarTilesList.Count > 0) {
            DeactivateAdjacentElementsInSameRow(similarTilesList);
        }
    }
    private void AddToListsOfSimilarTilesOnLastColoumn(List<Tile> tiles, List<List<Tile>> similarTilesList, List<Tile> similarTiles)
    {
        if (!similarTilesList.Contains(similarTiles)) {
            similarTilesList.Add(tiles);
        }
    }
    private void AddToSimilarTilesList(Tile currentTile, Tile nextTile, List<Tile> similarTiles) 
    {
        if (!similarTiles.Contains(currentTile)) {
            similarTiles.Add(currentTile);
        }
        if (!similarTiles.Contains(nextTile)) {
            similarTiles.Add(nextTile);
        }
    }
    private bool CheckForEmptyTiles(Tile currentTile, Tile nextTile, List<Tile> similarTiles ,ElementType elementType) 
    {
        if (currentTile == null || nextTile == null) {
            if (similarTiles.Count <= 2) {
                similarTiles.Clear();
            }
            return true;
        }
        else {
            return false;
        }
    }
    private ElementType UpdateElementTypeOnEmptyTileFound(Tile currentTile, Tile nextTile, ElementType elementType) 
    {
        if (currentTile != null) {
            return currentTile.elementType;
        }
        if (nextTile != null) {
            return nextTile.elementType;
        }
        return elementType;
    }
    private void DeactivateAdjacentElementsInSameRow(List<List<Tile>> tilesToBeRemovedList)
    {
        List<Tile> tilesToBeRemoved = new List<Tile>();
        int numberOftilesListsToBeRemoved = tilesToBeRemovedList.Count;

        for (int listIndex = 0; listIndex < numberOftilesListsToBeRemoved; listIndex++) {
            tilesToBeRemoved = tilesToBeRemovedList[listIndex];
            int numberOftilesToBeRemoved = tilesToBeRemoved.Count;

            for (int i = 0; i < numberOftilesToBeRemoved; i++) {
                int tileRowIndex = tilesToBeRemoved[i].rowIndex;
                int tileColIndex = tilesToBeRemoved[i].colIndex;
                OnElementClick(tileRowIndex, tileColIndex);
            }
        }
    }
}