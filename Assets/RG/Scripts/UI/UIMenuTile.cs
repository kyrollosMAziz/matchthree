using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuTile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public ElementType tileType;

    private TMP_Dropdown dropdown;
    
    void Start() {
        tileType = (ElementType)GameData.Instance.ReadElementTypeData(xIndex,yIndex);

        dropdown = GetComponent<TMP_Dropdown>();
        ElementType typeValue = tileType;
        dropdown.value = (int)typeValue;

        //if not null, add listener for when the value of the Dropdown changes, to take action
        dropdown?.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }
    /// <summary>
    ///Save the value chosen from drop down list and taking both, row, and coloumn indexes as refrence also saved by their values in playerprefs 
    /// </summary>
    /// <param name="change"></param>
    void DropdownValueChanged(TMP_Dropdown change) {
        int typeValue = (int)dropdown.value;
        GameData.Instance.SaveData(xIndex, yIndex, typeValue);
    }
    public void SetMyMapIndex(int rowIndex,int colIndex)  {
        xIndex = rowIndex;
        yIndex = colIndex;
    }
    public Vector2 GetMyMapIndex() {
        return new Vector2(xIndex,yIndex);
    }
    public ElementType GetElementType() {
        return tileType;
    }
}