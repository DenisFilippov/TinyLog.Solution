using System.Text;
using TinyLog.Client;
using TinyLog.Core;
using TinyLog.DAL.Sqlite;

namespace TinyLog.Tests;

internal class ClientTests
{
  private LogClient _logClient;
  
  [SetUp]
  public void Setup()
  {
    _logClient = new LogClient();
  }

  [Test]
  public void ErrorTest()
  {
    var exception = new InvalidOperationException("invalid operation exception",
      new ArgumentNullException("argument null exception",
        new ArithmeticException("arithmetic exception")));

    _logClient.LogLevel = ItemTypes.Fatal;
    Assert.That(LogContext.Count, Is.EqualTo(0));
    _logClient.Fatal(exception);
    Assert.That(LogContext.Count, Is.EqualTo(1));
    _logClient.Error(exception);
    Assert.That(LogContext.Count, Is.EqualTo(1));
    var item = LogContext.Pop();
    Assert.That(LogContext.Count, Is.EqualTo(0));
  }
}