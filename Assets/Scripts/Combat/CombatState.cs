namespace Pyrite.Combat
{
    public enum CombatState
    {
        Setup,
        DetermineTurnOrder,
        UnitTurnStart,
        PlayerInputWait,
        EnemyAIPlanning,
        ActionExecuction,
        RoundEvaluation,
        Victory,
        Defeat
    }
}