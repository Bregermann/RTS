using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask clickMask;
    public GameObject selectionBoxPrefab;

    private Camera mainCamera;
    private List<GameObject> selectedUnits = new List<GameObject>();
    private Vector3 selectionBoxStartPos;
    private GameObject selectionBox;
    private bool isDragging = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSelectionBox();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndSelectionBox();
            SelectUnits();
        }
        else if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox();
        }
    }

    private void StartSelectionBox()
    {
        selectionBoxStartPos = Input.mousePosition;
        selectionBox = Instantiate(selectionBoxPrefab, selectionBoxStartPos, Quaternion.identity);
        isDragging = true;
    }

    private void EndSelectionBox()
    {
        isDragging = false;
        Destroy(selectionBox);
    }

    private void UpdateSelectionBox()
    {
        if (!isDragging)
        {
            return;
        }

        Vector3 currentMousePos = Input.mousePosition;
        Vector3 lowerLeft = new Vector3(
            Mathf.Min(selectionBoxStartPos.x, currentMousePos.x),
            Mathf.Min(selectionBoxStartPos.y, currentMousePos.y),
            selectionBoxStartPos.z
        );
        Vector3 upperRight = new Vector3(
            Mathf.Max(selectionBoxStartPos.x, currentMousePos.x),
            Mathf.Max(selectionBoxStartPos.y, currentMousePos.y),
            selectionBoxStartPos.z
        );
        Vector3 selectionBoxCenter = (lowerLeft + upperRight) / 2f;
        selectionBox.transform.position = selectionBoxCenter;
        Vector3 selectionBoxSize = new Vector3(
            Mathf.Abs(upperRight.x - lowerLeft.x),
            Mathf.Abs(upperRight.y - lowerLeft.y),
            0f
        );
        selectionBox.transform.localScale = selectionBoxSize;
    }

    private void SelectUnits()
    {
        if (!isDragging)
        {
            return;
        }

        // Clear the previous selection
        foreach (GameObject unit in selectedUnits)
        {
            if (unit != null)
            {
                unit.GetComponent<UnitController>().Deselect();
            }
        }
        selectedUnits.Clear();

        // Select new units
        Vector2 lowerLeft = mainCamera.ScreenToWorldPoint(selectionBoxStartPos);
        Vector2 upperRight = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapAreaAll(lowerLeft, upperRight, clickMask);
        foreach (Collider2D hit in hits)
        {
            GameObject unit = hit.gameObject;
            if (unit.CompareTag("PlayerUnit"))
            {
                unit.GetComponent<UnitController>().Select();
                selectedUnits.Add(unit);
            }
        }
    }

    public List<GameObject> GetSelectedUnits()
    {
        return selectedUnits;
    }
}