using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;

namespace ElderCare_Domain.Models;

public partial class ElderCareContext : DbContext
{
    public ElderCareContext()
    {
    }

    public ElderCareContext(DbContextOptions<ElderCareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bankinformation> Bankinformations { get; set; }

    public virtual DbSet<Carer> Carers { get; set; }

    public virtual DbSet<CarerCategory> CarerCategories { get; set; }

    public virtual DbSet<CarerPoint> CarerPoints { get; set; }

    public virtual DbSet<CarerService> CarerServices { get; set; }

    public virtual DbSet<CarerShilft> CarerShilfts { get; set; }

    public virtual DbSet<CarersCustomer> CarersCustomers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<CertificationCarer> CertificationCarers { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractService> ContractServices { get; set; }

    public virtual DbSet<ContractVersion> ContractVersions { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }



    public virtual DbSet<Elderly> Elderlies { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HealthDetail> HealthDetails { get; set; }

    public virtual DbSet<Hobby> Hobbies { get; set; }

    public virtual DbSet<LivingCondition> LivingConditions { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageService> PackageServices { get; set; }

    public virtual DbSet<Psychomotor> Psychomotors { get; set; }

    public virtual DbSet<PsychomotorHealth> PsychomotorHealths { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Shilft> Shilfts { get; set; }

    public virtual DbSet<SystemConfig> SystemConfigs { get; set; }

    public virtual DbSet<Timetable> Timetables { get; set; }

    public virtual DbSet<Tracking> Trackings { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlServer(GetConnectionString());


    }
    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, false)
            .Build();
        return config["ConnectionStrings:DefaultDB"];
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.CreatedDate)
               .HasColumnType("datetime")
               .HasColumnName("created_date");
            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Bankinformation>(entity =>
        {
            entity.HasKey(e => e.BankinfoId);

            entity.ToTable("Bankinformation");

            entity.Property(e => e.BankinfoId)
                .ValueGeneratedNever()
                .HasColumnName("bankinfo_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("account_number");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .HasColumnName("bank_name");
            entity.Property(e => e.Branch)
                .HasMaxLength(100)
                .HasColumnName("branch");
        });

        modelBuilder.Entity<Carer>(entity =>
        {
            entity.ToTable("Carer");

            entity.Property(e => e.CarerId)
                .ValueGeneratedNever()
                .HasColumnName("carer_id");
            entity.Property(e => e.Age)
                .HasMaxLength(50)
                .HasColumnName("age");
            entity.Property(e => e.BankinfoId).HasColumnName("bankinfo_id");
            //entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(100)
                .HasColumnName("gender");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Bankinfo).WithMany(p => p.Carers)
                .HasForeignKey(d => d.BankinfoId)
                .HasConstraintName("FK_Carer_Bankinformation");
        });

        modelBuilder.Entity<CarerCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CarerCategory");

            entity.HasOne(d => d.Carer).WithMany()
                .HasForeignKey(d => d.Carerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarerCategory_Carer");

            entity.HasOne(d => d.Cate).WithMany()
                .HasForeignKey(d => d.Cateid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarerCategory_Category");
        });

        modelBuilder.Entity<CarerPoint>(entity =>
        {
            entity.HasKey(e => e.PointId);

            entity.ToTable("CarerPoint");

            entity.Property(e => e.PointId)
                .ValueGeneratedNever()
                .HasColumnName("point_id");
            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.Point).HasColumnName("point");
        });

        modelBuilder.Entity<CarerService>(entity =>
        {
            entity.ToTable("CarerService");

            entity.Property(e => e.CarerServiceId)
                .ValueGeneratedNever()
                .HasColumnName("carer_service_id");
            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Carer).WithMany(p => p.CarerServices)
                .HasForeignKey(d => d.CarerId)
                .HasConstraintName("FK_CarerService_Carer");

            entity.HasOne(d => d.Service).WithMany(p => p.CarerServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_CarerService_Service");
        });

        modelBuilder.Entity<CarerShilft>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CarerShilft");

            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.ShilftId).HasColumnName("shilft_id");

            entity.HasOne(d => d.Carer).WithMany()
                .HasForeignKey(d => d.CarerId)
                .HasConstraintName("FK_CarerShilft_Carer");

            entity.HasOne(d => d.Shilft).WithMany()
                .HasForeignKey(d => d.ShilftId)
                .HasConstraintName("FK_CarerShilft_Shilft");
        });

        modelBuilder.Entity<CarersCustomer>(entity =>
        {
            entity.HasKey(e => e.CarercusId).HasName("PK_CarersCustomers");

            entity.ToTable("CarersCustomer");

            entity.Property(e => e.CarercusId)
                .ValueGeneratedNever()
                .HasColumnName("carercus_id");
            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");

            entity.HasOne(d => d.Carer).WithMany(p => p.CarersCustomers)
                .HasForeignKey(d => d.CarerId)
                .HasConstraintName("FK_CarersCustomer_Carer");

            entity.HasOne(d => d.Customer).WithMany(p => p.CarersCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CarersCustomer_Customer");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId);

            entity.ToTable("Category");

            entity.Property(e => e.CateId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ServiceName)
              .HasMaxLength(50)
              .HasColumnName("service_name");
        });

        modelBuilder.Entity<Certification>(entity =>
        {
            entity.HasKey(e => e.CertId);

            entity.ToTable("Certification");

            entity.Property(e => e.CertId)
                .ValueGeneratedNever()
                .HasColumnName("cert_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<CertificationCarer>(entity =>
        {
            entity.ToTable("CertificationCarer");

            entity.HasKey(e => e.CarerCertId);

            entity.Property(e =>e.CarerCertId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasValueGenerator<GuidValueGenerator>()
                .HasColumnName("carer_cert_id");

            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.CertId).HasColumnName("cert_id");
            entity.Property(e => e.Qualificationurl)
                .HasMaxLength(300)
                .HasColumnName("qualificationurl");

            entity.HasOne(d => d.Carer).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.CarerId)
                .HasConstraintName("FK_CertificationCarer_Carer");

            entity.HasOne(d => d.Cert).WithMany()
                .HasForeignKey(d => d.CertId)
                .HasConstraintName("FK_CertificationCarer_Certification");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contract");

            entity.Property(e => e.ContractId)
                .ValueGeneratedNever()
                .HasColumnName("contract_id");
            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.ContractType).HasColumnName("contract_type");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ElderlyId).HasColumnName("elderly_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Packageprice).HasColumnName("packageprice");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Carer).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.CarerId)
                .HasConstraintName("FK_Contract_Carer");

            entity.HasOne(d => d.Customer).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Contract_Customer");

            entity.HasOne(d => d.Elderly).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.ElderlyId)
                .HasConstraintName("FK_Contract_Elderly");

            entity.HasOne(d => d.Package).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_Contract_Package");
        });

        modelBuilder.Entity<ContractService>(entity =>
        {
            entity.HasKey(e => e.ContractServicesId).HasName("PK_ContractServices");

            entity.ToTable("ContractService");

            entity.Property(e => e.ContractServicesId)
                .ValueGeneratedNever()
                .HasColumnName("contract_services_id");
            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractServices)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractService_Contract");

            entity.HasOne(d => d.Service).WithMany(p => p.ContractServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_ContractService_Service");
        });

        modelBuilder.Entity<ContractVersion>(entity =>
        {
            entity.ToTable("ContractVersion");

            entity.Property(e => e.ContractVersionId)
                .ValueGeneratedNever()
                .HasColumnName("contract_version_id");
            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractVersions)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_ContractVersion_Contract");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.BankinfoId).HasColumnName("bankinfo_id");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .HasColumnName("customer_name");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Bankinfo).WithMany(p => p.Customers)
                .HasForeignKey(d => d.BankinfoId)
                .HasConstraintName("FK_Customer_Bankinformation");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.DeviceId).HasName("PK_FCMToken");

            entity.ToTable("Device");

            entity.Property(e => e.DeviceId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasValueGenerator<GuidValueGenerator>()
                .HasColumnName("device_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.DeviceToken)
                .HasMaxLength(255)
                .HasColumnName("device_token");

            entity.HasOne(d => d.Account).WithMany(p => p.Devices)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Device_Account");
        });

        modelBuilder.Entity<Elderly>(entity =>
        {
            entity.ToTable("Elderly");

            entity.Property(e => e.ElderlyId)
                .ValueGeneratedNever()
                .HasColumnName("elderly_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.HealthDetailId).HasColumnName("health_detail_id");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.LivingconditionId).HasColumnName("livingcondition_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Note)
                .HasMaxLength(300)
                .HasColumnName("note");
            entity.Property(e => e.Relationshiptocustomer)
                .HasMaxLength(100)
                .HasColumnName("relationshiptocustomer");

            entity.HasOne(d => d.Customer).WithMany(p => p.Elderlies)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Elderly_Customer");

            entity.HasOne(d => d.HealthDetail).WithMany()
                .HasForeignKey(d => d.HealthDetailId)
                .HasConstraintName("FK_Elderly_HealthDetail");

            entity.HasOne(d => d.Livingcondition).WithMany(p => p.Elderlies)
                .HasForeignKey(d => d.LivingconditionId)
                .HasConstraintName("FK_Elderly_LivingCondition");
        });


        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId)
                .ValueGeneratedNever()
                .HasColumnName("feedback_id");
            entity.Property(e => e.CarerServiceId).HasColumnName("carer_service_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Ratng).HasColumnName("ratng");

            entity.HasOne(d => d.CarerService).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CarerServiceId)
                .HasConstraintName("FK_Feedback_CarerService");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Feedback_Customer");
        });

        modelBuilder.Entity<HealthDetail>(entity =>
        {
            entity.ToTable("HealthDetail");

            entity.Property(e => e.HealthDetailId)
                .ValueGeneratedNever()
                .HasColumnName("health_detail_id");
            entity.Property(e => e.Allergy)
                .HasMaxLength(100)
                .HasColumnName("allergy");
            entity.Property(e => e.BloodPressure)
                .HasMaxLength(100)
                .HasColumnName("blood_pressure");
            entity.Property(e => e.DiabetesType)
                .HasMaxLength(100)
                .HasColumnName("diabetes_type");
            entity.Property(e => e.HeartProblems)
                .HasMaxLength(100)
                .HasColumnName("heart_problems");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.MedicalCondition)
                .HasMaxLength(300)
                .HasColumnName("medical_condition");
            entity.Property(e => e.StomachAche)
                .HasMaxLength(100)
                .HasColumnName("stomach_ache");
            entity.Property(e => e.VestibularDisorders)
                .HasMaxLength(100)
                .HasColumnName("vestibular_disorders");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<Hobby>(entity =>
        {
            entity.ToTable("Hobby");

            entity.Property(e => e.HobbyId)
                .ValueGeneratedNever()
                .HasColumnName("hobby_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description").IsRequired(false);
            entity.Property(e => e.ElderlyId).HasColumnName("elderly_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Elderly).WithMany(p => p.Hobbies)
                .HasForeignKey(d => d.ElderlyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Hobby_Elderly");

        });

        modelBuilder.Entity<LivingCondition>(entity =>
        {
            entity.HasKey(e => e.LivingconId);

            entity.ToTable("LivingCondition");

            entity.Property(e => e.LivingconId)
                .ValueGeneratedNever()
                .HasColumnName("livingcon_id");
            entity.Property(e => e.HaveSeperateRoom).HasColumnName("have_seperate_room");
            entity.Property(e => e.LiveWithRelative).HasColumnName("live_with_relative");
            entity.Property(e => e.Others)
                .HasMaxLength(300)
                .HasColumnName("others");
            entity.Property(e => e.Regions)
                .HasMaxLength(100)
                .HasColumnName("regions");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotiId);

            entity.ToTable("Notification");

            entity.Property(e => e.NotiId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasValueGenerator<GuidValueGenerator>()
                .HasColumnName("noti_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Body)
                .HasMaxLength(250)
                .HasColumnName("body");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.SubTitle)
                .HasMaxLength(50)
                .HasColumnName("subtitle");
            entity.Property(e => e.Sound)
                .HasMaxLength(50)
                .HasColumnName("sound");
            entity.Property(e => e.Badge)
                .HasColumnName("badge");
            entity.Property(e => e.ChannelId)
                .HasMaxLength(50)
                .HasColumnName("channel_id");
            entity.Property(e => e.MutableContent)
                .HasColumnName("mutable_content");

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Account");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("Package");

            entity.Property(e => e.PackageId)
                .ValueGeneratedNever()
                .HasColumnName("package_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<PackageService>(entity =>
        {
            entity.HasKey(e => e.PackageServicesId).HasName("PK_PackageServices");

            entity.ToTable("PackageService");

            entity.Property(e => e.PackageServicesId)
                .ValueGeneratedNever()
                .HasColumnName("package_services_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageServices)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PackageService_Package");
        });

        modelBuilder.Entity<Psychomotor>(entity =>
        {
            entity.HasKey(e => e.PsychomotorHealthId);

            entity.ToTable("Psychomotor");

            entity.Property(e => e.PsychomotorHealthId)
                .ValueGeneratedNever()
                .HasColumnName("psychomotor_health_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<PsychomotorHealth>(entity =>
        {
            entity.ToTable("PsychomotorHealth");

            entity.HasKey(e => e.PsychomotorHealthDetailId);
            
            entity.Property(e => e.PsychomotorHealthDetailId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasValueGenerator<GuidValueGenerator>()
                .HasColumnName("psychomotor_health_detail_id");
                
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.HealthDetailId).HasColumnName("health_detail_id");
            entity.Property(e => e.PsychomotorHealthId).HasColumnName("psychomotor_health_id");
            entity.Property(e => e.Status).HasColumnName("status");
            
            entity.HasOne(d => d.HealthDetail).WithMany(p => p.PsychomotorHealths)
                .HasForeignKey(d => d.HealthDetailId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PsychomotorHealth_HealthDetail");
            entity.HasOne(d => d.PsychomotorHealthNavigation).WithMany(p => p.PsychomotorHealths)
                .HasForeignKey(d => d.PsychomotorHealthId)
                .HasConstraintName("FK_PsychomotorHealth_Psychomotor");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.ReportId)
                .ValueGeneratedNever()
                .HasColumnName("report_id");
            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");

            entity.HasOne(d => d.Carer).WithMany(p => p.Reports)
                .HasForeignKey(d => d.CarerId)
                .HasConstraintName("FK_Report_Carer");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reports)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Report_Customer");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK_Services");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("service_id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("desciption");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Shilft>(entity =>
        {
            entity.ToTable("Shilft");

            entity.Property(e => e.ShilftId)
                .ValueGeneratedNever()
                .HasColumnName("shilft_id");
            entity.Property(e => e.Desciption).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity
                .ToTable("SystemConfig");
            entity.HasKey(e => e.SystemConfigId);

            entity.Property(e => e.SystemConfigId).HasColumnName("systemconfig_id");
            entity.Property(e => e.DataName)
            .HasMaxLength(50)
            .HasColumnName("data_name");
            entity.Property(e => e.DataValue)
            .HasMaxLength(50)
            .HasColumnName("data_value");
        });

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.ToTable("Timetable");

            entity.Property(e => e.TimetableId)
                .ValueGeneratedNever()
                .HasColumnName("timetable_id");
            entity.Property(e => e.CarerId).HasColumnName("carer_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.ReportDate)
                .HasColumnType("datetime")
                .HasColumnName("report_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Timeframe)
                .HasMaxLength(100)
                .HasColumnName("timeframe");
        });

        modelBuilder.Entity<Tracking>(entity =>
        {
            entity.ToTable("Tracking");

            entity.Property(e => e.TrackingId)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier")
                .HasValueGenerator<GuidValueGenerator>()
                .HasColumnName("tracking_id");
            entity.Property(e => e.ContractServicesId).HasColumnName("contract_services_id");
            entity.Property(e => e.CusApprove).HasColumnName("cus_approve");
            entity.Property(e => e.CusFeedback)
                .HasMaxLength(300)
                .HasColumnName("cus_feedback");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .HasColumnName("image");
            entity.Property(e => e.PackageServicesId).HasColumnName("package_services_id");
            entity.Property(e => e.ReportContent)
                .HasMaxLength(300)
                .HasColumnName("report_content");
            entity.Property(e => e.ReportDate)
                .HasColumnType("datetime")
                .HasColumnName("report_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimetableId).HasColumnName("timetable_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.ContractServices).WithMany(p => p.Trackings)
                .HasForeignKey(d => d.ContractServicesId)
                .HasConstraintName("FK_Tracking_ContractService");

            entity.HasOne(d => d.PackageServices).WithMany(p => p.Trackings)
                .HasForeignKey(d => d.PackageServicesId)
                .HasConstraintName("FK_Tracking_PackageService");

            entity.HasOne(d => d.Timetable).WithMany(p => p.Trackings)
                .HasForeignKey(d => d.TimetableId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Tracking_Timetable");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("transaction_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CarercusId).HasColumnName("carercus_id");
            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.FigureMoney).HasColumnName("figure_money");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Transaction_Account");

            entity.HasOne(d => d.Contract).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Transaction_Contract");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
