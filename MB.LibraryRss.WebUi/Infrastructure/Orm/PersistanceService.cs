namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using AutoMapper;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Models;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Repos;
  using MB.LibraryRss.WebUi.Interfaces;

  using Newtonsoft.Json;

  public class PersistanceService : IPersistanceService
  {
    private readonly UnitOfWork uow;

    private readonly IMappingEngine mapper;

    public PersistanceService(UnitOfWork uow, IMappingEngine mapper)
    {
      this.uow = uow;
      this.mapper = mapper;
    }

    private ElementRepository Elements 
    { 
      get
      {
        return this.uow.ElementRepository;
      } 
    }

    private FeedRepository Feeds
    {
      get
      {
        return this.uow.FeedRepository;
      }
    }

    // todo: get this saving
    public void Save(List<TitleResult> results, DateTime lastUpdated)
    {
      // Clearing old data if we get a new feed. This means there will only be 0 or 1 record in the feeds table.
      // If we want to change that, we'll need to think about rolling old feeds out because the db has only ~20mb 
      // storage available.
      foreach (var feed in this.Feeds.FetchAll())
      {
        this.Feeds.Delete(feed);
      }

      var latestFeed = new Feed
        {
          Elements = results.Select(result => new Element { Data = JsonConvert.SerializeObject(result) }).ToList(),
          FeedLastUpdated = lastUpdated, 
          Inserted = DateTime.Now
        };

      this.Feeds.Insert(latestFeed);

      this.uow.Save();
    }

    public List<TitleResult> GetLatest()
    {
      var feed = this.Feeds.GetLatestFeed();

      return feed == null ? new List<TitleResult>() : feed.Elements.Select(this.mapper.Map<TitleResult>).ToList();
    }

    public DateTime? GetLastUpdated()
    {
      return this.Feeds.GetMaxFeedLastUpdated();
    }
  }
}
