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

    private void CreateGrid()
    {
        ClearGrid();

        if (map.Size.x <= 0 || map.Size.y <= 0)
        {
            Debug.Log("Map has a size of 0.");
            return;
        }

        var halfWidth = (map.Size.x - 1) / 2.0f;
        var halfHeight = (map.Size.y - 1) / 2.0f;

        int x = 0;
        int y = 0;

        for (float zPosition = 0 - halfHeight; zPosition <= halfHeight; zPosition += emptyCellPrefab.transform.localScale.z)
        {
            x = 0;

            for (float xPosition = 0 - halfWidth; xPosition <= halfWidth; xPosition += emptyCellPrefab.transform.localScale.x)
            {
                var cell = Instantiate(emptyCellPrefab, new Vector3(xPosition, 0, zPosition), emptyCellPrefab.transform.rotation, gridTransform);
                cell.name = $"Cell {x},{y}";

                x++;
            }

            y++;
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
