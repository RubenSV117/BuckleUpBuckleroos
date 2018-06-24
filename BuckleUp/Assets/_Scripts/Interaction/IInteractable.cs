using UnityEngine;

/// <summary>
/// Interface for objects in the scene that are interactable
/// 
/// Ruben Sanchez
/// 6/23/18
/// </summary>

public interface IInteractable
{
    void InteractableBegin();
    void Interact();
    void InteractableEnd();
}
