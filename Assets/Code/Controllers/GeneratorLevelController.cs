using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GeneratorLevelController : IOnController, IOnStart, IOnLateUpdate
{
    public List<VoxelTile> PositionSpawnedTiles => _positionSpawnedTiles;

    private List<VoxelTile> _positionSpawnedTiles = new List<VoxelTile>();
    private VoxelTile _firstTile;
    private List<VoxelTile> _voxelTiles;
    private VoxelTile[,] _spawnedTiles;
    private GameConfig _gameConfig;
    private int _offsetInstanceTiles;
    private Button buttonRespawn;
    private RightUI _rightUI;
    private Transform _canvas;
    private NavMeshSurface _navMesh;
    private BtnUIController _btnUIController;
    private Dictionary<Button, Vector3> _spawnedButtons = new Dictionary<Button, Vector3>();

    public GeneratorLevelController(List<VoxelTile> tiles, GameConfig gameConfig, RightUI rightUI,
        BtnUIController btnUIController, Transform canvas, NavMeshSurface navMesh)
    {
        _spawnedTiles = new VoxelTile[gameConfig.MapSizeX,gameConfig.MapSizeY];
        _voxelTiles = tiles;
        _gameConfig = gameConfig;
        _offsetInstanceTiles = _voxelTiles[0].SizeTile;
        _rightUI = rightUI;
        _btnUIController = btnUIController;
        buttonRespawn = gameConfig.ButtonSpawn;
        _canvas = canvas;
        _navMesh = navMesh;
    }
    
    public void OnStart()
    {
        GameObject.Instantiate(buttonRespawn);
        _btnUIController.TileSelected += SelectFirstTile;
    }
    
    private void SelectFirstTile(int numTile)
    {
        switch (numTile)
        {
            case 0:
                _firstTile = _voxelTiles[numTile];
                break;
            case 1:
                _firstTile = _voxelTiles[numTile];
                break;
            case 2:
                _firstTile = _voxelTiles[numTile];
                break;
        }
        PlaceFirstTile(_firstTile);
        _btnUIController.TileSelected -= SelectFirstTile;
        _rightUI.gameObject.SetActive(false);
    }
    
    private void PlaceFirstTile(VoxelTile tile)
    {
        int x = _gameConfig.MapSizeX / 2;
        int y = _gameConfig.MapSizeY / 2;
        if (_spawnedTiles[x, y] == null)
        {
            _spawnedTiles[x, y] = GameObject.Instantiate(tile, new Vector3(x, 0, y), 
                Quaternion.identity);
            _spawnedTiles[x, y].NumZone = 1;
            _positionSpawnedTiles.Add(_spawnedTiles[x, y]);
            CreateButton(_spawnedTiles[x, y]);
        }
        GameObject.Instantiate(_gameConfig.MainTower,new Vector3(x, 0, y), Quaternion.identity);
    }
    
    private void CreateButton(VoxelTile tile)
    {
        int i = 0;
        foreach (var ell in tile.TablePassAccess)
        {
            switch (ell)
            {
                case 0:
                    break;
                case 1:
                    if (i == 0 && Extensions.CheckEmptyPosition(tile, 0, -_offsetInstanceTiles, _spawnedTiles))
                    {
                        Vector3 posToSpawnBtn = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - _offsetInstanceTiles);
                        InstansButton(posToSpawnBtn, Vector3.back, tile, i);
                    }
                    else if (i == 1 && Extensions.CheckEmptyPosition(tile, -_offsetInstanceTiles, 0, _spawnedTiles))
                    {
                        Vector3 posToSpawnBtn = new Vector3(tile.transform.position.x - _offsetInstanceTiles, tile.transform.position.y, tile.transform.position.z);
                        InstansButton(posToSpawnBtn, Vector3.left, tile, i);
                    }
                    else if (i == 2 && Extensions.CheckEmptyPosition(tile, 0, _offsetInstanceTiles, _spawnedTiles))
                    {
                        Vector3 posToSpawnBtn = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + _offsetInstanceTiles);
                        InstansButton(posToSpawnBtn, Vector3.forward, tile, i);
                    }
                    else if (i == 3 && Extensions.CheckEmptyPosition(tile, _offsetInstanceTiles, 0, _spawnedTiles))
                    {
                        Vector3 posToSpawnBtn = new Vector3(tile.transform.position.x + _offsetInstanceTiles, tile.transform.position.y, tile.transform.position.z);
                        InstansButton(posToSpawnBtn, Vector3.right, tile, i);
                    } 
                    break;
            }
            i++;
        }
        _navMesh.BuildNavMesh();
    }
    
    private void InstansButton(Vector3 posForButton, Vector3 direction, VoxelTile tile, int numOfGroupAvailableTiles)
    {
        if (!_spawnedButtons.ContainsValue(posForButton))
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(posForButton);
            Button btn = GameObject.Instantiate(buttonRespawn, pos, Quaternion.identity, _canvas);
            _spawnedButtons.Add(btn, posForButton);
            btn.onClick.AddListener(delegate
            {
                _spawnedButtons.Remove(btn);
                CreateTile(tile, direction * _offsetInstanceTiles, numOfGroupAvailableTiles);
                btn.onClick.RemoveAllListeners();
                GameObject.Destroy(btn.gameObject);
            });
        }
    }
    
    private void CreateTile(VoxelTile voxelTile, Vector3 spawnPos, int i)
    {
        var _availableTiles = Extensions.TilesCanBeSet(i, _voxelTiles);
        var pos = new Vector3(voxelTile.transform.position.x + spawnPos.x, 0 , voxelTile.transform.position.z + spawnPos.z);
        var tile = GameObject.Instantiate(_availableTiles[Random.Range(0, _availableTiles.Count-1)], pos, Quaternion.identity);
        
        tile.NumZone = voxelTile.NumZone + 1;
        if (tile.NumZone==2)
        {
            tile.WeightTile = 5;
        }
        else
        {
            tile.WeightTile = 5 * (tile.NumZone - 1);
        }
        
        _availableTiles.Clear();
        _spawnedTiles[(int) pos.x, (int) pos.z] = tile;
        _positionSpawnedTiles.Add(tile);
        CreateButton(tile);
    }
    
    public void OnLateUpdate(float deltaTime)
    {
        if (_spawnedButtons.Count != 0)
        {
            foreach (var ell in _spawnedButtons)
            {
                ell.Key.transform.position = Camera.main.WorldToScreenPoint(ell.Value);
            }
        }
    }
}