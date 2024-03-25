using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Seat
{
    public string Id { get; set; }
    public bool IsReserved { get; set; }
    public bool InitiallyReserved { get; private set; } // Tracks the initial reservation state

    public Seat(string id)
    {
        Id = id;
        IsReserved = false;
        InitiallyReserved = false; // Default to false
    }

    public void SetInitiallyReserved()
    {
        InitiallyReserved = IsReserved; // Called after loading from file
    }
}