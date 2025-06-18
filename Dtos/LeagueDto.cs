namespace MundialClubesApi.Dtos;

public class LeagueWrapper
{
    public LeagueInfo League { get; set; } = new();
    public CountryInfo Country { get; set; } = new();
    public List<SeasonInfo> Seasons { get; set; } = new();
}

public class LeagueInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Logo { get; set; } = "";
}

public class CountryInfo
{
    public string Name { get; set; } = "";
    public string? Code { get; set; }
    public string? Flag { get; set; }
}

public class SeasonInfo
{
    public int Year { get; set; }
    public string Start { get; set; } = "";
    public string End { get; set; } = "";
    public bool Current { get; set; }
    public Coverage Coverage { get; set; } = new();
}

public class Coverage
{
    public Fixtures Fixtures { get; set; } = new();
    public bool Standings { get; set; }
    public bool Players { get; set; }
    public bool TopScorers { get; set; }
    public bool TopAssists { get; set; }
    public bool TopCards { get; set; }
    public bool Injuries { get; set; }
    public bool Predictions { get; set; }
    public bool Odds { get; set; }
}

public class Fixtures
{
    public bool Events { get; set; }
    public bool Lineups { get; set; }
    public bool Statistics_Fixtures { get; set; }
    public bool Statistics_Players { get; set; }
}
