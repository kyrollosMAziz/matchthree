using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instace;
    public static GameData Instance
    {
        get => instace;
    }

    private void Awake()
    {
        instace = GetComponent<GameData>();
    }
    /// <summary>
    ///Save tiles value and taking their row and coloumn index as refrences
    /// </summary>
    public void SaveData(int row, int col, int tileType)
    {
        string key = row.ToString() + col.ToString();
        PlayerPrefs.SetInt(key, tileType);
    }

    public int ReadElementTypeData(int x, int y)
    {
        if (PlayerPrefs.HasKey(x.ToString() + y.ToString())) {
            string key = x.ToString() + y.ToString();
            return PlayerPrefs.GetInt(key);
        }
        return (int)ElementType.sphere;
    }
}