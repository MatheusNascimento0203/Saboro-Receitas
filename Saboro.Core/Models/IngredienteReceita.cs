using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saboro.Core.Models
{
    public class IngredienteReceita
    {
        public int Id { get; set; }
        public int IdReceita { get; set; }
        public int DescricaoIngrediente { get; set; }

        public Receita Receita { get; set; }
    }
}