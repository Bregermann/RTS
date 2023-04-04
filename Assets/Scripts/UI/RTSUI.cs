using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTSUI : MonoBehaviour
{
    public Text selectedText; // Text object to display selected unit/building information
    public Button moveButton; // Button to initiate unit movement
    public RectTransform selectionBox; // Selection box object

    private Vector2 boxStartPosition; // The start position of the selection box
    private Vector2 boxEndPosition; // The end position of the selection box

    private List<Unit> selectedUnits = new List<Unit>(); // List of currently selected units
    private List<Building> selectedBuildings = new List<Building>(); // List of currently selected buildings

    // Update is called once per frame
    private void Update()
    {
        // Handle unit selection
        if (Input.GetMouseButtonDown(0))
        {
            // Reset selection
            ClearSelection();

            // Set box start position
            boxStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            // Update box end position
            boxEndPosition = Input.mousePosition;

            // Update selection box position and size
            selectionBox.gameObject.SetActive(true);
            selectionBox.position = (boxStartPosition + boxEndPosition) / 2;
            selectionBox.sizeDelta = new Vector2(Mathf.Abs(boxStartPosition.x - boxEndPosition.x), Mathf.Abs(boxStartPosition.y - boxEndPosition.y));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Clear selection box and reset positions
            selectionBox.gameObject.SetActive(false);
            boxStartPosition = Vector2.zero;
            boxEndPosition = Vector2.zero;

            // Raycast to select units and buildings
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null)
                {
                    SelectUnit(unit);
                }
                Building building = hit.collider.GetComponent<Building>();
                if (building != null)
                {
                    SelectBuilding(building);
                }
            }
        }

        // Handle unit movement
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (Unit unit in selectedUnits)
            {
                unit.MoveToPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

    // Select a unit
    private void SelectUnit(Unit unit)
    {
        selectedUnits.Add(unit);
        selectedText.text = "Selected Unit: " + unit.unitName;
        moveButton.gameObject.SetActive(true);
    }

    // Select a building
    private void SelectBuilding(Building building)
    {
        selectedBuildings.Add(building);
        selectedText.text = "Selected Building: " + building.buildingName;
        moveButton.gameObject.SetActive(false);
    }

    // Clear the current selection
    private void ClearSelection()
    {
        selectedUnits.Clear();
        selectedBuildings.Clear();
        selectedText.text = "";
        moveButton.gameObject.SetActive(false);
    }
}