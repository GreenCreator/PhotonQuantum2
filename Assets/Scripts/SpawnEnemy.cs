using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private Vector3 spawnZoneCoord;
    [SerializeField] private int maxEnemy = 10;

    private List<Enemy> enemies = new List<Enemy>();
    private EnemyPoolManager enemyPoolManager;

    private void Start()
    {
        enemyPoolManager = GetComponent<EnemyPoolManager>();
    }

    public void Spawn(Enemy enemy = null)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (enemy)
            {
                enemies.Remove(enemy);
                enemyPoolManager.ReturnEnemyToPool(enemy.gameObject);
            }

            while (enemies.Count < maxEnemy)
            {
                var pos = new Vector3(
                    Random.Range(transform.position.x - spawnZoneCoord.x / 2,
                        transform.position.x + spawnZoneCoord.x / 2),
                    transform.position.y,
                    Random.Range(transform.position.z - spawnZoneCoord.z / 2,
                        transform.position.z + spawnZoneCoord.z / 2));

                var enemyGameObject = enemyPoolManager.GetRandomEnemy(pos, Quaternion.identity);
                enemyGameObject.transform.position = pos;
                
                enemy = enemyGameObject.GetComponent<Enemy>();
                enemy.GetComponent<PhotonView>().RPC(nameof(enemy.Init), RpcTarget.All);
                enemies.Add(enemy);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(transform.position, spawnZoneCoord);
    }
}