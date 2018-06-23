using UnityEngine;

/// <summary>
/// Sends events for PlayMaker
/// 
/// Ruben Sanchez
/// 5/28/18
/// </summary>
public class FsmEventsManager : MonoBehaviour
{
    [SerializeField] private InputManager input;

    private PlayMakerFSM stateMachine;


    private void Awake()
    {
        stateMachine = GetComponent<PlayMakerFSM>();
    }


    
}
