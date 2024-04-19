public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Postcode { get; set; }
    public List<Reservation> Reservations { get; set; }

    public Customer()
    {
        Reservations = new List<Reservation>();
    }
}
