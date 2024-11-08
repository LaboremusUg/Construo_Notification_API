﻿// <auto-generated />
using System;
using Construo.NotificationAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Construo.NotificationAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241108074042_003")]
    partial class _003
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Construo.NotificationAPI.Models.MessageQueueItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bcc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Context")
                        .HasColumnType("text");

                    b.Property<int?>("ContextId")
                        .HasColumnType("integer");

                    b.Property<int>("EmailGroup")
                        .HasColumnType("integer");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsBodyHtml")
                        .HasColumnType("boolean");

                    b.Property<int?>("MaxSendAttempts")
                        .HasColumnType("integer");

                    b.Property<int>("MessageType")
                        .HasColumnType("integer");

                    b.Property<string>("OverriddenRecipients")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("RecipientContactId")
                        .HasColumnType("integer");

                    b.Property<int>("SendAttempts")
                        .HasColumnType("integer");

                    b.Property<string>("SentByContact")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SentByContactId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("StatusComment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("TemplateType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeRegistered")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("TimeSentAttempt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("TimeToSend")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MessageQueueItems");
                });

            modelBuilder.Entity("Construo.NotificationAPI.Models.Sms.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<int>("ServiceProviderId")
                        .HasColumnType("integer");

                    b.Property<string>("ServiceProviderPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ServiceProviderUsername")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ServiceProviderId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Construo.NotificationAPI.Models.Sms.SmsLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeliveryStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ServiceProvider")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SmsLogs");
                });

            modelBuilder.Entity("Construo.NotificationAPI.Models.Sms.SmsServiceProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ServiceProviders");
                });

            modelBuilder.Entity("Construo.NotificationAPI.Models.Sms.Client", b =>
                {
                    b.HasOne("Construo.NotificationAPI.Models.Sms.SmsServiceProvider", "ServiceProvider")
                        .WithMany("Clients")
                        .HasForeignKey("ServiceProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceProvider");
                });

            modelBuilder.Entity("Construo.NotificationAPI.Models.Sms.SmsServiceProvider", b =>
                {
                    b.Navigation("Clients");
                });
#pragma warning restore 612, 618
        }
    }
}