using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Models
{
    public class Mensagem
    {
        public string Conteudo { get; set; }
        public string Destinatario { get; set; }

        public Comando Comando { get; set; }

        public Mensagem(string str)
        {
            string cmd = str.Substring(0, str.IndexOf(' '));

            this.Comando = (Comando) Enum.Parse(typeof(Comando), cmd);
            this.Conteudo = str.Substring(str.IndexOf(' ') + 1);

            if (this.Conteudo.Contains("@"))
            {
                this.Destinatario = this.Conteudo.Substring(0, this.Conteudo.IndexOf(' ')).Replace("@", "");
                this.Conteudo = this.Conteudo.Substring(this.Conteudo.IndexOf(' ') + 1);
            }

        }
    }
}
