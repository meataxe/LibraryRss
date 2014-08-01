namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using System.Data.Entity;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Models;

  public class RssEntities : DbContext
  {
    public RssEntities(IConnectionService connectionStringService)
      : base(connectionStringService.NameOrConnectionString())     
    {      
    }

    public DbSet<Feed> Feeds { get; set; }

    public DbSet<Element> Elements { get; set; }

    public DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      // Explicit naming
      modelBuilder.Entity<Feed>().ToTable("Feeds");
      modelBuilder.Entity<Element>().ToTable("Elements");
      modelBuilder.Entity<Setting>().ToTable("Settings");
    }
  }
}
