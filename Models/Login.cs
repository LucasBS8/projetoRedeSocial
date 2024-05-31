using System.ComponentModel.DataAnnotations.Schema;

namespace projetoRedeSocial.Models
{
    public class Login
    {
        [Column("email")]
        public string email { get; set; }
        [Column("senha")]
        public string senha { get; set; }

    }
}
