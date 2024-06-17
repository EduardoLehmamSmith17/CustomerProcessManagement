﻿// <auto-generated />
using System;
using CustomerProcessManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CustomerProcessManagement.Migrations
{
    [DbContext(typeof(SystemProcessManagementcsDBContext))]
    [Migration("20240617125050_corrigindo 3")]
    partial class corrigindo3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CustomerProcessManagement.Models.Contact", b =>
                {
                    b.Property<int>("IdContact")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("IdContact");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdContact"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Email");

                    b.Property<int?>("IdLegalPersonContact")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("integer");

                    b.Property<int?>("IdPhysicalPersonContact")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("integer");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Telephone");

                    b.HasKey("IdContact");

                    b.HasIndex("IdLegalPersonContact")
                        .IsUnique();

                    b.HasIndex("IdPhysicalPersonContact")
                        .IsUnique();

                    b.ToTable("Contacts", (string)null);
                });

            modelBuilder.Entity("CustomerProcessManagement.Models.LegalPerson", b =>
                {
                    b.Property<int>("IdLegalPerson")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("IdLegalPerson");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdLegalPerson"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("Address");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("character varying(14)")
                        .HasColumnName("CNPJ");

                    b.Property<string>("CorporateReason")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("CorporateReason");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("FantasyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("FantasyName");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("IdLegalPerson");

                    b.ToTable("LegalPersons", (string)null);
                });

            modelBuilder.Entity("CustomerProcessManagement.Models.PhysicalPerson", b =>
                {
                    b.Property<int>("IdPhysicalPerson")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("IdPhysicalPerson");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdPhysicalPerson"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("Address");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)")
                        .HasColumnName("CPF");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DateOfBirth");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("FullName");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("IdPhysicalPerson");

                    b.ToTable("PhysicalPersons", (string)null);
                });

            modelBuilder.Entity("CustomerProcessManagement.Models.Contact", b =>
                {
                    b.HasOne("CustomerProcessManagement.Models.LegalPerson", null)
                        .WithOne("Contact")
                        .HasForeignKey("CustomerProcessManagement.Models.Contact", "IdLegalPersonContact")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CustomerProcessManagement.Models.PhysicalPerson", null)
                        .WithOne("Contact")
                        .HasForeignKey("CustomerProcessManagement.Models.Contact", "IdPhysicalPersonContact")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CustomerProcessManagement.Models.LegalPerson", b =>
                {
                    b.Navigation("Contact")
                        .IsRequired();
                });

            modelBuilder.Entity("CustomerProcessManagement.Models.PhysicalPerson", b =>
                {
                    b.Navigation("Contact")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
