public class ResumenTemporada
{
    public int Id { get; set; }
    public int LigaId { get; set; }
    public string? LigaNombre { get; set; }
    public int Temporada { get; set; }
    public string? Campeon { get; set; }
    public string? Goleador { get; set; }
    public int Goles { get; set; }
    public string? Asistidor { get; set; }
    public int Asistencias { get; set; }
    public string? UltimoPartido { get; set; }
    public string? Resultado { get; set; }
    public DateTime CreadoEn { get; set; }
}
