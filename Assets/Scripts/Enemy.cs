using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] private float _maxHealth;
    private float currentHealth;

    public event Action<float> HealthChanged;
    public event Action<Enemy, Player> DeathAction;

    private void Start()
    {
        currentHealth = _maxHealth;
        DeathAction += GameManager.Instance.OnEnemyDeath;
    }

    [PunRPC]
    private void ChangeHealth(float changeValue, int attackerActorNumber)
    {
        var attacker = PhotonNetwork.CurrentRoom.GetPlayer(attackerActorNumber);
        currentHealth = changeValue;
        if (currentHealth <= 0)
        {
            Death(attacker);
        }
        else
        {
            var currentHealthAsPercentage = currentHealth / _maxHealth;
            HealthChanged?.Invoke(currentHealthAsPercentage);
        }
    }

    private void Death(Player attacker)
    {
        HealthChanged?.Invoke(0);
        DeathAction?.Invoke(this, attacker);
    }

    public void TakeDamage(float damagePerSecond, Player attacker)
    {
        photonView.RPC(nameof(TakeDamageRPC), RpcTarget.All, damagePerSecond, attacker.ActorNumber);
    }

    [PunRPC]
    private void TakeDamageRPC(float damagePerSecond, int attackerActorNumber)
    {
        if (photonView.IsMine)
        {
            var changeHealth = currentHealth - damagePerSecond;
            photonView.RPC(nameof(ChangeHealth), RpcTarget.All, changeHealth, attackerActorNumber);
        }
    }

    [PunRPC]
    public void SetEnemyActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    [PunRPC]
    public void Init()
    {
        currentHealth = _maxHealth;
        HealthChanged?.Invoke(1);
    }
}