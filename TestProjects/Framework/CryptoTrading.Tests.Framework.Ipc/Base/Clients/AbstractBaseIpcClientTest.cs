using System;
using CryptoTrading.Framework.Ipc.Base.Clients;
using CryptoTrading.Framework.Ipc.Interface.Participants;

using Moq;

using NUnit.Framework;

namespace CryptoTrading.Tests.Framework.Ipc.Base.Clients
{
    internal class AbstractBaseIpcClientTest
    {
        private static Mock<AbstractBaseIpcClient<IIpcListenerTarget>> Mock;
        private static bool ClientStarted;

        [SetUp]
        public void Setup()
        {
            Mock = new(TimeSpan.Zero);
            Mock.CallBase = true;
            Mock.Setup(client => client.StartListening()).Returns(StartListening);
            Mock.Setup(client => client.StopListening()).Returns(StopListening);
        }

        [TearDown]
        public void TearDown()
        {
            Mock = null;
        }

        [Test]
        public void TestStart()
        {
            ClientStarted = false;
            using var client = Mock.Object;
            client.Start();
            Assert.IsFalse(client.StartListening(), "Client cannot start listening if it has already started.");
        }

        [Test]
        public void TestStartListening()
        {
            ClientStarted = false;
            using var client = Mock.Object;
            bool started = client.StartListening();
            Assert.IsTrue(started, "A not started client should be able to start listening.");
            Assert.IsFalse(client.StartListening(), "A listening or started client cannot start listening.");
        }

        [Test]
        public void TestStop()
        {
            using var client = Mock.Object;
            client.Stop();
            Assert.IsFalse(client.StopListening(), "Client cannot stop listening if it has already stopped.");
        }

        [Test]
        public void TestStopListening()
        {
            ClientStarted = true;
            using var client = Mock.Object;
            bool started = client.StopListening();
            Assert.IsTrue(started, "A started client should be able to stop listening.");
            Assert.IsFalse(client.StopListening(), "A client that stopped listening cannot stop listening again.");
        }

        private bool StartListening()
        {
            if (ClientStarted)
            {
                return false;
            }
            ClientStarted = true;
            return true;
        }

        private bool StopListening()
        {
            if (!ClientStarted)
            {
                return false;
            }
            ClientStarted = false;
            return true;
        }
    }
}