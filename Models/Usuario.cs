using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apontamento.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int ControleId { get; set; }
        public virtual ICollection<TabelaControle> ListaDeControle { get; private set; } = new List<TabelaControle>();

    }


}
