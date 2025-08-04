namespace AliasTopan.EitherPattern.ManualTest.Tests;

public static class TransformTest
{
    public static void RunTest()
    {
        Console.WriteLine("# _TransformTest_");

        Either<ConfigError, string> getPortResult = GetProxyConfig()
            .Map(cfg => cfg.Port)
            .Map(port => $"port:{port}");

        Console.WriteLine(getPortResult.ToString());
        Console.Write("\n");
    }

    private static Either<ConfigError, ProxyConfig> GetProxyConfig()
    {
        ProxyConfig proxyCfg = new ProxyConfig("127.0.0.1", 8080);
        // Either<ConfigError, ProxyConfig> eitherCfg = Either<ConfigError, ProxyConfig>.Error(new ConfigError("file not found!"));

        return Either<ConfigError, ProxyConfig>.Success(proxyCfg);
    }
}

public record ConfigError(string Message);
public record ProxyConfig(string Ip, int Port);
