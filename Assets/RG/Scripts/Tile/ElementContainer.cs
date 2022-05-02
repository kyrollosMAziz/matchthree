using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Element Container instantiates prefabed object and handles it's movement in the scene
/// </summary>

public class ElementContainer : MonoBehaviour
{
    public Transform target;
    private GameObject element;

    public Transform parent;
    public bool isMove;
    public float speedMultiplier = 0.1f;

    private void Awake() {
        parent = transform.parent;
    }
    public void SetElementActive(GameObject tilePresenter) {
        element = (GameObject)Instantiate(tilePresenter);
        Transform goTransform = element.transform;
        goTransform.parent = transform;
        goTransform.localPosition = Vector3.zero;
    }
    public void Update() {
        if (isMove && target && Vector3.Distance(transform.position, target.position) < 0.001f) {
            isMove = false;
        }
        if (isMove) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, target.localPosition.y, transform.localPosition.z), speedMultiplier * Time.deltaTime);
        }
    }
    public void updateElement(Transform newParent, Transform newTarget) {
        parent = newParent;

        if (parent && transform.parent != parent) {
            transform.parent = parent;
            target = newTarget;
            isMove = true;
        }
    }
}