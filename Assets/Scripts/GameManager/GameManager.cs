using Photon.Realtime;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SpawnPlayer spawnPlayer;
    [SerializeField] private SpawnEnemy spawnEnemy;
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private EnemyPoolManager enemyPool;

    private PlayerController currentPlayer;
    private GameStats gameStats;
    private UIManager uiManager;

    public override void Awake()
    {
        base.Awake();
        gameStats = GetComponent<GameStats>();
    }

    public void StartGame()
    {
        enemyPool.InitializePool();

        var uiObject = Instantiate(uiPrefab);
        uiManager = uiObject.GetComponent<UIManager>();

        currentPlayer = spawnPlayer.Init();

        uiManager.SetPlayerStats(currentPlayer.GetComponent<PlayerStats>());
        uiManager.SetGameStats(gameStats);

        spawnEnemy.Spawn();
    }

    public void OnEnemyDeath(Enemy enemy, Player attacker)
    {
        if (attacker.IsLocal)
        {
            gameStats.IncrementEnemiesDefeated();
        }

        spawnEnemy.Spawn(enemy);
        Debug.Log($"Player {attacker.NickName} killed the enemy.");
    }
}