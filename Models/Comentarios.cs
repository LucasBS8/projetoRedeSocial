using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetoRedeSocial.Models
{
    public class Comentarios
    {
        [Column("comentarioId")]
        [Display(Name = "Comentário ID")]
        public int comentarioId { get; set; }

        [Column("usuarioId")]
        [Display(Name = "Usuario ID")]
        public int usuarioId { get; set; }

        [Column("usuarioComentario")]
        [Display(Name = "Comentario do usuario")]
        public Usuario? usuarioComentario { get; set; }

        [Column("postId")]
        [Display(Name = "Post ID")]
        public int postId { get; set; }

        [Column("postComentario")]
        [Display(Name = "Comentario do post")]
        public Post? postComentario { get; set; }

        [Column("comentario")]
        [Display(Name = "Comentário")]
        public string? comentario { get; set; }

        [Column("data")]
        [Display(Name = "Data")]
        public string? data { get; set; }
    }
}
