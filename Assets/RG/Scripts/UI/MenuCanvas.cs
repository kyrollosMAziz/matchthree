using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    public int rows = 10, cols = 10;// map size : m x n (rows x columns)

    public GameObject UiElement;
    public GameObject inputFieldPrefab;
    /// <summary>
    ///Create 2d Menu on the screen
    /// </summary>
    private void Awake() {
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                GameObject element = (GameObject)Instantiate(UiElement, transform);
                var elementTile = element.GetComponent<UIMenuTile>();
                elementTile.SetMyMapIndex(row, col);
            }
        }
    }
    /// <summary>
    ///fit 2d Menu within the screen
    /// </summary>
    void Start(){
        RectTransform parentRect = gameObject.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(parentRect.rect.width / cols, parentRect.rect.height / rows);
    }
}