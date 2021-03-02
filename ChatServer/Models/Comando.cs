using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public enum Comando
    {
        Login = 0,
        Mensagem = 1,
        MensagemPrivada = 2,
        EntrouNaSala = 3,
        SaiuDaSala = 4,
        RecebeMensagem = 5,
        RecebeMensagemPrivada = 6,
        ErroLogin = 7,
        ErroMensao = 8
    }
}
