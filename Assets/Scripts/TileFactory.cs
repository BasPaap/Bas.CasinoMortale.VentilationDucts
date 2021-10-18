using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    [SerializeField] private GameObject straightDuctPrefab;
    [SerializeField] private GameObject cornerDuctPrefab;
    [SerializeField] private GameObject threeWayDuctPrefab;
    [SerializeField] private GameObject fourWayDuctPrefab;

    public static TileFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject GetTilePrefab<T>(T tile) where T : Tile
    {
        if (tile is DuctTile ductTile)
        {
            switch (ductTile.Type)
            {
                case DuctType.Straight:
                    return straightDuctPrefab;
                case DuctType.Corner:
                    return cornerDuctPrefab;
                case DuctType.ThreeWayCrossing:
                    return threeWayDuctPrefab;
                case DuctType.FourWayCrossing:
                    return fourWayDuctPrefab;
                case DuctType.None:
                    return null;
            };
        }

        return null;
    }    
}
