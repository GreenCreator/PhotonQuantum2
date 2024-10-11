using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> enemyPool;

    private GameObject GetRandomEnemyPrefab()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    private GameObject SpawnRandomEnemy(Vector3 position, Quaternion rotation)
    {
        var enemyPrefab = GetRandomEnemyPrefab();
        var enemyObject = PhotonNetwork.Instantiate(enemyPrefab.name, position, rotation, 0);
        return enemyObject;
    }

    public void InitializePool()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            enemyPool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                var enemyPrefab = GetRandomEnemyPrefab();
                var enemyObject = PhotonNetwork.Instantiate(enemyPrefab.name, Vector3.zero, Quaternion.identity, 0);
                var enemy = enemyObject.GetComponent<Enemy>();
                enemyObject.GetComponent<PhotonView>().RPC(nameof(enemy.SetEnemyActive), RpcTarget.All, false);
                enemyPool.Add(enemyObject);
            }
        }
    }

    public GameObject GetRandomEnemy(Vector3 position, Quaternion rotation)
    {
        GameObject enemyOb = null;

        if (enemyPool.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyPool.Count);
            enemyOb = enemyPool[randomIndex];

            if (enemyOb.activeSelf)
            {
                enemyPool.Remove(enemyOb);
                return GetRandomEnemy(position, rotation);
            }

            enemyOb.transform.position = position;
            enemyOb.transform.rotation = rotation;

            var enemy = enemyOb.GetComponent<Enemy>();
            enemyOb.GetComponent<PhotonView>().RPC(nameof(enemy.SetEnemyActive), RpcTarget.All, true);
            return enemyOb;
        }

        return SpawnRandomEnemy(position, rotation);
    }

    public void ReturnEnemyToPool(GameObject enemyOb)
    {
        var enemy = enemyOb.GetComponent<Enemy>();
        enemyOb.GetComponent<PhotonView>().RPC(nameof(enemy.SetEnemyActive), RpcTarget.All, false);
        enemyPool.Add(enemyOb);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SyncPoolStateWithNewPlayer(newPlayer);
        }
    }

    public void SyncPoolStateWithNewPlayer(Player newPlayer)
    {
        foreach (var ob in enemyPool)
        {
            var isActive = ob.activeSelf;
            var enemy = ob.GetComponent<Enemy>();
            ob.GetComponent<PhotonView>().RPC(nameof(enemy.SetEnemyActive), RpcTarget.All, isActive);
        }
    }
}