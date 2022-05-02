using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager uIManagerInstance;
    public static UIManager UIManagerInstance {
        get => uIManagerInstance; 
    }
    
    private CanvasGroup canvasGroup;
    public bool isShowMenuForEditing;//Set this to true in inspector to edit the map in game UI

    private void Awake() {
        uIManagerInstance = this;
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /// <summary>
    ///Hide 2d Menu when 3d map is created
    /// </summary>
    public void OnCreateMap() {
        gameObject.SetActive(false);
    }
}