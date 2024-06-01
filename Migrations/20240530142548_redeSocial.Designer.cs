﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projetoRedeSocial.Models;

#nullable disable

namespace projetoRedeSocial.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20240530142548_redeSocial")]
    partial class redeSocial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("projetoRedeSocial.Models.Bloqueados", b =>
                {
                    b.Property<int>("idBloqueio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idBloqueio");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idBloqueio"));

                    b.Property<int>("idUsuario")
                        .HasColumnType("int")
                        .HasColumnName("idUsuario");

                    b.Property<int>("idUsuarioBloqueado")
                        .HasColumnType("int")
                        .HasColumnName("idUsuarioBloqueado");

                    b.HasKey("idBloqueio");

                    b.HasIndex("idUsuario");

                    b.HasIndex("idUsuarioBloqueado");

                    b.ToTable("bloqueados");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Comentarios", b =>
                {
                    b.Property<int>("comentarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("comentarioId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("comentarioId"));

                    b.Property<string>("comentario")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("comentario");

                    b.Property<string>("data")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("data");

                    b.Property<int>("postId")
                        .HasColumnType("int")
                        .HasColumnName("postId");

                    b.Property<int>("usuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuarioId");

                    b.HasKey("comentarioId");

                    b.HasIndex("postId");

                    b.HasIndex("usuarioId");

                    b.ToTable("comentarios");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Curtidas", b =>
                {
                    b.Property<int>("idCurtida")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idCurtida");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idCurtida"));

                    b.Property<int>("idPost")
                        .HasColumnType("int")
                        .HasColumnName("idPost");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int")
                        .HasColumnName("idUsuario");

                    b.HasKey("idCurtida");

                    b.HasIndex("idPost");

                    b.HasIndex("idUsuario");

                    b.ToTable("curtidas");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Post", b =>
                {
                    b.Property<int>("postId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("postId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("postId"));

                    b.Property<string>("postArquivo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postArquivo");

                    b.Property<string>("postCor")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postCor");

                    b.Property<string>("postDate")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postDate");

                    b.Property<string>("postDesc")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postDesc");

                    b.Property<string>("postStatus")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postStatus");

                    b.Property<string>("postTitulo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postTitulo");

                    b.Property<int>("usuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuarioId");

                    b.HasKey("postId");

                    b.HasIndex("usuarioId");

                    b.ToTable("post");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Seguidores", b =>
                {
                    b.Property<int>("idSeguidor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idseguidor");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idSeguidor"));

                    b.Property<int>("idUsuario")
                        .HasColumnType("int")
                        .HasColumnName("idUsuario");

                    b.Property<int>("idUsuarioSeguidor")
                        .HasColumnType("int")
                        .HasColumnName("idUsuarioSeguidor");

                    b.HasKey("idSeguidor");

                    b.HasIndex("idUsuario");

                    b.HasIndex("idUsuarioSeguidor");

                    b.ToTable("seguidores");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Usuario", b =>
                {
                    b.Property<int>("usuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("usuarioId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("usuarioId"));

                    b.Property<string>("usuarioCPF")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usuarioCPF");

                    b.Property<string>("usuarioEmail")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usuarioEmail");

                    b.Property<string>("usuarioEndereco")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usuarioEndereco");

                    b.Property<string>("usuarioNome")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usuarioNome");

                    b.Property<string>("usuarioSenha")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usuarioSenha");

                    b.Property<string>("usuarioTelefone")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("usuarioTelefone");

                    b.HasKey("usuarioId");

                    b.ToTable("usuario");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Bloqueados", b =>
                {
                    b.HasOne("projetoRedeSocial.Models.Usuario", "bloqueioUsuario")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("projetoRedeSocial.Models.Usuario", "bloqueadoUsuario")
                        .WithMany()
                        .HasForeignKey("idUsuarioBloqueado")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("bloqueadoUsuario");

                    b.Navigation("bloqueioUsuario");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Comentarios", b =>
                {
                    b.HasOne("projetoRedeSocial.Models.Post", "postComentario")
                        .WithMany()
                        .HasForeignKey("postId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("projetoRedeSocial.Models.Usuario", "usuarioComentario")
                        .WithMany()
                        .HasForeignKey("usuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("postComentario");

                    b.Navigation("usuarioComentario");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Curtidas", b =>
                {
                    b.HasOne("projetoRedeSocial.Models.Post", "postCurtida")
                        .WithMany()
                        .HasForeignKey("idPost")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("projetoRedeSocial.Models.Usuario", "curtidaUsuario")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("curtidaUsuario");

                    b.Navigation("postCurtida");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Post", b =>
                {
                    b.HasOne("projetoRedeSocial.Models.Usuario", "usuarioPost")
                        .WithMany()
                        .HasForeignKey("usuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("usuarioPost");
                });

            modelBuilder.Entity("projetoRedeSocial.Models.Seguidores", b =>
                {
                    b.HasOne("projetoRedeSocial.Models.Usuario", "seguidoUsuario")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("projetoRedeSocial.Models.Usuario", "usuarioSeguidor")
                        .WithMany()
                        .HasForeignKey("idUsuarioSeguidor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("seguidoUsuario");

                    b.Navigation("usuarioSeguidor");
                });
#pragma warning restore 612, 618
        }
    }
}