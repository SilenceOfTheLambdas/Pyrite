namespace Combat
{
    public enum CombatState
    {
        NiC, // Not in Combat
        Setup,
        UnitTurnStart,
        PlayerInputWait,
        EnemyAIPlanning,
        ActionExecution,
        RoundEvaluation,
        Victory,
        Defeat
    }
}