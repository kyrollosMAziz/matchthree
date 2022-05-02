using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Tile" , menuName ="Create Tile")]
public class TileData : ScriptableObject
{
    public ElementType Type;
    public GameObject tilePresenter;
}