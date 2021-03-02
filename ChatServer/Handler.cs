using ChatServer.Models;
using ChatServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Handler : WebSocketHandler
    {
        public Handler(UsuarioService service) : base(service) { }

        public override async Task OnConnected(WebSocket ws)
        {
            await base.OnConnected(ws);
        }

        public override async Task OnDisconnected(WebSocket ws)
        {
            var usuario = Service.Obter(ws);
            if (usuario.Logado)
            {
                await EnviarMensagemParaTodosAsync($"{Comando.SaiuDaSala} {usuario.Nome} saiu da sala.");
            }
            await base.OnDisconnected(ws);
        }

        public override async Task ReceberAsync(WebSocket ws, WebSocketReceiveResult result, byte[] buffer)
        {
            //Obter o usuário do socket
            var usuario = Service.Obter(ws);

            //Parse da mensagem para identificar o comando e o conteúdo
            var mensagem = new Mensagem(Encoding.UTF8.GetString(buffer, 0, result.Count));

            Usuario destinatario = null;
            if (mensagem.Destinatario != null)
                destinatario = Service.Obter(mensagem.Destinatario);

            //Tratar a ação com base no comando. Consultar class Message para obter a lista de comandos
            switch (mensagem.Comando)
            {
                case Comando.Login:
                    if (Service.Login(usuario.Id, mensagem.Conteudo))
                    {
                        await EnviarMensagemParaTodosAsync($"{Comando.EntrouNaSala} {mensagem.Conteudo} entrou na sala #geral.");
                    }
                    else
                    {
                        await EnviarMensagemParaDestinatarioAsync($"{Comando.ErroLogin} Nome de usuário já existe.", ws);
                    }
                    break;
                case Comando.Mensagem:
                    if (!string.IsNullOrEmpty(mensagem.Destinatario))
                    {
                        if (destinatario == null)
                        {
                            await EnviarMensagemParaDestinatarioAsync($"{Comando.ErroMensao} {mensagem.Destinatario} não encontrado na sala.", ws);
                        }
                        else
                        {
                            await EnviarMensagemParaTodosAsync($"{Comando.RecebeMensagem} {usuario.Nome} diz para {mensagem.Destinatario}: {mensagem.Conteudo}");
                        }
                    }
                    else
                    {
                        await EnviarMensagemParaTodosAsync($"{Comando.RecebeMensagem} {usuario.Nome} diz: {mensagem.Conteudo}");
                    }
                    break;
                case Comando.MensagemPrivada:
                    if (destinatario == null)
                    {
                        await EnviarMensagemParaDestinatarioAsync($"{Comando.ErroMensao} {mensagem.Destinatario} não encontrado na sala.", ws);
                    }
                    else
                    {
                        await EnviarMensagemParaDestinatarioAsync($"{Comando.RecebeMensagemPrivada} {usuario.Nome} diz para {mensagem.Destinatario} (privado): {mensagem.Conteudo}", mensagem.Destinatario);
                        await EnviarMensagemParaDestinatarioAsync($"{Comando.RecebeMensagemPrivada} {usuario.Nome} diz para {mensagem.Destinatario} (privado): {mensagem.Conteudo}", ws);
                    }
                    break;
            }
        }


    }
}
