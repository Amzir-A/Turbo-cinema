public abstract class PaymentProcess
{
    public List<Seat> SelectedSeats { get; set; }
    public Movie SelectedMovie { get; set; }
    public Playtime SelectedPlaytime { get; set; }
    public List<(string, int, decimal)> SelectedFoodAndDrinks { get; set; }
    public Customer Customer { get; set; }
    
    public abstract int CalculateTotalPrice();
    public abstract void DisplayPaymentScreen();
    public abstract void ProcessPayment(int totalPrice, Customer customer);
}
