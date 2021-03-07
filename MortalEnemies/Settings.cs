namespace MortalEnemies
{
    public enum AttackCommitment
    {
        None,
        Original,
        RivalRemix
    }

    public class Settings
    {
        public AttackCommitment CommitmentMode = AttackCommitment.Original;
    }
}
