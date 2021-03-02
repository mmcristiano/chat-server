using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class Usuario
    {
        public string Id { get; }
        public string Nome { get; set; }
        public WebSocket Websocket { get; }
        public bool Logado { get; set; }
        

        // Adiciona o websocker para nova conexão
        public Usuario(WebSocket websocket)
        {
            Id = Guid.NewGuid().ToString();
            Websocket = websocket;
        }

        public void Login(string nome)
        {
            this.Nome = nome;
            this.Logado = true;
        }

    }
}
