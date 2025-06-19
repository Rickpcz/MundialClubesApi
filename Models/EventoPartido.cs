public class EventoPartido
{
    public int Id { get; set; }
    public int PartidoId { get; set; }
    public string Tiempo { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Jugador { get; set; } = "";
    public string Detalle { get; set; } = "";
    public int EquipoId { get; set; }
}