using ChatServer.Services;
using ChatServer.Tests.Helpers;
using System;
using Xunit;

namespace ChatServer.Tests
{
    public class WebSocketTests
    {
        private readonly UsuarioService _manager;

        public WebSocketTests()
        {
            _manager = new UsuarioService();
        }

        public class GetSocketById : WebSocketTests
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("foo")]
            public void WhenNonExistentId_ShouldReturnNull(string id)
            {
                var socket = _manager.Obter(id);

                Assert.Null(socket);
            }

            [Fact]
            public void WhenExistingId_ShouldReturnSocket()
            {
                var socket = new FakeSocket();

                _manager.AdicionarWebSocket(socket);
                var id = _manager.Obter(socket)?.Id;

                Assert.Same(socket, _manager.ObterWebSocket(id));
            }
        }

        public class GetAll : WebSocketTests
        {
            [Fact]
            public void WhenEmpty_ShouldReturnZero()
            {
                Assert.Equal(0, _manager.Obter().Count);
            }

            [Fact]
            public void WhenOneSocket_ShouldReturnOne()
            {
                _manager.AdicionarWebSocket(new FakeSocket());

                Assert.Equal(1, _manager.Obter().Count);
            }
        }


        public class AddSocket : WebSocketTests
        {

            [Fact]
            public void WhenInstance_ShouldContainSocket()
            {
                _manager.AdicionarWebSocket(new FakeSocket());

                Assert.Equal(1, _manager.Obter().Count);
            }
        }


    }
}
