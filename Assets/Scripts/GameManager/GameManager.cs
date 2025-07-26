using System;
using UnityEngine;

[RequireComponent(typeof(GeneratedWaveSpawner))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerManager[] Players { get; private set; } = Array.Empty<PlayerManager>();
    
    private GeneratedWaveSpawner m_waveSpawner = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Multiple GameManager instances found in the scene. Destroying {gameObject.name}.", this);
            Destroy(this);
            return;
        }
        
        Instance = this;
        
        // Get components
        m_waveSpawner = GetComponent<GeneratedWaveSpawner>();
        
        // Keep track of all players
        Players = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}