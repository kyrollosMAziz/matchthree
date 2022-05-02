using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData TileData;
    public ElementType elementType;
    public Transform elementPos;

    public int rowIndex;
    public int colIndex;
    
    [SerializeField]
    private ElementContainer m_ElementContainer;
    public ElementContainer ElementContainer {
        get => m_ElementContainer;
        set => m_ElementContainer = value;
    }
    public void SetElementContainerValue() {
        
        if (TileData.tilePresenter != null) {
            m_ElementContainer.SetElementActive(TileData.tilePresenter);
        }
    }
}