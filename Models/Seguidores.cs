using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetoRedeSocial.Models
{
    public class Seguidores
    {
        [Column("idseguidor")]
        [Display(Name = "Seguidor ID")]
        public int idSeguidor { get; set; }

        [Column("idUsuario")]
        [Display(Name = "ID Usuario seguido")]
        public int idUsuario { get; set; }

        [Column("seguidoUsuario")]
        [Display(Name = "Usuario seguido")]
        public Usuario? seguidoUsuario { get; set; }

        [Column("idUsuarioSeguidor")]
        [Display(Name = "ID Usuario Seguidor")]
        public int idUsuarioSeguidor { get; set; }

        [Column("usuarioSeguidor")]
        [Display(Name = "Usuario Seguidor")]
        public Usuario? usuarioSeguidor { get; set; }
    }
}
