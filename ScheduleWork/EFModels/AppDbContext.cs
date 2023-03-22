using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ScheduleWork.EFModels
{
	public partial class AppDbContext : DbContext
	{
		public AppDbContext()
			: base("name=AppDbContext")
		{
		}

		public virtual DbSet<Essay> Essays { get; set; }
		public virtual DbSet<H_Activities> H_Activities { get; set; }
		public virtual DbSet<H_CheckIns> H_CheckIns { get; set; }
		public virtual DbSet<H_Source_Details> H_Source_Details { get; set; }
		public virtual DbSet<Member> Members { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<Video> Videos { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<H_Activities>()
				.HasMany(e => e.H_CheckIns)
				.WithRequired(e => e.H_Activities)
				.HasForeignKey(e => e.Activity_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<H_Activities>()
				.HasMany(e => e.H_Source_Details)
				.WithRequired(e => e.H_Activities)
				.HasForeignKey(e => e.Activity_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.Account)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.Phone_Number)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.EncryptedPassword)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.H_CheckIns)
				.WithRequired(e => e.Member)
				.HasForeignKey(e => e.Member_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.H_Source_Details)
				.WithRequired(e => e.Member)
				.HasForeignKey(e => e.Member_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Orders)
				.WithRequired(e => e.Member)
				.HasForeignKey(e => e.Member_id)
				.WillCascadeOnDelete(false);
		}
	}
}
