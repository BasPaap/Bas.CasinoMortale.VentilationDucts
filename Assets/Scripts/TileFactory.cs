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
    [SerializeField] private GameObject grillDuctPrefab;
    [SerializeField] private GameObject soundPrefab;

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

    public GameObject GetTilePrefabByType<T>(T tileType) where T : Enum
    {
        if (tileType is DuctType ductType)
        {
            switch (ductType)
            {
                case DuctType.Straight:
                    return straightDuctPrefab;
                case DuctType.Corner:
                    return cornerDuctPrefab;
                case DuctType.ThreeWayCrossing:
                    return threeWayDuctPrefab;
                case DuctType.FourWayCrossing:
                    return fourWayDuctPrefab;
                case DuctType.Grill:
                    return grillDuctPrefab;
                case DuctType.None:
                    return null;
            };
        }

        return null;
    }

    public GameObject GetTilePrefab<T>(T tile) where T : TileData
    {
        if (tile is DuctTileData ductTile)
        {
            return GetTilePrefabByType(ductTile.Type);
        }
        else if (tile is SoundTileData)
        {
            return soundPrefab;
        }

        return null;
    }    

    public GameObject GetSoundTilePrefab()
    {
        return soundPrefab;
    }
}
