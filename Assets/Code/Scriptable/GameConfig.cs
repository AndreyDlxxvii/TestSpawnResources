using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _mapSizeX;
    [SerializeField] private int _mapSizeY;
    [SerializeField] private VoxelTile[] _tilePrefabs;
    [SerializeField] private GameObject _mainTower;
    [SerializeField] private Building _buildFirst;
    [SerializeField] private Building _buildSecond;
    [SerializeField] private VoxelTile _firstTile;
    [SerializeField] private VoxelTile _secondTile;
    [SerializeField] private Button _buttonSpawn;
    [SerializeField] private VoxelTile _thirdTile;
    [SerializeField] private Mineral [] _mineralT1;
    [SerializeField] private Mineral [] _mineralT2;
    [SerializeField] private Mineral [] _mineralT3;
    [Range(0f, 1f)]
    [SerializeField] private float _tearOneWeight = 0.75f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearTwoWeight = 0.23f;
    [Range(0f, 1f)]
    [SerializeField] private float _tearThirdWeight = 0.02f;

    public float TearOneWeight => _tearOneWeight;

    public float TearTwoWeight => _tearTwoWeight;

    public float TearThirdWeight => _tearThirdWeight;

    public Mineral[] MineralT1 => _mineralT1;

    public Mineral[] MineralT2 => _mineralT2;

    public Mineral[] MineralT3 => _mineralT3;

    public Button ButtonSpawn => _buttonSpawn;

    public Building BuildFirst => _buildFirst;

    public Building BuildSecond => _buildSecond;

    public VoxelTile FirstTile => _firstTile;

    public VoxelTile SecondTile => _secondTile;

    public VoxelTile ThirdTile => _thirdTile;

    public int MapSizeX => _mapSizeX;

    public int MapSizeY => _mapSizeY;

    public VoxelTile[] TilePrefabs => _tilePrefabs;

    public GameObject MainTower => _mainTower;
    
}
