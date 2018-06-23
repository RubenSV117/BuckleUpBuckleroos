using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets alpha threshold for button to register a hit
///
/// Ruben Sanchez
/// 6/23/18
/// </summary>

public class SetButtonAlphaThreshold : MonoBehaviour
{
    [SerializeField] private  float transparency = .8f;

    void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = transparency;
    }
}