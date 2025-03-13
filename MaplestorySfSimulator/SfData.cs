class SfData
{
    public SfData(float success, float destroy, bool isAllowSG = false)
    {
        this.SuccessRate = success;
        this.DestroyRate = destroy;
        this.SuccessRate_StarCatch = success * 1.05f;

        this.SuccessRateAndDestroyRate = success + destroy;
        SuccessRateAndDestroyRate_StarCatch = SuccessRate_StarCatch + destroy;

        this.SuccessRateAndDestroyRateSunDayEvent = success + (destroy * 0.7f);
        this.SuccessRateAndDestroyRateSunDayEvent_StarCatch = SuccessRate_StarCatch + (destroy * 0.7f);
        this.IsAllowSG = isAllowSG;
    }
    public bool IsAllowSG { get; set; }
    public float SuccessRate { get; set; }
    public float DestroyRate { get; set; }
    public float FailureRate => 1 - SuccessRate - DestroyRate;


    //optimize
    readonly float SuccessRate_StarCatch;
    readonly float SuccessRateAndDestroyRate;
    readonly float SuccessRateAndDestroyRate_StarCatch;
    readonly float SuccessRateAndDestroyRateSunDayEvent;
    readonly float SuccessRateAndDestroyRateSunDayEvent_StarCatch;
    public EnhanceResult Enhance(int currentLevel, bool isSundayEvent, bool isStarCatch)
    {
        float random = (float)Random.Shared.NextDouble();
        if (isStarCatch)
        {
            if (random < SuccessRate_StarCatch)
                return EnhanceResult.Success;

            if (isSundayEvent && currentLevel < 21)
            {
                if (random < SuccessRateAndDestroyRateSunDayEvent_StarCatch)
                    return EnhanceResult.Destroy;
            }
            else
            {
                if (random < SuccessRateAndDestroyRate_StarCatch)
                    return EnhanceResult.Destroy;
            }
        }
        else
        {
            if (random < SuccessRate)
                return EnhanceResult.Success;
            if (isSundayEvent && currentLevel < 21)
            {
                if (random < SuccessRateAndDestroyRateSunDayEvent)
                    return EnhanceResult.Destroy;
            }
            else
            {
                if (random < SuccessRateAndDestroyRate)
                    return EnhanceResult.Destroy;
            }
        }
        return EnhanceResult.Failure;
    }
}
