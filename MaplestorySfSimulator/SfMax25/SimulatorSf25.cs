using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaplestorySfSimulator.SfMax25
{
    internal static class SimulatorSf25
    {        //https://maplestory.nexon.com/testworld/news/all/88
        static readonly IReadOnlyDictionary<int, SfData25> dict = new Dictionary<int, SfData25>()
        {
            { 15, new SfData25(0.3f, 0.0210f, true)  },
            { 16, new SfData25(0.3f, 0.0210f, true)  },
            { 17, new SfData25(0.3f, 0.0210f)  },
            { 18, new SfData25(0.3f, 0.0280f)  },
            { 19, new SfData25(0.3f, 0.0280f)  },
            { 20, new SfData25(0.3f, 0.0700f)  },
            { 21, new SfData25(0.3f, 0.0700f)  },
            { 22, new SfData25(0.03f, 0.194f)  },
            { 23, new SfData25(0.02f, 0.294f)  },
            { 24, new SfData25(0.01f, 0.394f)  },
        };
        public static async Task TestAsync(
           int totalTry = 100_000_000,
           bool isSafeGuard = true,
           bool isStarCatch = true
           )
        {
            int thread = Environment.ProcessorCount;

            Task<Dictionary<int, int>>[] tasks = new Task<Dictionary<int, int>>[thread];
            for (int i = 0; i < thread; i++)
            {
                int trycount = totalTry / thread;
                if (i == thread - 1)
                {
                    trycount = totalTry - (trycount * (thread - 1));
                }
                tasks[i] = Task.Factory.StartNew<Dictionary<int, int>>(
                    () => SfThread(trycount, isSafeGuard, isStarCatch),
                    TaskCreationOptions.LongRunning
                    );
            }
            await Task.WhenAll(tasks);

            Dictionary<int, int> dictResult = new();
            for (int i = 15; i <= 30; i++)
            {
                dictResult[i] = 0;
            }
            foreach (var item in tasks)
            {
                for (int i = 15; i <= 30; i++)
                {
                    dictResult[i] += item.Result[i];
                }
            }

            //print result
            Console.WriteLine($"SafeGuard:{isSafeGuard}");
            Console.WriteLine($"StarCatch:{isStarCatch}");
            Console.WriteLine($"Total Try:{totalTry}");
            for (int i = 15; i <= 30; i++)
            {
                float spares = (float)totalTry / dictResult[i];
                Console.WriteLine($"{i} star => \t{spares:0.00} spares");
            }
        }


        static Dictionary<int, int> SfThread(int count, bool isSafeGuard, bool isStarCatch)
        {
            Dictionary<int, int> dictResult = new();
            for (int i = 15; i <= 30; i++)
            {
                dictResult[i] = 0;
            }

            for (int i = 0; i < count; i++)
            {
                int result = TrySf(isSafeGuard, isStarCatch);
                for (int j = 15; j <= result; j++)
                {
                    dictResult[j]++;
                }
            }
            return dictResult;
        }
        //return max result
        static int TrySf(bool isSafeGuard, bool isStarCatch)
        {
            int start = isSafeGuard ? 17 : 15;
            while (start < 25)
            {
                switch (dict[start].Enhance(start, isSafeGuard, isStarCatch))
                {
                    case EnhanceResult.Failure:
                        break;

                    case EnhanceResult.Destroy:
                        return start;

                    case EnhanceResult.Success:
                        start++;
                        break;
                }
            }
            return start;
        }
    }
}
