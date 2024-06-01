using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projetoRedeSocial.Models
{
    public class Usuario
    {
        [Column("usuarioId")]
        [Display(Name = "usuario ID")]
        public int usuarioId { get; set; }

        [Column("usuarioImagem")]
        [Display(Name = "Perfil")]
        public string? usuarioImagem { get; set; }

        [Column("usuarioDesc")]
        [Display(Name = "Descrição")]
        public string? usuarioDesc { get; set; }

        [Column("usuarioNome")]
        [Display(Name = "Nome")]
        public string? usuarioNome { get; set; }

        [Column("usuarioTelefone")]
        [Display(Name = "Telefone")]
        public string? usuarioTelefone { get; set; }

        [Column("usuarioEmail")]
        [Display(Name = "E-mail")]
        public string? usuarioEmail { get; set; }

        [Column("usuarioSenha")]
        [Display(Name = "Senha")]
        public string? usuarioSenha { get; set; }

        [Column("usuarioEndereco")]
        [Display(Name = "Endereço")]
        public string? usuarioEndereco { get; set; }

        [Column("usuarioCPF")]
        [Display(Name = "CPF")]
        public string? usuarioCPF { get; set; }

    }
}
