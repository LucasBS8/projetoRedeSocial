using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetoRedeSocial.Models
{
    public class Post
    {
        [Column("postId")]
        [Display(Name = "Post ID")]
        public int postId { get; set; }

        [Column("usuarioId")]
        [Display(Name = "Usuario ID")]
        public int usuarioId { get; set; }

        [Column("usuarioPost")]
        [Display(Name = "post do usuario")]
        public Usuario? usuarioPost { get; set; }

        [Column("postTitulo")]
        [Display(Name = "Título")]
        public string? postTitulo { get; set; }

        [Column("postDesc")]
        [Display(Name = "Descrição")]
        public string? postDesc { get; set; }

        [Column("postArquivo")]
        [Display(Name = "Arquivo")]
        public string? postArquivo { get; set; }

        [Column("postCor")]
        [Display(Name = "Cor")]
        public string? postCor { get; set; }

        [Column("postDate")]
        [Display(Name = "Data da postagem")]
        public string? postDate { get; set; }

        [Column("postStatus")]
        [Display(Name = "Status")]
        public string? postStatus { get; set; }

    }
}
