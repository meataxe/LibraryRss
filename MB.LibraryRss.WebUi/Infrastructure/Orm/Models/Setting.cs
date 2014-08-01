namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Models
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Setting
  {
    // Autogen pk's
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Class { get; set; }

    public string Value { get; set; }
  }
}
