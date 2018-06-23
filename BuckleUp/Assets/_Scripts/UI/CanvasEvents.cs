using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Holds events for UI canvas
/// 
/// Ruben Sanchez
/// 6/22/18
/// </summary>

public class CanvasEvents : MonoBehaviour
{
    [SerializeField] private GameObject interactButton;

    public delegate void Interact();
    public event Interact OnInteract;
 
    public void SetInteractButton(bool activated)
    {
        interactButton.SetActive(activated);
    }

    public void SendInteract()
    {
        if(OnInteract != null)
            OnInteract.Invoke();
    }
}
