using Microsoft.EntityFrameworkCore;
using mongo1;
using mongo1.MSSQL_Classes;

public class MyDbContext : DbContext
{

    public DbSet<Classes> Classes { get; set; }

    public DbSet<Methods> Methods { get; set; }

    public DbSet<Parameters> Parameters { get; set; }

    public DbSet<ParameterTypes> ParameterTypes { get; set; }


    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=CLASSES;User ID=sa;Password=password;Encrypt=False;");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Classes>()
        .HasKey(c => c.ClassId); // Define 'Id' as the primary key for the 'Classes' entity

        modelBuilder.Entity<Methods>()
        .HasKey(m => m.MethodId); // Define 'Id' as the primary key for the 'Methods' entity

        modelBuilder.Entity<Parameters>()
        .HasKey(p => p.ParamId); // Define 'Id' as the primary key for the 'Parameters' entity

        modelBuilder.Entity<ParameterTypes>()
        .HasKey(pt => pt.ParameterTypeId); // Define 'Id' as the primary key for the 'ParameterTypes' entity

        modelBuilder.Entity<Methods>()
            .HasOne(m => m.className)
            .WithMany(c => c.Methods)
            .HasForeignKey(m => m.ClassId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        //modelBuilder.Entity<Methods>()
        //    .HasOne(m => m.returnType)
        //    .WithMany(r => r.Methods)
        //    .HasForeignKey(m => m.ReturnTypeId)
        //    .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Parameters>()
            .HasOne(p => p.methodName)
            .WithMany(m => m.Parameters)
            .HasForeignKey(p => p.MethodId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Parameters>()
            .HasOne(p => p.parameterType)
            .WithMany(pt => pt.Parameters)
            .HasForeignKey(p => p.ParameterTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull);

    }
}
