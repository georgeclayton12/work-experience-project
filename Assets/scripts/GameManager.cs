using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Declaring that we only want a single insrtance of this class
    // This is using the singleton pattern
    public static GameManager Instance;
    private void Awake()
    {
        // If there is not game manager yet, let this class be it
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
        }
        // If there is already a game manager, destory this class, and warn in editor
        else if (GameManager.Instance != this)
        {
            Destroy(this);
            Debug.LogError("There where multoiple game managers in the scene");
        }
    }

    public void NewGame()
    {
        SceneManager.UnloadSceneAsync("Menu ui");
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
       
    }

    public void SpawnPlayer(Vector2 position)
    {
        
    }

    public string CreateSeed()
    {
        throw new NotImplementedException();
    }

    public Texture2D CreateMapData(string seed)
    {
        throw new NotImplementedException();
    }

    public void PopulateMap(Texture2D mapData)
    {

    }

}