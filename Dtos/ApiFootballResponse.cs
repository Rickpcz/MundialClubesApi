using System.Text.Json;

namespace MundialClubesApi.Dtos;

public class ApiFootballResponse<T>
{
    public string Get { get; set; } = "";
    public object? Parameters { get; set; }
    public object? Errors { get; set; }
    public int Results { get; set; }
    public PagingInfo Paging { get; set; } = new();
    public List<T> Response { get; set; } = new();
}

public class PagingInfo
{
    public int Current { get; set; }
    public int Total { get; set; }
}
