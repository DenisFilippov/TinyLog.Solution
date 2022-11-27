using System.Text;
using TinyLog.Client;
using TinyLog.Core;

namespace TinyLog.Tests;

internal class ClientTests
{
  private LogClient _logClient;

  private Exception CreateException()
  {
    var result = new InvalidOperationException("invalid operation exception",
      new ArgumentNullException("argument null exception",
        new ArithmeticException("arithmetic exception")));

    result.Data.Add("key1", Encoding.UTF8.GetBytes("Denis"));
    result.Data.Add("key2", Encoding.UTF8.GetBytes("Maria"));


    return result;
  }

  [SetUp]
  public void Setup()
  {
    _logClient = new LogClient();
  }

  [Test]
  public void ErrorTest()
  {
    var exception = CreateException();

    _logClient.LogLevel = ItemTypes.Fatal;
    Assert.That(LogContext.Count, Is.EqualTo(0));
    var item1 = LogContext.Pop();
    _logClient.Fatal(exception);
    Assert.That(LogContext.Count, Is.EqualTo(1));
    _logClient.Error(exception);
    Assert.That(LogContext.Count, Is.EqualTo(1));
    var item = LogContext.Pop();
    Assert.That(LogContext.Count, Is.EqualTo(0));
  }

  [Test]
  public void ClientTest1()
  {
    using var cts = new CancellationTokenSource();
    var token = cts.Token;
    token.Register(() => { Console.WriteLine("DONE"); });
    LogManager.Initialize("E:\\Projects\\CSharp\\TinyLog.Solution\\TinyLog.Tests\\TLog.json");
    LogManager.Start(token);

    _logClient.Error(CreateException());
    _logClient.Info("Hello, world!!!");
    _logClient.Warning("It`s a warning.");

    Thread.Sleep(20000);
    Console.WriteLine("DONE");
  }
}