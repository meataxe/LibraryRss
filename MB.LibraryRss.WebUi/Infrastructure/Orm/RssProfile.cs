namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using AutoMapper;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Infrastructure.Helpers;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Models;

  using Newtonsoft.Json;

  public class RssProfile : Profile
  {
    protected override void Configure()
    {
      Mapper.CreateMap<Element, TitleResult>().ConstructUsing(GetTitleResult).IgnoreAllNonExisting();
    }

    private static TitleResult GetTitleResult(Element element)
    {
      return JsonConvert.DeserializeObject<TitleResult>(element.Data);
    }
  }
}
