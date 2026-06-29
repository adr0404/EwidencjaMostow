using System;
using System.Collections.Generic;

namespace MostyApp.Models;

public partial class SltypyPrzegladow
{
    public int TypPrzegladuId { get; set; }

    public string NazwaTypu { get; set; } = null!;

    public int WaznoscWmiesiacach { get; set; }

    public decimal KosztEwidencyjny { get; set; }

    public virtual ICollection<Przeglady> Przeglady { get; set; } = new List<Przeglady>();
}
