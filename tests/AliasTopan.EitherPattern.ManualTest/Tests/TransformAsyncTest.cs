namespace AliasTopan.EitherPattern.ManualTest.Tests;

public class TransformAsyncTest
{
    public static async Task RunTest()
    {
        Console.WriteLine("# _TransformAsyncTest_");

        Either<ConfigError, string> getPortResult = await GetProxyConfigAsync()
            .MapAsync(cfg => cfg.Port)
            .MapAsync(port => $"port:{port}");

        Console.WriteLine(getPortResult.ToString());
        Console.Write("\n");
    }

    private static async Task<Either<ConfigError, ProxyConfig>> GetProxyConfigAsync()
    {
        await Task.CompletedTask;

        ProxyConfig proxyCfg = new ProxyConfig("127.0.0.1", 443);

        return Either<ConfigError, ProxyConfig>.Success(proxyCfg);
    }
}
