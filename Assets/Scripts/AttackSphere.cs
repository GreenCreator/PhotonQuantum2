using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AttackSphere : MonoBehaviourPun
{
    private PlayerStats playerStats;
    private Rigidbody rb;

    private Dictionary<Enemy, float> attackTimers = new Dictionary<Enemy, float>();

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            AttackEnemies();
        }
    }

    private void AttackEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, playerStats.AttackRadius);
        List<Enemy> nearestEnemies = GetNearestEnemies(enemiesInRange, 3);

        foreach (Enemy enemy in nearestEnemies)
        {
            if (!attackTimers.ContainsKey(enemy))
            {
                attackTimers[enemy] = 1f; 
            }

            attackTimers[enemy] += Time.deltaTime;

            if (attackTimers[enemy] >= 1f)
            {
                enemy.TakeDamage(playerStats.Damage, photonView.Owner);
                attackTimers[enemy] = 0f; 
            }
        }

        RemoveOutOfRangeEnemies(nearestEnemies);
    }

    private void RemoveOutOfRangeEnemies(List<Enemy> nearestEnemies)
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (var entry in attackTimers)
        {
            if (!nearestEnemies.Contains(entry.Key))
            {
                enemiesToRemove.Add(entry.Key);
            }
        }

        foreach (var enemy in enemiesToRemove)
        {
            attackTimers.Remove(enemy);
        }
    }

    private List<Enemy> GetNearestEnemies(Collider[] enemies, int maxTargets)
    {
        var enemyDistances = new List<KeyValuePair<Enemy, float>>();

        foreach (Collider enemyCollider in enemies)
        {
            var enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                enemyDistances.Add(new KeyValuePair<Enemy, float>(enemy, distance));
            }
        }

        enemyDistances.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        List<Enemy> nearestEnemies = new List<Enemy>();
        for (int i = 0; i < Mathf.Min(maxTargets, enemyDistances.Count); i++)
        {
            nearestEnemies.Add(enemyDistances[i].Key);
        }

        return nearestEnemies;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, playerStats.AttackRadius);
    }
}
