public class GeoRecord
{
    public int Id { get; set; }
    public string Serial { get; set; } = string.Empty;
    public bool Alert { get; set; }
    public string AlertType { get; set; } = string.Empty;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
