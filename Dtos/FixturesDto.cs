public class FixtureWrapper
{
    public FixtureInfo Fixture { get; set; } = new();
    public TeamInfoLocalVisitante Teams { get; set; } = new();
    public GoalsInfo Goals { get; set; } = new();
}

public class FixtureInfo
{
    public int Id { get; set; }
    public string Date { get; set; } = "";
    public StatusInfo Status { get; set; } = new(); 
}


public class TeamInfoLocalVisitante
{
    public TeamInfo Home { get; set; } = new();
    public TeamInfo Away { get; set; } = new();
}

public class GoalsInfo
{
    public int? Home { get; set; }
    public int? Away { get; set; }
}
public class StatusInfo
{
    public string Long { get; set; } = "";
    public string Short { get; set; } = "";
    public int? Elapsed { get; set; }
}
