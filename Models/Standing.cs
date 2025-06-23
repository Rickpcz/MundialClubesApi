public class Standing
{
    public int Id { get; set; }
    public int LigaId { get; set; }
    public int Temporada { get; set; }
    public int EquipoId { get; set; }
    public string NombreEquipo { get; set; } = "";
    public string Logo { get; set; } = "";
    public int Posicion { get; set; }
    public int Puntos { get; set; }
    public int PartidosJugados { get; set; }
    public int Ganados { get; set; }
    public int Empatados { get; set; }
    public int Perdidos { get; set; }
    public int GolesFavor { get; set; }
    public int GolesContra { get; set; }
    public string Grupo { get; set; } = "";
    public DateTime FechaActualizacion { get; set; }
}
