using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Seat
{
    public string Id { get; set; }
    public bool IsReserved { get; set; }

    public Seat(string id)
    {
        Id = id;
        IsReserved = false;
    }
}