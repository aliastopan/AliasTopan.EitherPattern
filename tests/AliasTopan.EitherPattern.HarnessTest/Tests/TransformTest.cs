namespace AliasTopan.EitherPattern.HarnessTest.Tests;

public static class TransformTest
{
    public static void Run()
    {
        Console.WriteLine("# _TransformTest_");

        ProxyConfig proxyCfg = new ProxyConfig("127.0.0.1", 8080);
        // Either<ConfigError, ProxyConfig> eitherCfg = Either<ConfigError, ProxyConfig>.Error(new ConfigError("file not found!"));
        Either<ConfigError, ProxyConfig> eitherCfg = Either<ConfigError, ProxyConfig>.Success(proxyCfg);

        Either<ConfigError, string> eitherPort = eitherCfg
            .Map(cfg => cfg.Port)
            .Map(port => $"port:{port}");

        Console.WriteLine(eitherPort.ToString());
    }
}

public record ConfigError(string Message);
public record ProxyConfig(string Ip, int Port);
