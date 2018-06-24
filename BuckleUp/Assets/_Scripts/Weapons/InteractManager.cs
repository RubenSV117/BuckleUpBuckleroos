using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages interactable system
/// 
/// Ruben Sanchez
/// 6/3/18
/// </summary>
public class InteractManager : MonoBehaviour
{
    private IInteractable interactableObject;
    private CanvasEvents canvasEvents;

    void Start()
    {
        canvasEvents = GameManager.Instance.CanvasEvents;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Entered interactable trigger 
        if (other.transform.root.GetComponentInChildren<IInteractable>() != null)
        {
            interactableObject = other.transform.root.GetComponentInChildren<IInteractable>();

            // subscribe the interacting player's OnInteract event to Equip
            canvasEvents.OnInteract += interactableObject.Interact;

            interactableObject.InteractableBegin();

            // activate interact button
            canvasEvents.SetInteractButtonActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.GetComponentInChildren<Weapon>() != null)
        {
            // unsubscribe the interacting player's OnInteract event to Equip
            canvasEvents.OnInteract -= interactableObject.Interact;

            // deactivate interact button
            canvasEvents.SetInteractButtonActive(false);
        }
    }
}

