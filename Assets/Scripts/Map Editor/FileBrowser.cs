using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileBrowser : MonoBehaviour
{
    [SerializeField] GameObject fileOptionPrefab;
    [SerializeField] Transform contentTransform;

    public bool IsOpen { get; private set; }
        
    private readonly List<FileOption> fileOptions = new List<FileOption>();
    private bool shouldCloseOnNextFrame;
    private float closeTime;

    public event EventHandler<FileBrowserClosedEventArgs> Closed;

    public void Show(string searchPattern)
    {
        gameObject.SetActive(true);
        IsOpen = true;
                
        PopulateList(searchPattern);
    }

    private void PopulateList(string searchPattern)
    {
        fileOptions.Clear();
        for (int i = contentTransform.childCount - 1; i >= 0; i--)
        {
            var child = contentTransform.GetChild(i);
            Destroy(child.gameObject);
        }

        var filePaths = Directory.GetFiles(Application.streamingAssetsPath, searchPattern);
        var fileNames = filePaths.Select(f => Path.GetFileName(f));

        foreach (var fileName in fileNames)
        {
            var fileOption = Instantiate(fileOptionPrefab, contentTransform).GetComponent<FileOption>();
            fileOption.FileName = fileName;
            fileOptions.Add(fileOption);
        }
    }

    public void OnOKButtonClicked()
    {
        List<string> selectedFileNames = new List<string>(fileOptions.Where(f => f.IsSelected).Select(f => f.FileName));
        
        if (Closed != null)
        {
            Closed(this, new FileBrowserClosedEventArgs(false, selectedFileNames));
        }

        Hide();
    }

    public void Hide()
    {
        // Because we don't want any other mouseup events to fire during the frame when one of the buttons was clicked,
        // keep the file browser open (which makes the editor ignore mouse events) until the next frame, when the 
        // mouseup event from the button no longer exists.
        shouldCloseOnNextFrame = true;
        closeTime = Time.time;
    }

    public void OnCancelButtonClicked()
    {
        if (Closed != null)
        {
            Closed(this, new FileBrowserClosedEventArgs(true));
        }

        Hide();
    }

    private void Update()
    {
        if (shouldCloseOnNextFrame && closeTime < Time.time)
        {
            // Actually close the dialog now.
            IsOpen = false;
            shouldCloseOnNextFrame = false;
            gameObject.SetActive(false);
        }
    }
}
