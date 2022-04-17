using UnityEngine;
using UnityEngine.UI;

public class LeftUI : MonoBehaviour
{
    [SerializeField] private Button _buildFirstButton;
    [SerializeField] private Button _buildSecondButton;
    [SerializeField] private Button _buildResources;

    public Button BuildResources => _buildResources;

    public Button BuildFirstButton => _buildFirstButton;

    public Button BuildSecondButton => _buildSecondButton;
    
    
}