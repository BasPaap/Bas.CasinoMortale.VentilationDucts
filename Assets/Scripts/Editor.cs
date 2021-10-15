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

    [SerializeField] private Transform gridTransform;
    [SerializeField] private GameObject emptyCellPrefab;

    private void Awake()
    {
        map = GameObject.FindObjectOfType<Map>();
        editorCamera = GetComponentInChildren<Camera>();
        mainCamera = Camera.main; // This needs to be stored because once disabled we can't access it via Camera.main.
        editorCamera.gameObject.SetActive(false);
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
    }

    private void Close()
    {
        isOpen = false;
        ToggleCameras();        
    }

    private void Open()
    {
        isOpen = true;
        ToggleCameras();

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

        for (int y = 0; y < map.Size.y; y++)
        {
            for (int x = 0; x < map.Size.x; x++)
            {
                var cell = Instantiate(emptyCellPrefab, map.GetPosition(x,y), emptyCellPrefab.transform.rotation, gridTransform);
                cell.name = $"Cell {x},{y}";
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = gridTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(gridTransform.GetChild(i).gameObject);
        }
    }

    private void ToggleCameras()
    {
        editorCamera.gameObject.SetActive(!editorCamera.gameObject.activeSelf);
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
    }    
}
