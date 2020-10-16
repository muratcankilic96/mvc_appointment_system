namespace MVCKuafor.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EmployeeModel : DbContext
    {
        public EmployeeModel()
            : base("name=EmployeeModel")
        {
        }

        public virtual DbSet<Employees> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Employees>()
                .Property(e => e.surname)
                .IsUnicode(false);

            modelBuilder.Entity<Employees>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<Employees>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Employees>()
                .Property(e => e.email)
                .IsUnicode(false);
        }
    }
}
