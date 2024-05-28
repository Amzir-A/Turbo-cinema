public class Seat
{
    public string ID { get; set; }
    public bool IsAvailable { get; set; }

    public Seat(string id, bool isAvailable)
    {
        ID = id;
        IsAvailable = isAvailable;
    }
}
