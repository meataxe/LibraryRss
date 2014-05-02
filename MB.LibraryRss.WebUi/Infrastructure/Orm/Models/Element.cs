namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Models
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Element
  {
    // Autogen pk's
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int ElementId { get; set; }

    [ForeignKey("Feed")]
    public int FeedId { get; set; }

    public virtual Feed Feed { get; set; }

    public string Data { get; set; }
  }
}
