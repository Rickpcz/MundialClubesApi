public class Equipo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Logo { get; set; } = "";
    public string Pais { get; set; } = "";
    public int LigaId { get; set; } // FK
}