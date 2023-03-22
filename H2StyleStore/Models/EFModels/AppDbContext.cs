using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace H2StyleStore.Models.EFModels
{
	public partial class AppDbContext : DbContext
	{
		public AppDbContext()
			: base("name=AppDbContext")
		{
		}

		public virtual DbSet<Address> Addresses { get; set; }
		public virtual DbSet<Cart> Carts { get; set; }
		public virtual DbSet<CommonQuestion> CommonQuestions { get; set; }
		public virtual DbSet<CustomerQuestion> CustomerQuestions { get; set; }
		public virtual DbSet<Eassy_Follows> Eassy_Follows { get; set; }
		public virtual DbSet<EComments_Likes> EComments_Likes { get; set; }
		public virtual DbSet<Elike> Elikes { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<Essay> Essays { get; set; }
		public virtual DbSet<Essays_Comments> Essays_Comments { get; set; }
		public virtual DbSet<H_Activities> H_Activities { get; set; }
		public virtual DbSet<H_CheckIns> H_CheckIns { get; set; }
		public virtual DbSet<H_Source_Details> H_Source_Details { get; set; }
		public virtual DbSet<Image> Images { get; set; }
		public virtual DbSet<Member> Members { get; set; }
		public virtual DbSet<News> News { get; set; }
		public virtual DbSet<Order_Details> Order_Details { get; set; }
		public virtual DbSet<Order_Log> Order_Log { get; set; }
		public virtual DbSet<Order_Status> Order_Status { get; set; }
		public virtual DbSet<Order_StatusDescription> Order_StatusDescription { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<PCategory> PCategories { get; set; }
		public virtual DbSet<PComments_Helpful> PComments_Helpful { get; set; }
		public virtual DbSet<PermissionsE> PermissionsEs { get; set; }
		public virtual DbSet<PermissionsM> PermissionsMs { get; set; }
		public virtual DbSet<Product_Comments> Product_Comments { get; set; }
		public virtual DbSet<Product_Likes> Product_Likes { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<Question_Categories> Question_Categories { get; set; }
		public virtual DbSet<Spec> Specs { get; set; }
		public virtual DbSet<Tag> Tags { get; set; }
		public virtual DbSet<VCommentLike> VCommentLikes { get; set; }
		public virtual DbSet<VideoCategory> VideoCategories { get; set; }
		public virtual DbSet<VideoComment> VideoComments { get; set; }
		public virtual DbSet<VideoLike> VideoLikes { get; set; }
		public virtual DbSet<Video> Videos { get; set; }
		public virtual DbSet<VideoView> VideoViews { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Address>()
				.Property(e => e.destination_THE)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.Property(e => e.Account)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.Property(e => e.EncryptedPassword)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.HasMany(e => e.Essays)
				.WithRequired(e => e.Employee)
				.HasForeignKey(e => e.Influencer_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Essay>()
				.HasMany(e => e.Eassy_Follows)
				.WithRequired(e => e.Essay)
				.HasForeignKey(e => e.Eassy_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Essay>()
				.HasMany(e => e.Elikes)
				.WithRequired(e => e.Essay)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Essay>()
				.HasMany(e => e.Essays_Comments)
				.WithRequired(e => e.Essay)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Essay>()
				.HasMany(e => e.Images)
				.WithMany(e => e.Essays)
				.Map(m => m.ToTable("Essays_Imgs").MapLeftKey("Essay_Id").MapRightKey("Img_Id"));

			modelBuilder.Entity<Essay>()
				.HasMany(e => e.Tags)
				.WithMany(e => e.Essays)
				.Map(m => m.ToTable("Essays_Tags").MapLeftKey("Essay_Id"));

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

			modelBuilder.Entity<Image>()
				.Property(e => e.Path)
				.IsUnicode(false);

			modelBuilder.Entity<Image>()
				.HasMany(e => e.Videos)
				.WithRequired(e => e.Image)
				.HasForeignKey(e => e.ImageId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Image>()
				.HasMany(e => e.Products)
				.WithMany(e => e.Images)
				.Map(m => m.ToTable("Imgs_Products").MapLeftKey("Img_Id").MapRightKey("Product_Id"));

			modelBuilder.Entity<Image>()
				.HasMany(e => e.Product_Comments)
				.WithMany(e => e.Images)
				.Map(m => m.ToTable("PComments_Imgs").MapLeftKey("PComment_img_id").MapRightKey("Comment_id"));

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
				.HasMany(e => e.Addresses)
				.WithRequired(e => e.Member)
				.HasForeignKey(e => e.Member_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Carts)
				.WithRequired(e => e.Member)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.CustomerQuestions)
				.WithOptional(e => e.Member)
				.HasForeignKey(e => e.Member_Id);

			modelBuilder.Entity<Member>()
				.HasOptional(e => e.Eassy_Follows)
				.WithRequired(e => e.Member);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Essays_Comments)
				.WithRequired(e => e.Member)
				.HasForeignKey(e => e.Member_Id)
				.WillCascadeOnDelete(false);

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

			modelBuilder.Entity<Member>()
				.HasMany(e => e.VideoComments)
				.WithRequired(e => e.Member)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order_Status>()
				.HasMany(e => e.Orders)
				.WithRequired(e => e.Order_Status)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order_StatusDescription>()
				.HasMany(e => e.Orders)
				.WithOptional(e => e.Order_StatusDescription)
				.HasForeignKey(e => e.Status_Description_id);

			modelBuilder.Entity<Order>()
				.HasMany(e => e.Order_Details)
				.WithRequired(e => e.Order)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order>()
				.HasMany(e => e.Product_Comments)
				.WithRequired(e => e.Order)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PCategory>()
				.HasMany(e => e.Products)
				.WithRequired(e => e.PCategory)
				.HasForeignKey(e => e.Category_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PermissionsE>()
				.HasMany(e => e.Employees)
				.WithOptional(e => e.PermissionsE)
				.HasForeignKey(e => e.Permission_id);

			modelBuilder.Entity<Product>()
				.HasMany(e => e.Product_Comments)
				.WithRequired(e => e.Product)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Product>()
				.HasMany(e => e.Product_Likes)
				.WithRequired(e => e.Product)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Product>()
				.HasMany(e => e.Specs)
				.WithRequired(e => e.Product)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Product>()
				.HasMany(e => e.Tags)
				.WithMany(e => e.Products)
				.Map(m => m.ToTable("Tags_Products").MapLeftKey("Product_Id"));

			modelBuilder.Entity<Question_Categories>()
				.HasMany(e => e.CommonQuestions)
				.WithRequired(e => e.Question_Categories)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Question_Categories>()
				.HasMany(e => e.CustomerQuestions)
				.WithRequired(e => e.Question_Categories)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Spec>()
				.HasMany(e => e.Carts)
				.WithRequired(e => e.Spec)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Tag>()
				.HasMany(e => e.Videos)
				.WithMany(e => e.Tags)
				.Map(m => m.ToTable("Tags_Video").MapLeftKey("TagId").MapRightKey("VideoId"));

			modelBuilder.Entity<VideoCategory>()
				.HasMany(e => e.Essays)
				.WithRequired(e => e.VideoCategory)
				.HasForeignKey(e => e.CategoryId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<VideoCategory>()
				.HasMany(e => e.Videos)
				.WithRequired(e => e.VideoCategory)
				.HasForeignKey(e => e.CategoryId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<VideoComment>()
				.HasMany(e => e.VCommentLikes)
				.WithRequired(e => e.VideoComment)
				.HasForeignKey(e => e.CommentId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Video>()
				.HasMany(e => e.VideoComments)
				.WithRequired(e => e.Video)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Video>()
				.HasMany(e => e.VideoLikes)
				.WithRequired(e => e.Video)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Video>()
				.HasOptional(e => e.VideoView)
				.WithRequired(e => e.Video);
		}
	}
}
