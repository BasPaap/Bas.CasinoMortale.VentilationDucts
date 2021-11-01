
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class FileBrowserClosedEventArgs : EventArgs
{
    public Collection<string> SelectedFileNames { get; private set; } = new Collection<string>();
    public bool IsCanceled { get; private set; }
    public FileBrowserClosedEventArgs(bool isCanceled, IEnumerable<string> selectedFileNames)
        : this(isCanceled)
    {
        foreach (var selectedFileName in selectedFileNames)
        {
            SelectedFileNames.Add(selectedFileName);
        }
    }

    public FileBrowserClosedEventArgs(bool isCanceled)
    {
        IsCanceled = isCanceled;        
    }
}

