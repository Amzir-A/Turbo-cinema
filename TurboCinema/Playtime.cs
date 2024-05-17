using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

using System;

public class Playtime
{
    public DateTime DateTime { get; set; }
    public string? Room { get; set; }
    public List<List<Seat>> Seats { get; set; }
}
