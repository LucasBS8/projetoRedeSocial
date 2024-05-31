using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetoRedeSocial.Models
{
    public class Bloqueados
    {
        [Key]
        [Column("idBloqueio")]
        [Display(Name = "Bloqueio ID")]
        public int idBloqueio { get; set; }

        [Column("idUsuario")]
        [Display(Name = "Usuario ID")]
        public int idUsuario { get; set; }

        [ForeignKey("idUsuario")]
        [Display(Name = "Usuario que bloqueou")]
        public Usuario? bloqueioUsuario { get; set; }

        [Column("idUsuarioBloqueado")]
        [Display(Name = "Usuario Bloqueado")]
        public int idUsuarioBloqueado { get; set; }

        [ForeignKey("idUsuarioBloqueado")]
        [Display(Name = "Usuario bloqueado")]
        public Usuario? bloqueadoUsuario { get; set; }
    }
}
