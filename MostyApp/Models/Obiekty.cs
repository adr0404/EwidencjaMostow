using System;
using System.Collections.Generic;

namespace MostyApp.Models;

public partial class Obiekty
{
    public int ObiektId { get; set; }

    public string NazwaObiektu { get; set; } = null!;

    public int TypKonstrukcjiId { get; set; }

    public string OpisLokalizacji { get; set; } = null!;

    public string StatusEksploatacyjny { get; set; } = null!;

    public decimal NosnoscEwidencyjna { get; set; }

    public virtual ICollection<Przeglady> Przeglady { get; set; } = new List<Przeglady>();

    public virtual SltypyKonstrukcji TypKonstrukcji { get; set; } = null!;
}
