using ChatServer.Models;
using ChatServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    public abstract class WebSocketHandler
    {
        protected UsuarioService Service { get; set; }

        public WebSocketHandler(UsuarioService Service)
        {
            this.Service = Service;
        }

        public virtual async Task OnConnected(WebSocket ws)
        {
            Service.AdicionarWebSocket(ws);
        }

        public virtual async Task OnDisconnected(WebSocket ws)
        {
            await Service.RemoverWebSocket(Service.Obter(ws).Id);
        }

        public async Task EnviarMensagemAsync(WebSocket ws, string message)
        {
            if (ws.State != WebSocketState.Open)
                return;
            var encoded = Encoding.UTF8.GetBytes(message);
            await ws.SendAsync(buffer: new ArraySegment<byte>(array: encoded,
                                                                  offset: 0,
                                                                  count: encoded.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task EnviarMensagemParaTodosAsync(string mensagem)
        {
            List<Usuario> usuarios = Service.Obter();
            foreach (var usuario in usuarios)
            {
                if (usuario.Websocket.State == WebSocketState.Open && usuario.Logado)
                    await EnviarMensagemAsync(usuario.Websocket, mensagem);
            }
        }

        public async Task EnviarMensagemParaDestinatarioAsync(string mensagem, string nome)
        {
            var usuario = Service.Obter(nome);
            if (usuario.Websocket.State == WebSocketState.Open && usuario.Logado)
                await EnviarMensagemAsync(usuario.Websocket, mensagem);
        }

        public async Task EnviarMensagemParaDestinatarioAsync(string mensagem, WebSocket ws)
        {
            if (ws.State == WebSocketState.Open)
                await EnviarMensagemAsync(ws, mensagem);
        }

        public abstract Task ReceberAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);


    }
}
