using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Holds events for UI canvas
/// 
/// Ruben Sanchez
/// 6/22/18
/// </summary>

public class CanvasEvents : MonoBehaviour
{
    [SerializeField] private GameObject interactButton;
    [SerializeField] private GameObject MoveButton;
    [SerializeField] private GameObject AttackButton;
    [SerializeField] private Image interactIcon;

    public delegate void Interact();
    public event Interact OnInteract;

    private InputManager input;

    void Awake()
    {
        GameManager.Instance.CanvasEvents = this;
        input = GameManager.Instance.Input;
    }
 
    public void SetInteractButton(bool activated)
    {
        interactButton.SetActive(activated);
    }

    public void SendInteract()
    {
        if(OnInteract != null)
            OnInteract.Invoke();
    }

    public void SetInteractButtonIcon(Sprite icon)
    {
        interactIcon.sprite = icon;
    }
}
