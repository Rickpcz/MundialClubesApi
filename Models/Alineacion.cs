public class Alineacion
{
    public int Id { get; set; }
    public int PartidoId { get; set; }
    public int EquipoId { get; set; }
    public string Formacion { get; set; } = "";
    public List<JugadorAlineacion> Jugadores { get; set; } = new();
}