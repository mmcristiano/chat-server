using ChatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    public class UsuarioService : IUsuarioService
    {
        private List<Usuario> Usuarios;

        public UsuarioService()
        {
            Usuarios = new List<Usuario>();
        }

        public List<Usuario> Obter()
        {
            List<Usuario> usuarios = Usuarios;
            return usuarios;
        }

        public Usuario Obter(string nome)
        {
            Usuario usuario =
                Usuarios.Where(x => x.Nome!=null && x.Nome.ToUpper() == nome.ToUpper())?.FirstOrDefault();
            return usuario;
        }

        public Usuario Obter(WebSocket ws)
        {
            Usuario usuario =
                 Usuarios.Where(u => u.Websocket == ws)?.FirstOrDefault();
            return usuario;
        }


        public WebSocket ObterWebSocket(string id)
        {
            WebSocket ws =
                Usuarios.FirstOrDefault(x => x.Id == id).Websocket;
            return ws;
        }

        public void AdicionarWebSocket(WebSocket ws)
        {
            if (ws != null)
            {
                Usuario usuario = new Usuario(ws);
                Usuarios.Add(usuario);
            }                
        }

        public async Task RemoverWebSocket(string id)
        {
            Usuario usuario;
            if (id != null)
            {
                usuario = Usuarios.Where(u => u.Id == id)?.FirstOrDefault();

                if(usuario != null)
                    await usuario.Websocket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Encerrado pelo serviço",
                                        cancellationToken: CancellationToken.None);
            }
        }

        public bool Login(string id, string nome)
        {
            if (id == null || nome == null)
                return false;

            var usuario = Usuarios.Where(x => x.Nome != null && x.Nome.ToUpper() == nome.ToUpper())?.FirstOrDefault();
            if (usuario != null)
                return false;

            Usuarios.FirstOrDefault(u => u.Id == id)?.Login(nome);
            return true;
        }       

    }
}
