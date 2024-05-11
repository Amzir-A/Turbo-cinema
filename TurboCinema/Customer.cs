public class Customer
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Postcode { get; set; }
    public string Password { get; set; }
    public List<Reservation> Reservations { get; set; }

    public Customer()
    {
        Reservations = new List<Reservation>();
    }
}

