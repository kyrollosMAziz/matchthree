using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float timeToAllowPlayerToPlay = 0.5f;

    private SceneController sceneController;
    private bool isAllowPlayerToClick = true;

    private void Start()
    {
        sceneController = GameManager.SceneController;
    }

    void Update()
    {
        if (isAllowPlayerToClick && Input.GetKeyUp(KeyCode.Mouse0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)) {
                if (hit.collider) {
                    Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                    if (tile != null && tile.ElementContainer) {
                        OnTileClick(tile);
                    }
                }
            }
        }
    }
    void OnTileClick(Tile tile)
    {
        isAllowPlayerToClick = false;
        sceneController.OnElementClick(tile.rowIndex, tile.colIndex);
        if (!isAllowPlayerToClick) {
            StartCoroutine(AllowPlayerToClick());
        }
    }
    IEnumerator AllowPlayerToClick()
    {
        yield return new WaitForSeconds(timeToAllowPlayerToPlay);
        isAllowPlayerToClick = !isAllowPlayerToClick;
    }
}