//https://maplestory.nexon.com/testworld/news/all/88
Dictionary<int, SfData> dict = new()
{
    { 15, new SfData(0.30f, 0.0210f, true)  },
    { 16, new SfData(0.30f, 0.0210f, true)  },
    { 17, new SfData(0.15f, 0.0680f, true)  },
    { 18, new SfData(0.15f, 0.0680f)  },
    { 19, new SfData(0.15f, 0.0850f)  },
    { 20, new SfData(0.30f, 0.1050f)  },
    { 21, new SfData(0.15f, 0.1275f)  },
    { 22, new SfData(0.15f, 0.1700f)  },
    { 23, new SfData(0.10f, 0.1800f)  },
    { 24, new SfData(0.10f, 0.1800f)  },
    { 25, new SfData(0.10f, 0.1800f)  },
    { 26, new SfData(0.07f, 0.1860f)  },
    { 27, new SfData(0.05f, 0.1900f)  },
    { 28, new SfData(0.03f, 0.1940f)  },
    { 29, new SfData(0.01f, 0.1980f)  },
};

bool isSafeGuard = true;
bool isSundayEvent = true;
bool isStarCatch = true;
int thread = Environment.ProcessorCount;
int totalTry = 100_000_000;

Task<Dictionary<int, int>>[] tasks = new Task<Dictionary<int, int>>[thread];
for (int i = 0; i < thread; i++)
{
    int trycount = totalTry / thread;
    if (i == thread - 1)
    {
        trycount = totalTry - (trycount * (thread - 1));
    }
    tasks[i] = Task.Factory.StartNew<Dictionary<int, int>>(() => SfThread(trycount), TaskCreationOptions.LongRunning);
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
Console.WriteLine($"SG:{isSafeGuard}");
Console.WriteLine($"Sunday Event (-30% destroy below 21):{isSundayEvent}");
Console.WriteLine($"StarCatch:{isStarCatch}");
Console.WriteLine($"Total Try:{totalTry}");
for (int i = 15; i <= 30; i++)
{
    float spares = (float)totalTry / dictResult[i];
    Console.WriteLine($"{i} star => \t{spares:0.00} spares");
}
Console.ReadLine();



//return max result
int TrySf()
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
Dictionary<int, int> SfThread(int count)
{
    Dictionary<int, int> dictResult = new();
    for (int i = 15; i <= 30; i++)
    {
        dictResult[i] = 0;
    }

    for (int i = 0; i < count; i++)
    {
        int result = TrySf();
        for (int j = 15; j <= result; j++)
        {
            dictResult[j]++;
        }
    }
    return dictResult;
}