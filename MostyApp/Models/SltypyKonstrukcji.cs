using System;
using System.Collections.Generic;

namespace MostyApp.Models;

public partial class SltypyKonstrukcji
{
    public int TypKonstrukcjiId { get; set; }

    public string NazwaTypu { get; set; } = null!;

    public virtual ICollection<Obiekty> Obiekty { get; set; } = new List<Obiekty>();
}
