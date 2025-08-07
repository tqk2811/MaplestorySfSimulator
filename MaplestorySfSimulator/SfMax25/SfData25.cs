using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaplestorySfSimulator.SfMax25
{
    internal class SfData25
    {
        public SfData25(float success, float destroy, bool isAllowSG = false)
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
            this.IsAllowSG = isAllowSG;
        }





        public bool IsAllowSG { get; set; }
        public float SuccessRate { get; set; }
        public float DestroyRate { get; set; }


        readonly float SuccessRate_StarCatch;
        readonly float DestroyRate_StarCatch;
        readonly float SuccessRateAndDestroyRate;
        readonly float SuccessRateAndDestroyRate_StarCatch;


        public EnhanceResult Enhance(int currentLevel, bool isSafeGuard, bool isStarCatch)
        {
            float random = (float)Random.Shared.NextDouble();
            if (isStarCatch)
            {
                if (random < SuccessRate_StarCatch)
                    return EnhanceResult.Success;
                if (random < SuccessRateAndDestroyRate_StarCatch)
                    return EnhanceResult.Destroy;
            }
            else
            {
                if (random < SuccessRate)
                    return EnhanceResult.Success;
                if (random < SuccessRateAndDestroyRate)
                    return EnhanceResult.Destroy;
            }
            return EnhanceResult.Failure;
        }
    }
}
