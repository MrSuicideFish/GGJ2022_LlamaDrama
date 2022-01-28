using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : Mirror.NetworkBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject manager = GameObject.FindWithTag("GameManager");
                if (manager != null)
                {
                    instance = manager.GetComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    [Header("ALPACA")] 
    public Texture2D blueAlpacaTex;
    public Texture2D pinkAlpacaTex;

    public Color blueAlpacaColor;
    public Color pinkAlpacaColor;
    
    [Header("GAMEPLAY COMPONENTS")]
    public CinemachineVirtualCamera gameplayCamera;
    public CinemachineVirtualCamera nonGameplayCamera;
    public CinemachineTargetGroup cameraTargetGroup;
    public MMObjectPool objectPool;
    public UIAlpacaTracker nametagCanvasPrefab;
    
    [Header("Debugging")]
    public bool levelTrackingEnabled = true;

    public CinemachineTrackedDolly gameCameraDollyTrack { get; private set; }
    
    [SyncVar] public float pathFollowSpeed;
    [SyncVar] public float pathStopPosition = 0.9f;
    [SyncVar] [HideInInspector] public bool gameHasStarted;
    [SyncVar] [HideInInspector] public float dollyTrackPosition;
    
    private UIAlpacaTracker nametagCanvas;
    private List<AlpacaController> players;

    private void OnEnable()
    {
        if (nametagCanvas == null)
        {
            nametagCanvas = UIAlpacaTracker.Instantiate(nametagCanvasPrefab);
        }
        gameplayCamera.gameObject.SetActive(true);
        nonGameplayCamera.gameObject.SetActive(false);
    }
    
    [Command(requiresAuthority=false)]
    public void AddPlayer(AlpacaController alpaca)
    {
        ClientAddPlayer(alpaca);
    }
    
    [ClientRpc(includeOwner=true)]
    private void ClientAddPlayer(AlpacaController alpaca)
    {
        if (players == null)
        {
            players = new List<AlpacaController>();
        }
        
        cameraTargetGroup.AddMember(alpaca.transform, 1, alpaca.characterController.radius);
        if (alpaca.netIdentity.isClientOnly)
        {
            alpaca.SetAlpacaColor(AlpacaColor.BLUE);
        }
        else
        {
            alpaca.SetAlpacaColor(AlpacaColor.PINK);
        }
        
        nametagCanvas.CreateNametag("NO NAME", alpaca);
        players.Add(alpaca);
    }

    public void StartGame()
    {
        SpawnFarmers();
        SetPathPosition(0);
        gameHasStarted = true;
    }

    public void EndGame()
    {
        gameHasStarted = false;
    }
    
    private void SetPathPosition(float pos)
    {
        dollyTrackPosition = pos;
    }

    private void UpdateCameraDolly()
    {
        if (gameCameraDollyTrack == null)
        {
            gameCameraDollyTrack = gameplayCamera.GetCinemachineComponent<CinemachineTrackedDolly>();    
        }

        gameCameraDollyTrack.m_PathPosition = dollyTrackPosition;
    }

    private void Update()
    {        
        UpdateCameraDolly();
        
        if (gameHasStarted)
        {
            if (levelTrackingEnabled)
            {
                float nextTrackPosition = dollyTrackPosition + (Time.deltaTime * pathFollowSpeed);
                if (nextTrackPosition < pathStopPosition)
                {
                    SetPathPosition(nextTrackPosition);
                }
            }
        }
    }

    private void OnGUI()
    {
        if (!gameHasStarted)
        {
            if (GUI.Button(new Rect(0, 250, 300, 150), "Start Game"))
            {
                StartGame();
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 250, 300, 150), "End Game"))
            {
                EndGame();
            }
        }
    }

    private void SpawnFarmers()
    {
        
    }
}
