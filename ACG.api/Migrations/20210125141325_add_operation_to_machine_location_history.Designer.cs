﻿// <auto-generated />
using System;
using ACG;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ACG.api.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20210125141325_add_operation_to_machine_location_history")]
    partial class add_operation_to_machine_location_history
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("uuid-ossp")
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("ACG.api.Model.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ExternalId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<bool>("IsRegistered")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("ProducerCode")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("UserId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("clients");
                });

            modelBuilder.Entity("ACG.api.Model.Field", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<double>("Area")
                        .HasColumnType("double precision");

                    b.Property<MultiPolygon>("Boundaries")
                        .HasColumnType("geometry");

                    b.Property<string>("ClientId")
                        .HasColumnType("text");

                    b.Property<string>("ExternalId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<bool>("IsRegistered")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("ProducerCode")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<MultiPolygon>("UnpassableBoundaries")
                        .HasColumnType("geometry");

                    b.Property<string>("UserId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("fields");
                });

            modelBuilder.Entity("ACG.api.Model.Machine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("CBSubscriptionId")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ExternalId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<double?>("Lat")
                        .HasColumnType("double precision");

                    b.Property<double?>("Lng")
                        .HasColumnType("double precision");

                    b.Property<string>("Model")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("OtherData")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ProducerCode")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("ProducerCommercialName")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("Type")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("UserId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("machines");
                });

            modelBuilder.Entity("ACG.api.Model.MachineHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<double?>("Lat")
                        .HasColumnType("double precision");

                    b.Property<double?>("Lng")
                        .HasColumnType("double precision");

                    b.Property<Guid>("MachineId")
                        .HasMaxLength(64)
                        .HasColumnType("uuid");

                    b.Property<string>("Operation")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Point>("Position")
                        .HasColumnType("geometry");

                    b.HasKey("Id");

                    b.ToTable("machines_history");
                });
#pragma warning restore 612, 618
        }
    }
}