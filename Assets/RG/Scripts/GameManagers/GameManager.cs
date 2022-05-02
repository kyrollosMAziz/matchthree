using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { 
        get => instance; 
    }

    private static SceneController sceneController;
    public static SceneController SceneController { 
        get => sceneController; 
    }

    public void Awake() {
        instance = this;
        sceneController = GetComponent<SceneController>();
    }
}