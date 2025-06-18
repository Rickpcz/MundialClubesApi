using MundialClubesApi.Dtos;

public class TeamWrapper
{
    public TeamInfo Team { get; set; } = new();
    public CountryInfo Country { get; set; } = new();
}

public class TeamInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Logo { get; set; } = "";
}