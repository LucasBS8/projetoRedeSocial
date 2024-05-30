using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetoRedeSocial.Models
{
    public class Curtidas
    {
        [Column("idCurtida")]
        [Display(Name = "Curtida ID")]
        public int idCurtida { get; set; }

        [Column("idUsuario")]
        [Display(Name = "Usuario ID")]
        public int idUsuario { get; set; }

        [Column("curtidaUsuario")]
        [Display(Name = "curtida do suario")]
        public Usuario? curtidaUsuario { get; set; }

        [Column("idPost")]
        [Display(Name = "Post ID")]
        public int idPost { get; set; }

        [Column("postCurtida")]
        [Display(Name = "Post curtido")]
        public Post? postCurtida { get; set; }
    }
}
