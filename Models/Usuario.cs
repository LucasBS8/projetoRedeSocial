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

        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        [Column("usuarioDesc")]
        [Display(Name = "Descrição")]
        public string? usuarioDesc { get; set; }

        [StringLength(20, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        [Column("usuarioNome")]
        [Display(Name = "Nome")]
        public string? usuarioNome { get; set; }

        [StringLength(10, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        [RegularExpression(@"^(?=.*[0-9]).{10,}$", ErrorMessage = "Apenas devem ter números")]
        [Column("usuarioTelefone")]
        [Display(Name = "Telefone")]
        public string? usuarioTelefone { get; set; }

        [Column("usuarioEmail")]
        [Display(Name = "E-mail")]
        public string? usuarioEmail { get; set; }

        [StringLength(10, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas e pelo menos um número.")]
        [Column("usuarioSenha")]
        [Display(Name = "Senha")]
        public string? usuarioSenha { get; set; }

        [StringLength(20, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        [Column("usuarioEndereco")]
        [Display(Name = "Endereço")]
        public string? usuarioEndereco { get; set; }

        [StringLength(11, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        [RegularExpression(@"^(?=.*[0-9]).{8,}$", ErrorMessage = "Apenas devem ter números")]
        [Column("usuarioCPF")]
        [Display(Name = "CPF")]
        public string? usuarioCPF { get; set; }

    }
}
