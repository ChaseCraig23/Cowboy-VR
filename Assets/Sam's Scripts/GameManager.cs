using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text cowCountText;
    private int waveCounterTMP;
    public GameState gameState;

    //change this for when the first skybox change happens
    public Material firstSkyboxChange;
    //change this for when the second skybox change happens
    public Material secondSkyboxChange;
    //number of rounds til win
    public int numOfRounds;
    [SerializeField] private int waveRound;
    public EnemySpawnController enemySpawner;
    //Count of amount of aliens in scene
    public int numberOfAliens;
    public AudioSource newWaveSound;
    private int startNumberOfCows;
    public Material startSkyBox;
    public int firstSkyChange;
    public int secondSkyChange;
    
    
    public EnemySpawnController enemySpawnController;
    //number of enemies to spawn at beginning
    [FormerlySerializedAs("enemyCount")] public int numEnemiesSpawn;

    [SerializeField] private int numberOfCows;
    // Start is called before the first frame update
    void Awake()
    {
        SkyboxChange(startSkyBox);
        waveCounterTMP = numOfRounds;
        gameState = GameState.Gameplay;
        GameObject[] aliens = GameObject.FindGameObjectsWithTag("Enemy");
        numberOfAliens = aliens.Length;
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");
        startNumberOfCows = cows.Length;
        Debug.Log("starting cows " +startNumberOfCows);
    }

    private void Update()
    {
        Debug.Log("current cows" + numberOfCows);
        // if (sceneTestChange == 1)
        // {
        //     SkyboxChange(skyboxChange);
        // }
        cowCountText.text = (numberOfCows +" Cows Left" );
        
        //GetGameState();
        GameObject[] aliens = GameObject.FindGameObjectsWithTag("Enemy");
        numberOfAliens = aliens.Length;
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");
        numberOfCows = cows.Length;
        //Increase wave amount if we have one alien left
        waveText.text = ("Aliens" + numberOfAliens);
        if (numberOfAliens == 1)
        {
            waveRound += 1;
            waveCounterTMP -= 1;
            
            WaveManager();
        }
        //if wave countdown complete, go to win screen
        if (waveRound == numOfRounds)
        {
            WavesComplete();
        }
        //lose if our cows = 0
        if (numberOfCows ==1)
        {
            AllCowsKilled();
        }
        if (numberOfCows <= secondSkyChange)
        {
            SkyboxChange(secondSkyboxChange);
        }
        else if (numberOfCows <= firstSkyChange)
        {
            SkyboxChange(firstSkyboxChange);
        }
    }
    //How can I use this function so I can check my gamestate in update?
    // public void GetGameState()
    // {
    //     return gameState;
    // }

    public void WavesComplete()
    {
        gameState = GameState.Victory;
        //numEnemiesSpawn = 0;
        SceneManager.LoadScene("Win Scene");

        //Script to teleport you to victory

    }
    //why cant I call my public int "numberOfAliens" here??
    //Wave Manager
    public void WaveManager()
    {
        enemySpawnController.SpawnEnemies(numEnemiesSpawn);
        numEnemiesSpawn += (numEnemiesSpawn / 2);
        newWaveSound.Play();

    }

    public void SkyboxChange(Material skybox)
    {
        RenderSettings.skybox = skybox;
    }
    public void AllCowsKilled()
    {
          gameState = GameState.GameOver;
          //script to teleport u to L screen
          SceneManager.LoadScene("Lose Scene");
    }
}
