namespace MundialClubesApi.Dtos
{

public class LineupDto
{
    public TeamMiniDto Team { get; set; } = new();
    public string Formation { get; set; } = "";
    public CoachDto Coach { get; set; } = new();
    public List<PlayerWrapper> StartXI { get; set; } = new();
}


    public class TeamMiniDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Logo { get; set; } = "";
    }

    public class CoachDto
    {
        public string Name { get; set; } = "";
        public string Photo { get; set; } = "";
    }

    public class PlayerWrapper
    {
        public PlayerInfo Player { get; set; } = new();
    }

    public class PlayerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Number { get; set; }
        public string Pos { get; set; } = "";
        public string Photo { get; set; } = "";
    }

    public class StatisticDto
    {
        public TeamMiniDto Team { get; set; } = new();
        public List<StatEntry> Statistics { get; set; } = new();
    }

    public class StatEntry
    {
        public string Type { get; set; } = "";
        public object? Value { get; set; }
    }

    public class EventoDto
    {
        public TimeDto Time { get; set; } = new();
        public TeamMiniDto Team { get; set; } = new();
        public PlayerMiniDto Player { get; set; } = new();
        public string Type { get; set; } = "";
        public string Detail { get; set; } = "";
    }

    public class PlayerMiniDto
    {
        public string Name { get; set; } = "";
    }

    public class TimeDto
    {
        public int Elapsed { get; set; }
        public int? Extra { get; set; }
    }
}
