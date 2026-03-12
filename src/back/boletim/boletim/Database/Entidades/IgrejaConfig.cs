using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace boletim.Database.Entidades
{
    public class IgrejaConfig
    {
        [Key]
        public int Id { get; set; }
        public Frequencia Frequencia { get; set; } = Frequencia.Semanal;

        [ForeignKey(nameof(Id))]
        public Igreja Igreja { get; set; }
    }

    public enum Frequencia
    {
        Semanal = 1,
        Quizenal = 2,
    }
}
