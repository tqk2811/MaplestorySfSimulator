using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaplestorySfSimulator.SfMax30
{
    internal static class SimulatorSf30
    {
        //https://maplestory.nexon.com/testworld/news/all/88
        static readonly IReadOnlyDictionary<int, SfData30> dict = new Dictionary<int, SfData30>()
        {
            { 15, new SfData30(0.30f, 0.0210f, true)  },
            { 16, new SfData30(0.30f, 0.0210f, true)  },
            { 17, new SfData30(0.15f, 0.0680f, true)  },
            { 18, new SfData30(0.15f, 0.0680f)  },
            { 19, new SfData30(0.15f, 0.0850f)  },
            { 20, new SfData30(0.30f, 0.1050f)  },
            { 21, new SfData30(0.15f, 0.1275f)  },
            { 22, new SfData30(0.15f, 0.1700f)  },
            { 23, new SfData30(0.10f, 0.1800f)  },
            { 24, new SfData30(0.10f, 0.1800f)  },
            { 25, new SfData30(0.10f, 0.1800f)  },
            { 26, new SfData30(0.07f, 0.1860f)  },
            { 27, new SfData30(0.05f, 0.1900f)  },
            { 28, new SfData30(0.03f, 0.1940f)  },
            { 29, new SfData30(0.01f, 0.1980f)  },
        };

        public static async Task TestAsync(
            int totalTry = 100_000_000,
            bool isSafeGuard = true,
            bool isSundayEvent = true,
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
                    () => SfThread(trycount, isSafeGuard, isSundayEvent, isStarCatch),
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
            Console.WriteLine($"Sunday Event (-30% destroy below {SfData30.SundayEventBelow}):{isSundayEvent}");
            Console.WriteLine($"StarCatch:{isStarCatch}");
            Console.WriteLine($"Total Try:{totalTry}");
            for (int i = 15; i <= 30; i++)
            {
                float spares = (float)totalTry / dictResult[i];
                Console.WriteLine($"{i} star => \t{spares:0.00} spares");
            }
        }

        static Dictionary<int, int> SfThread(int count, bool isSafeGuard, bool isSundayEvent, bool isStarCatch)
        {
            Dictionary<int, int> dictResult = new();
            for (int i = 15; i <= 30; i++)
            {
                dictResult[i] = 0;
            }

            for (int i = 0; i < count; i++)
            {
                int result = TrySf(isSafeGuard, isSundayEvent, isStarCatch);
                for (int j = 15; j <= result; j++)
                {
                    dictResult[j]++;
                }
            }
            return dictResult;
        }
        //return max result
        static int TrySf(bool isSafeGuard, bool isSundayEvent, bool isStarCatch)
        {
            int start = isSafeGuard ? 18 : 15;
            while (start < 30)
            {
                switch (dict[start].Enhance(start, isSundayEvent, isStarCatch))
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
