public class PartidoExternoDto
{
    public string IdEvento { get; set; } = string.Empty;
    public string Local { get; set; } = string.Empty;
    public string Visitante { get; set; } = string.Empty;
    public string LogoLocal { get; set; } = string.Empty;
    public string LogoVisitante { get; set; } = string.Empty;
    public int? GolesLocal { get; set; }
    public int? GolesVisitante { get; set; }
    public string Hora { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Liga { get; set; } = string.Empty;
    public string LigaLogo { get; set; } = string.Empty;
}
