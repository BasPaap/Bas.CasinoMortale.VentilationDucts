using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{    
    private Camera editorCamera;
    private Camera mainCamera;

    private void Awake()
    {
        editorCamera = GetComponentInChildren<Camera>();
        mainCamera = Camera.main; // This needs to be stored because once disabled we can't access it via Camera.main.
        editorCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(Hotkeys.EditorKey))
        {
            editorCamera.gameObject.SetActive(!editorCamera.gameObject.activeSelf);
            mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
        }
    }
}
