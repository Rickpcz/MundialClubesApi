public class Partido
{
    public int Id { get; set; }
    public int LigaId { get; set; }
    public DateTime Fecha { get; set; }
    public int EquipoLocalId { get; set; }
    public int EquipoVisitanteId { get; set; }
    public int? GolesLocal { get; set; }
    public int? GolesVisitante { get; set; }
    public StatusInfo Estado { get; set; } = new();

}
