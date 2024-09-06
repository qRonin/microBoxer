﻿// <auto-generated />
using System;
using Boxes.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Boxes.Infrastructure.Migrations
{
    [DbContext(typeof(BoxesContext))]
    partial class BoxesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Boxes.Domain.AggregatesModel.BoxAggregate.Box", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BoxName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Boxes");
                });

            modelBuilder.Entity("Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BoxId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("LastKnownBoxId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("OrderNumber")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BoxId");

                    b.HasIndex("UserId");

                    b.ToTable("BoxContents");
                });

            modelBuilder.Entity("Boxes.Domain.AggregatesModel.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Boxes.Infrastructure.Idempotency.ClientRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("requests", (string)null);
                });

            modelBuilder.Entity("IntegrationEventLogEF.Model.IntegrationEventLogEntry", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<int>("TimesSent")
                        .HasColumnType("integer");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.HasKey("EventId");

                    b.ToTable("IntegrationEventLog", (string)null);
                });

            modelBuilder.Entity("Boxes.Domain.AggregatesModel.BoxAggregate.Box", b =>
                {
                    b.HasOne("Boxes.Domain.AggregatesModel.UserAggregate.User", null)
                        .WithMany("UserBoxes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent", b =>
                {
                    b.HasOne("Boxes.Domain.AggregatesModel.BoxAggregate.Box", "Box")
                        .WithMany("BoxContents")
                        .HasForeignKey("BoxId");

                    b.HasOne("Boxes.Domain.AggregatesModel.UserAggregate.User", null)
                        .WithMany("UserBoxContents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Box");
                });

            modelBuilder.Entity("Boxes.Domain.AggregatesModel.BoxAggregate.Box", b =>
                {
                    b.Navigation("BoxContents");
                });

            modelBuilder.Entity("Boxes.Domain.AggregatesModel.UserAggregate.User", b =>
                {
                    b.Navigation("UserBoxContents");

                    b.Navigation("UserBoxes");
                });
#pragma warning restore 612, 618
        }
    }
}
