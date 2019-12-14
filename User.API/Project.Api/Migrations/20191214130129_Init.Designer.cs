﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.Infrastructure;

namespace Project.Api.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20191214130129_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Project.Domain.AggregatesModel.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Area");

                    b.Property<int>("AreaId");

                    b.Property<string>("Avatar");

                    b.Property<int>("BrokerageOptions");

                    b.Property<string>("City");

                    b.Property<int>("CityId");

                    b.Property<string>("Company");

                    b.Property<int>("FinMoney");

                    b.Property<string>("FormatBPFile");

                    b.Property<int>("Income");

                    b.Property<bool>("OnPlatform");

                    b.Property<string>("OriginBPFile");

                    b.Property<string>("Province");

                    b.Property<int>("ProvinceId");

                    b.Property<int>("ReferenceId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<int>("Revenue");

                    b.Property<bool>("ShowSecurityInfo");

                    b.Property<int>("SourceId");

                    b.Property<string>("Tags");

                    b.Property<int>("UserId");

                    b.Property<int>("Valuation");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectContributor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<int>("ContributorType");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsCloser");

                    b.Property<int>("ProjectId");

                    b.Property<int>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectContributors");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<int>("ProjectId");

                    b.Property<string>("Text");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectProperties");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectViewer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("ProjectId");

                    b.Property<int>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectViewers");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectVisibleRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProjectId");

                    b.Property<string>("Tags");

                    b.Property<bool>("Visible");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("ProjectVisibleRules");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectContributor", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project", "Project")
                        .WithMany("Contributors")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectProperty", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project", "Project")
                        .WithMany("Properties")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectViewer", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project", "Project")
                        .WithMany("Viewers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectVisibleRule", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project", "Project")
                        .WithOne("VisibleRule")
                        .HasForeignKey("Project.Domain.AggregatesModel.ProjectVisibleRule", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
