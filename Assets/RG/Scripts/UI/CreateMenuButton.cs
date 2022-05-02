using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenuButton : MonoBehaviour
{
    private SceneController sceneController;
    private Button button;

    private void Start() {
        sceneController = GameManager.SceneController;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCreateMapClicked);
    }
    private void OnCreateMapClicked() {
        sceneController.DrawGameMap();
    }
}