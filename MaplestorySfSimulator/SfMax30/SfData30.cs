namespace MaplestorySfSimulator.SfMax30
{
    class SfData30
    {
        //[Updated 11/11/25] 30% reduced chance of item destruction when enhancing items below 21-Stars
        //https://www.nexon.com/maplestory/news/update/32522/updated-11-14-v-264-every-little-thing-every-precious-thing-patch-notes#SunnySunday
        public static int SundayEventBelow { get; set; } = 21;
        public SfData30(float success, float destroy, bool isAllowSG = false)
        {
            float destroyRateOfFailed = destroy / (1 - success);

            float failedRate = 1 - success - destroy;//never changed
            this.SuccessRate = success;
            this.DestroyRate = destroy;
            this.SuccessRateAndDestroyRate = success + destroy;

            this.SuccessRate_StarCatch = success * 1.05f;
            float failedRate_StarCatch = 1 - SuccessRate_StarCatch;
            this.DestroyRate_StarCatch = failedRate_StarCatch * destroyRateOfFailed;
            this.SuccessRateAndDestroyRate_StarCatch = SuccessRate_StarCatch + DestroyRate_StarCatch;


            float destroyRate_SunDayEvent = DestroyRate * 0.7f;
            this.SuccessRateAndDestroyRate_SunDayEvent = success + destroyRate_SunDayEvent;

            float destroyRate_SunDayEvent_StarCatch = DestroyRate_StarCatch * 0.7f;
            this.SuccessRateAndDestroyRate_SunDayEvent_StarCatch = SuccessRate_StarCatch + destroyRate_SunDayEvent_StarCatch;
            this.IsAllowSG = isAllowSG;
        }
        public bool IsAllowSG { get; set; }
        public float SuccessRate { get; set; }
        public float DestroyRate { get; set; }


        //pre calc for optimize
        readonly float SuccessRate_StarCatch;
        readonly float DestroyRate_StarCatch;
        readonly float SuccessRateAndDestroyRate;
        readonly float SuccessRateAndDestroyRate_StarCatch;
        readonly float SuccessRateAndDestroyRate_SunDayEvent;
        readonly float SuccessRateAndDestroyRate_SunDayEvent_StarCatch;
        public EnhanceResult Enhance(int currentLevel, bool isSundayEvent, bool isStarCatch)
        {
            float random = (float)Random.Shared.NextDouble();
            if (isStarCatch)
            {
                if (random < SuccessRate_StarCatch)
                    return EnhanceResult.Success;

                if (isSundayEvent && currentLevel < SundayEventBelow)
                {
                    if (random < SuccessRateAndDestroyRate_SunDayEvent_StarCatch)
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

                if (isSundayEvent && currentLevel < SundayEventBelow)
                {
                    if (random < SuccessRateAndDestroyRate_SunDayEvent)
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
}