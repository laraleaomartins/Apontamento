using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Apontamento.Models;

namespace Apontamento.Data
{
    public class ApontamentoContext : DbContext
    {
        public ApontamentoContext (DbContextOptions<ApontamentoContext> options)
            : base(options)
        {
        }

        public DbSet<Apontamento.Models.TabelaControle> TabelaControle { get; set; }

        public DbSet<Apontamento.Models.Usuario> Usuario { get; set; }
    }
}
