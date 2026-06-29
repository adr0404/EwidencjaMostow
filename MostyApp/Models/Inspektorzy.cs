using System;
using System.Collections.Generic;

namespace MostyApp.Models;

public partial class Inspektorzy
{
    public int InspektorId { get; set; }

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string NrUprawnien { get; set; } = null!;

    public int KategoriaUprawnienId { get; set; }

    public string Telefon { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual SlkategorieUprawnien KategoriaUprawnien { get; set; } = null!;

    public virtual ICollection<Przeglady> Przeglady { get; set; } = new List<Przeglady>();
}
