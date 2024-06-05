﻿// <auto-generated />
using System;
using MegaLivros.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MegaLivros.Migrations
{
    [DbContext(typeof(MegaLivrosContext))]
    [Migration("20240514235557_MigrationInicial")]
    partial class MigrationInicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MegaLivros.Models.CategoriaModel", b =>
                {
                    b.Property<int>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("NomeCategoria")
                        .HasColumnType("longtext");

                    b.Property<int>("Valor")
                        .HasColumnType("int");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("MegaLivros.Models.ProdutosModel", b =>
                {
                    b.Property<int>("ProdutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("CategoriaLivro")
                        .HasColumnType("longtext");

                    b.Property<int?>("CategoriaModelCategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("NomeLivro")
                        .HasColumnType("longtext");

                    b.Property<int>("QteLivro")
                        .HasColumnType("int");

                    b.Property<int>("ValorLivro")
                        .HasColumnType("int");

                    b.HasKey("ProdutoId");

                    b.HasIndex("CategoriaModelCategoriaId");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("MegaLivros.Models.UsuarioModel", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Endereco")
                        .HasColumnType("longtext");

                    b.Property<int>("Idade")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext");

                    b.Property<string>("Senha")
                        .HasColumnType("longtext");

                    b.HasKey("IdUsuario");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("MegaLivros.Models.ProdutosModel", b =>
                {
                    b.HasOne("MegaLivros.Models.CategoriaModel", null)
                        .WithMany("Produtos")
                        .HasForeignKey("CategoriaModelCategoriaId");
                });

            modelBuilder.Entity("MegaLivros.Models.CategoriaModel", b =>
                {
                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}