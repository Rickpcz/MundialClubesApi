public class JugadorAlineacion
{
    public int Id { get; set; }
    public int AlineacionId { get; set; }
    public Jugador Jugador { get; set; } = new();
    public string Posicion { get; set; } = "";
}
