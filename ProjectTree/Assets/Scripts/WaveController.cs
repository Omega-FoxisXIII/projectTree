﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveController : MonoBehaviour
{
    public EnemySpawner[] spawners;

    private bool _canSpawn, _canEndWave;
    private float _time;
    public float waveCooldown;
    public float enemySpawnRate;
    public int maxWaveEnemies;

    public Material[] flyAnim;
    public Material[] groundAnim;

    [SerializeField] private TextMeshProUGUI nextRoundTimeText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private Image currentEnemiesImage;
    [SerializeField] private Animator hud;

    void Awake()
    {
        GameController.GetInstance().MaxWaveEnemies = maxWaveEnemies;
        GameController.GetInstance().EnemiesSpawnRate = enemySpawnRate;

        Dictionary<String, List<Material>> dict = new Dictionary<string, List<Material>>();
        dict.Add("Dron", new List<Material>(flyAnim));
        dict.Add("Tank", new List<Material>(groundAnim));
        GameController.GetInstance().setMaterials(dict);

        _canEndWave = true;
    }

    private void Start()
    {
        GameController.GetInstance().WaveCounter = 0;
        EndWave();
    }

    // Update is called once per frame
    void Update()
    {
        StartWave();

        SpawnEnemy();

        EndWave();

        _time += Time.deltaTime;

        nextRoundTimeText.SetText(Math.Round((Decimal) (waveCooldown - _time), 0).ToString());
    }

    private void StartWave()
    {
        if (!_canSpawn && _time >= waveCooldown)
        {
            GameController.GetInstance().startWave();
            hud.SetBool("inRound",true);
            hud.SetBool("nextRound",false);
            roundText.SetText("ROUND " + GameController.GetInstance().WaveCounter);

            _canSpawn = true;
            _time = 0;
        }
    }

    private void EndWave()
    {
        if (_canEndWave)
        {
            GameController.GetInstance().endWave();
            hud.SetBool("inRound",false);
            hud.SetBool("nextRound",true);
            _canEndWave = false;
            _canSpawn = false;
            _time = 0;
        }
    }

    public void SpawnEnemy()
    {
        if (_canSpawn && !_canEndWave && _time >= GameController.GetInstance().EnemiesSpawnRate &&
            GameController.GetInstance().CurrentEnemies <
            GameController.GetInstance().MaxWaveEnemies)
        {
            spawners[Random.Range(0, spawners.Length)].SpawnEnemy();

            _time = 0;
        }

        var waveEnemies = GameController.GetInstance().MaxWaveEnemies - GameController.GetInstance().DiedEnemies;
        
        currentEnemiesImage.fillAmount =
            1f - ((float) waveEnemies / (float) GameController.GetInstance().MaxWaveEnemies);
        _canEndWave = GameController.GetInstance().DiedEnemies >= GameController.GetInstance().MaxWaveEnemies;
    }

    public bool CanSpawn
    {
        get => _canSpawn;
        set => _canSpawn = value;
    }
}