using UniMob;
using UnityEngine.UI;

public class UIManager : LifetimeMonoBehaviour
{
    public Text playerStatsText;
    public Text enemyCounterText;
    public PlayerStats playerStats;
    public GameStats gameStats;
    public Button upgradeButton;

    protected override void Start()
    {
        // Реакции, которые обновляют UI при изменении свойств
        Atom.Reaction(Lifetime,
            () =>
            {
                playerStatsText.text =
                    $"Speed: {playerStats.Speed}\nDamage: {playerStats.Damage}\nAttack Radius: {playerStats.AttackRadius}";
            });

        Atom.Reaction(Lifetime, () => { enemyCounterText.text = $"Enemies Defeated: {gameStats.EnemiesDefeated}"; });
        
        upgradeButton.onClick.AddListener(() => playerStats.RandomUpgrade());
    }

    public void SetPlayerStats(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }

    public void SetGameStats(GameStats gameStats)
    {
        this.gameStats = gameStats;
    }
}