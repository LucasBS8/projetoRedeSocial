using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetoRedeSocial.Models
{
    public class Bloqueados
    {
        [Column("idBloqueio")]
        [Display(Name = "Bloqueio ID")]
        public int idBloqueio { get; set; }

        [Column("idUsuario")]
        [Display(Name = "Usuario ID")]
        public int idUsuario { get; set; }

        [Column("bloqueioUsuario")]
        [Display(Name = "Usuario que bloqueou")]
        public Usuario? bloqueioUsuario { get; set; }

        [Column("idUsuarioBloqueado")]
        [Display(Name = "Usuario Bloqueado")]
        public int idUsuarioBloqueado { get; set; }

        [Column("bloqueadoUsuario")]
        [Display(Name = "Usuario bloqueado")]
        public Usuario? bloqueadoUsuario { get; set; }
    }
}
