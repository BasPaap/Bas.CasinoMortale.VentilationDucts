using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{    
    private Camera editorCamera;
    private Camera mainCamera;
    private Map map;
    private bool isOpen;
    private GameObject selectedTool;
    private Quaternion originalSelectedToolRotation;
    private float selectedToolYRotation;

    [SerializeField] private Transform gridTransform;
    [SerializeField] private GameObject emptyCellPrefab;
    [SerializeField] private Canvas toolbarCanvas;
    
    private void Awake()
    {
        map = GameObject.FindObjectOfType<Map>();
        editorCamera = GetComponentInChildren<Camera>();
        mainCamera = Camera.main; // This needs to be stored because once disabled we can't access it via Camera.main.
        editorCamera.gameObject.SetActive(false);
        toolbarCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(Hotkeys.EditorKey))
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            selectedToolYRotation = (selectedToolYRotation + ((0 - Input.mouseScrollDelta.y) * 90.0f)) % 360.0f;
            selectedTool.transform.rotation = Quaternion.AngleAxis(selectedToolYRotation, Vector3.up) * originalSelectedToolRotation;
        }
    }

    private void Close()
    {
        isOpen = false;
        ToggleCameras();
        ToggleToolbar();
    }

    private void ToggleToolbar()
    {
        toolbarCanvas.gameObject.SetActive(!toolbarCanvas.gameObject.activeSelf);
    }

    private void Open()
    {
        isOpen = true;
        ToggleCameras();
        ToggleToolbar();

        // Create backup of current map
        map.CreateBackup();

        CreateGrid();
    }

    /// <summary>
    /// Creates the grid of tiles that serve as helpers when editing the map.
    /// </summary>
    private void CreateGrid()
    {
        ClearGrid();

        if (map.Size.x <= 0 || map.Size.y <= 0)
        {
            Debug.Log("Map has a size of 0.");
            return;
        }

        for (int row = 0; row < map.Size.y; row++)
        {
            for (int column = 0; column < map.Size.x; column++)
            {
                var cell = Instantiate(emptyCellPrefab, emptyCellPrefab.transform.position + map.GetPosition(column,row), emptyCellPrefab.transform.rotation, gridTransform).GetComponent<Cell>();
                cell.name = $"Cell {column},{row}";
                cell.MouseEnter += Cell_MouseEnter;
                cell.MouseExit += Cell_MouseExit;
            }
        }
    }

    private void Cell_MouseExit(object sender, EventArgs e)
    {
        if (sender is Cell cell && selectedTool != null)
        {
            selectedTool.SetActive(false);
        }
    }

    private void Cell_MouseEnter(object sender, EventArgs e)
    {
        if (sender is Cell cell && selectedTool != null)
        {
            selectedTool.transform.position = new Vector3(cell.transform.position.x, 0, cell.transform.position.z);
            selectedTool.SetActive(true);
        }
    }

    private void ClearGrid()
    {
        for (int i = gridTransform.childCount - 1; i >= 0; i--)
        {
            var child = gridTransform.GetChild(i);
            var cell = child.GetComponent<Cell>();
            if (cell != null)
            {
                cell.MouseEnter -= Cell_MouseEnter;
                cell.MouseExit -= Cell_MouseExit;
            }

            Destroy(child.gameObject);
        }
    }

    private void ToggleCameras()
    {
        editorCamera.gameObject.SetActive(!editorCamera.gameObject.activeSelf);
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
    }    

    public void OnDuctButtonClicked(string ductTypeName)
    {
        if (Enum.TryParse<DuctType>(ductTypeName, out DuctType ductType))
        {
            selectedToolYRotation = 0;
            var selectedToolPrefab = TileFactory.Instance.GetTilePrefabByType(ductType);
            originalSelectedToolRotation = selectedToolPrefab.transform.rotation;
            InstantiateTool(selectedToolPrefab);
        }
        else
        {
            Debug.LogWarning($"OnDuctButtonClicked called with invalid DuctType name \"{ductTypeName}\".");
        }
    }

    private void InstantiateTool(GameObject selectedToolPrefab)
    {
        selectedTool = Instantiate(selectedToolPrefab, selectedToolPrefab.transform.position, selectedToolPrefab.transform.rotation, transform);
        selectedTool.SetActive(false);
    }

    
}
