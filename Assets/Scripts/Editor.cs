using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Editor : MonoBehaviour
{
    private Camera editorCamera;
    private Camera mainCamera;
    private Map map;
    private bool isOpen;
    private GameObject tool;
    private TileData selectedTileData;
    private Quaternion originalToolRotation;
    private float toolYRotation;
    private readonly List<Cell> cells = new List<Cell>();

    [SerializeField] private Transform gridTransform;
    [SerializeField] private GameObject emptyCellPrefab;
    [SerializeField] private Canvas toolbarCanvas;
    [SerializeField] private FileBrowser fileBrowser;

    private void Awake()
    {
        map = GameObject.FindObjectOfType<Map>();
        editorCamera = GetComponentInChildren<Camera>();
        mainCamera = Camera.main; // This needs to be stored because once disabled we can't access it via Camera.main.
        editorCamera.gameObject.SetActive(false);
        toolbarCanvas.gameObject.SetActive(false);
        fileBrowser.gameObject.SetActive(false);        
    }

    private void Update()
    {
        if (isOpen)
        {
            if (tool != null)
            {
                tool.SetActive(false);
            }

            HandleMouseInput();
        }

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
        else if (Input.GetKeyUp(Hotkeys.ResetMapKey))
        {
            map.ResetMap();
        }
    }

    #region UI
    private void Open()
    {
        isOpen = true;
        ToggleCameras();
        ToggleToolbar();

        // Create backup of current map
        map.CreateBackup();

        CreateGrid();
    }

    private void Close()
    {
        isOpen = false;
        toolYRotation = 0;
        selectedTileData = null;
        if (tool != null)
        {
            Destroy(tool);
        }

        ToggleCameras();
        ToggleToolbar();
        ClearGrid();
    }

    private void HandleMouseInput()
    {
        if (!fileBrowser.IsOpen)
        {
            RaycastHit[] hits = Physics.RaycastAll(editorCamera.ScreenPointToRay(Input.mousePosition));
            var hitGameObjects = hits.Select(h => h.collider.gameObject);

            foreach (var cell in cells)
            {
                var isCellHit = hitGameObjects.Contains(cell.gameObject);

                cell.SetHighlight(isCellHit);

                if (isCellHit)
                {
                    MoveToolToCell(cell);

                    if (tool != null)
                    {
                        tool.SetActive(true);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        ApplyTool(cell);
                    }
                }
            }

            SetToolRotation();
        }
    }

    private void SetToolRotation()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            toolYRotation = (toolYRotation + ((0 - Input.mouseScrollDelta.y) * 90.0f)) % 360.0f;

            if (tool != null)
            {
                tool.transform.rotation = Quaternion.AngleAxis(toolYRotation, Vector3.up) * originalToolRotation;
            }

            if (selectedTileData != null)
            {
                selectedTileData.Rotation = toolYRotation;
            }
        }
    }

    private void MoveToolToCell(Cell cell)
    {
        if (tool != null)
        {
            tool.transform.position = new Vector3(cell.transform.position.x, 0, cell.transform.position.z);
        }

        if (selectedTileData != null)
        {
            selectedTileData.Column = cell.Column;
            selectedTileData.Row = cell.Row;
        }
    }

    private void InstantiateTool(GameObject selectedToolPrefab)
    {
        if (tool != null)
        {
            Destroy(tool);
        }

        toolYRotation = 0;
        originalToolRotation = selectedToolPrefab.transform.rotation;
        tool = Instantiate(selectedToolPrefab, selectedToolPrefab.transform.position, selectedToolPrefab.transform.rotation, transform);
        tool.name = $"Tool ({selectedToolPrefab.name})";
        tool.SetActive(false);
    }

    private void ToggleToolbar()
    {
        toolbarCanvas.gameObject.SetActive(!toolbarCanvas.gameObject.activeSelf);
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
                var cell = Instantiate(emptyCellPrefab, emptyCellPrefab.transform.position + map.GetPosition(column, row), emptyCellPrefab.transform.rotation, gridTransform).GetComponent<Cell>();
                cell.name = $"Cell {column},{row}";
                cell.Column = column;
                cell.Row = row;

                cells.Add(cell);
            }
        }
    }
        
    private void ClearGrid()
    {
        for (int i = gridTransform.childCount - 1; i >= 0; i--)
        {
            var child = gridTransform.GetChild(i);
            Destroy(child.gameObject);
        }

        cells.Clear();
    }

    private void ToggleCameras()
    {
        editorCamera.gameObject.SetActive(!editorCamera.gameObject.activeSelf);
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
    }
    #endregion

    private void FileBrowser_ClosedForSoundTile(object sender, FileBrowserClosedEventArgs e)
    {
        fileBrowser.Closed -= FileBrowser_ClosedForSoundTile;

        if (!e.IsCanceled)
        {
            var selectedToolPrefab = TileFactory.Instance.GetSoundTilePrefab();
            InstantiateTool(selectedToolPrefab);
            selectedTileData = new SoundTileData();

            foreach (var fileName in e.SelectedFileNames)
            {
                (selectedTileData as SoundTileData).FileNames.Add(fileName);                
            }
        }
    }

    private void ApplyTool(Cell cell)
    {
        if (selectedTileData != null)
        {
            map.AddTile(selectedTileData);
        }
        else
        {
            map.ClearCell(cell.Column, cell.Row);
        }

        map.Load();
    }
    
    #region Button Handlers
    public void OnClearButtonClicked()
    {
        toolYRotation = 0;
        selectedTileData = null;

        if (tool != null)
        {
            Destroy(tool);
        }
    }

    public void OnDuctButtonClicked(string ductTypeName)
    {
        if (Enum.TryParse<DuctType>(ductTypeName, out DuctType ductType))
        {
            var selectedToolPrefab = TileFactory.Instance.GetTilePrefabByType(ductType);
            InstantiateTool(selectedToolPrefab);
            selectedTileData = new DuctTileData { Type = ductType };
        }
        else
        {
            Debug.LogWarning($"{nameof(OnDuctButtonClicked)} called with invalid {nameof(DuctType)} name \"{ductTypeName}\".");
        }
    }

    public void OnSoundButtonClicked()
    {
        fileBrowser.Closed += FileBrowser_ClosedForSoundTile;
        fileBrowser.Show("*.mp3");
    }

    public void OnStartPositionButtonClicked()
    {
        var selectedToolPrefab = TileFactory.Instance.GetStartPositionPrefab();
        InstantiateTool(selectedToolPrefab);
        selectedTileData = new StartPositionTileData();
    }

    private void OnAddColumnButtonClicked(string sideName)
    {
        if (Enum.TryParse<ColumnSide>(sideName, out ColumnSide side))
        {
            map.AddColumn(side);
        }
        else
        {
            Debug.LogWarning($"{nameof(OnAddColumnButtonClicked)} called with invalid {nameof(ColumnSide)} name \"{sideName}\".");
        }

        map.Load();
    }

    private void OnDeleteColumnButtonClicked(string sideName)
    {
        if (Enum.TryParse<ColumnSide>(sideName, out ColumnSide side))
        {
            map.DeleteColumn(side);
        }
        else
        {
            Debug.LogWarning($"{nameof(OnDeleteColumnButtonClicked)} called with invalid {nameof(ColumnSide)} name \"{sideName}\".");
        }

        map.Load();
    }

    private void OnAddRowButtonClicked(string sideName)
    {
        if (Enum.TryParse<RowSide>(sideName, out RowSide side))
        {
            map.AddRow(side);
        }
        else
        {
            Debug.LogWarning($"{nameof(OnAddRowButtonClicked)} called with invalid {nameof(RowSide)} name \"{sideName}\".");
        }

        map.Load();
    }

    private void OnDeleteRowButtonClicked(string sideName)
    {
        if (Enum.TryParse<RowSide>(sideName, out RowSide side))
        {
            map.DeleteRow(side);
        }
        else
        {
            Debug.LogWarning($"{nameof(OnDeleteRowButtonClicked)} called with invalid {nameof(RowSide)} name \"{sideName}\".");
        }

        map.Load();
    }
    #endregion
}
