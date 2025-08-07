using MaplestorySfSimulator.SfMax25;
using MaplestorySfSimulator.SfMax30;

await SimulatorSf25.TestAsync(
    totalTry: 100_000_000,
    isSafeGuard: true,
    isStarCatch: true
    );

Console.WriteLine("==========================================");

await SimulatorSf30.TestAsync(
    totalTry: 100_000_000,
    isSafeGuard: true,
    isSundayEvent: true,
    isStarCatch: true
    );


Console.ReadLine();