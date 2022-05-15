using CryptoTrading.Kucoin.DesktopInterface.Domain.Aggregates;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;
using CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

using Kucoin.Net.Enums;

using Moq;

using NUnit.Framework;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Test.UseCases;

[TestFixture]
internal sealed class LoadExchangeTests
{
    private static readonly ExchangeSymbol DummySymbol = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT").Create();
    private static readonly ExchangeHistory DummyHistory = new (KlineInterval.ThirtyMinutes, Array.Empty<Candle>());
    private static readonly Exchange DummyExchange = new (DummySymbol, DummyHistory);

    [Test]
    public void LoadsRepositoryExchangeTest()
    {
        var callBackMock = new Mock<IExchangeUpdateCallBack>();
        var mockedCallBack = callBackMock.Object;
        var requestMock = new Mock<IExchangeRequest>();
        requestMock.SetupGet(request => request.ExchangeSymbol).Returns("BTC-USDT");
        requestMock.SetupGet(request => request.UpdateCallBack).Returns(mockedCallBack);
        var mockedRequest = requestMock.Object;
        var repositoryMock = new Mock<IExchangeRepository>();
        _ = repositoryMock.Setup(repository => repository.GetExchange(DummySymbol, mockedCallBack))
            .ReturnsAsync(new Exchange(DummySymbol, DummyHistory));
        var mockedRepository = repositoryMock.Object;
        var useCase = new LoadExchange(mockedRepository);
        Assert.AreEqual(DummyExchange, useCase.Execute(mockedRequest));
    }

    [Test]
    public void LoadsRepositoryExchangeSameReferenceTest()
    {
        var callBackMock = new Mock<IExchangeUpdateCallBack>();
        var mockedCallBack = callBackMock.Object;
        var requestMock = new Mock<IExchangeRequest>();
        requestMock.SetupGet(request => request.ExchangeSymbol).Returns("BTC-USDT");
        requestMock.SetupGet(request => request.UpdateCallBack).Returns(mockedCallBack);
        var mockedRequest = requestMock.Object;
        var repositoryMock = new Mock<IExchangeRepository>();
        _ = repositoryMock.Setup(repository => repository.GetExchange(DummySymbol, mockedCallBack))
            .ReturnsAsync(DummyExchange);
        var mockedRepository = repositoryMock.Object;
        var useCase = new LoadExchange(mockedRepository);
        Assert.AreEqual(DummyExchange, useCase.Execute(mockedRequest));
    }
}