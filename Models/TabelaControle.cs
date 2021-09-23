using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apontamento.Models
{
    public class TabelaControle
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Dias da Semana")]
        public string DiasDaSemana { get; set; }

        [Display(Name = "Periodo")]
        public string Periodo { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Display(Name = "Hora Inicial")]
        [DataType(DataType.Time)]
        public DateTime HoraInicial { get; set; }
        [Display(Name = "Hora Final")]

        [DataType(DataType.Time)]
        public DateTime HoraFinal { get; set; }
        [Display(Name = "Horas Trabalhadas")]
        public TimeSpan HorasTrabalhadas { get; set; }

        [Display(Name = "Atividade")]
        public string Atividade { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [ForeignKey("UsuarioFK")]
        public Usuario Usuario { get; set; }



    }
}
