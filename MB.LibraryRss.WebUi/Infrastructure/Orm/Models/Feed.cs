namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Feed
  {
    // Autogen pk's
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int FeedId { get; set; }

    public DateTime FeedLastUpdated { get; set; }

    public DateTime Inserted { get; set; }

    public virtual ICollection<Element> Elements { get; set; }
  }
}
