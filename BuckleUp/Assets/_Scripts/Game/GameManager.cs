using System;
using System.Runtime.CompilerServices;
using Cinemachine;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

/// <summary>
/// Singleton GameManager
/// 
/// Ruben Sanchez
/// 6/7/18
/// </summary>

public class GameManager
{
    public delegate void LocalPlayerInit(Player p);
    public event LocalPlayerInit OnLocalPlayerJoined;

    private static GameManager mInstance;
    public static GameManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameManager();
                mInstance.gameObj = new GameObject("GameManager");
                mInstance.gameObj.AddComponent<InputManager>();
            }

            return mInstance;
        }
    }

    private Player mLocalPlayer;
    public Player LocalPlayer
    {
        get
        {
            return mLocalPlayer;
        }

        set
        {
            mLocalPlayer = value;

            if (OnLocalPlayerJoined != null)
                OnLocalPlayerJoined.Invoke(mLocalPlayer);
        }
    }

    private GameObject gameObj;

    private InputManager mInput;
    public InputManager Input
    {
        get
        {
            if (mInput == null)
                mInput = gameObj.GetComponent<InputManager>();

            return mInput;
        }
    }

    public CinemachineVirtualCamera ActiveCam;

    public float CameraLookAtOffset;

    public WeaponManager mWeaponManager;

    public CanvasEvents CanvasEvents;

}
