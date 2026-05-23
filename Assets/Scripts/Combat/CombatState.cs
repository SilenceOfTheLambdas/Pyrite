namespace Pyrite.Combat
{
    public enum CombatState
    {
        Setup,
        UnitTurnStart,
        PlayerInputWait,
        EnemyAIPlanning,
        ActionExecuction,
        RoundEvaluation,
        Victory,
        Defeat
    }
}