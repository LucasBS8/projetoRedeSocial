using Microsoft.EntityFrameworkCore;

namespace projetoRedeSocial.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
            usuario = Set<Usuario>();
            post = Set<Post>();
            comentarios = Set<Comentarios>();
            curtidas = Set<Curtidas>();
            seguidores = Set<Seguidores>();
            bloqueados = Set<Bloqueados>();
        }

        public DbSet<Usuario> usuario { get; set; }
        public DbSet<Post> post { get; set; }
        public DbSet<Comentarios> comentarios { get; set; }
        public DbSet<Curtidas> curtidas { get; set; }
        public DbSet<Seguidores> seguidores { get; set; }
        public DbSet<Bloqueados> bloqueados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definir chaves primárias
            modelBuilder.Entity<Usuario>().HasKey(l => l.usuarioId);
            modelBuilder.Entity<Post>().HasKey(e => e.postId);
            modelBuilder.Entity<Comentarios>().HasKey(e => e.comentarioId);
            modelBuilder.Entity<Curtidas>().HasKey(e => e.idCurtida);
            modelBuilder.Entity<Seguidores>().HasKey(e => e.idSeguidor);
            modelBuilder.Entity<Bloqueados>().HasKey(e => e.idBloqueio);

            // Definir relacionamentos
            modelBuilder.Entity<Post>()
                .HasOne(e => e.usuarioPost)
                .WithMany()
                .HasForeignKey(e => e.usuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comentarios>()
                .HasOne(e => e.usuarioComentario)
                .WithMany()
                .HasForeignKey(e => e.usuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comentarios>()
                .HasOne(e => e.postComentario)
                .WithMany()
                .HasForeignKey(e => e.postId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Curtidas>()
                .HasOne(e => e.curtidaUsuario)
                .WithMany()
                .HasForeignKey(e => e.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Curtidas>()
                .HasOne(e => e.postCurtida)
                .WithMany()
                .HasForeignKey(e => e.idPost)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seguidores>()
                .HasOne(e => e.seguidoUsuario)
                .WithMany()
                .HasForeignKey(e => e.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seguidores>()
                .HasOne(e => e.usuarioSeguidor)
                .WithMany()
                .HasForeignKey(e => e.idUsuarioSeguidor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bloqueados>()
                .HasOne(e => e.bloqueioUsuario)
                .WithMany()
                .HasForeignKey(e => e.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bloqueados>()
                .HasOne(e => e.bloqueadoUsuario)
                .WithMany()
                .HasForeignKey(e => e.idUsuarioBloqueado)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
