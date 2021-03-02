using ChatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatServer.Services
{
    interface IUsuarioService
    {
        List<Usuario> Obter();
        Usuario Obter(string nome);
        Usuario Obter(WebSocket ws);
        WebSocket ObterWebSocket(string id);
        void AdicionarWebSocket(WebSocket ws);
        Task RemoverWebSocket(string id);
        bool Login(string id, string nome);
    }
}
