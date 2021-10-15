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
    }

    private void ToggleCameras()
    {
        editorCamera.gameObject.SetActive(!editorCamera.gameObject.activeSelf);
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
    }    
}
