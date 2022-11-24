using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public class SearchOptionsBuilder : IBuilder<SearchOptions>
{
  private readonly SearchOptions _searchOptions = new();

  public SearchOptions Build()
  {
    return _searchOptions;
  }

  public SearchOptionsBuilder AddDateTimeRange(DateTime? initial, DateTime? final)
  {
    _searchOptions.DateTimeRange = new Couple<DateTime>(initial, final);
    return this;
  }

  public SearchOptionsBuilder AddApplication(string application)
  {
    _searchOptions.Application = application;
    return this;
  }

  public SearchOptionsBuilder AddWriter(string writer)
  {
    _searchOptions.Writer = writer;
    return this;
  }

  public SearchOptionsBuilder AddItemTypes(params ItemTypes[] itemTypes)
  {
    _searchOptions.ItemTypes = itemTypes;
    return this;
  }
}