using UniMob;

public class GameStats : LifetimeMonoBehaviour
{
    [Atom] public int EnemiesDefeated { get; set; } = 0;

    public void IncrementEnemiesDefeated()
    {
        EnemiesDefeated++;
    }
}