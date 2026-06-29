using System;
using System.Collections.Generic;

namespace MostyApp.Models;

public partial class Przeglady
{
    public int PrzegladId { get; set; }

    public int ObiektId { get; set; }

    public int InspektorId { get; set; }

    public int TypPrzegladuId { get; set; }

    public DateOnly DataWykonania { get; set; }

    public DateOnly DataWaznosci { get; set; }

    public int StanTechniczny { get; set; }

    public decimal ZatwierdzonaNosnosc { get; set; }

    public decimal KosztRozliczeniowy { get; set; }

    public DateTime DataWpisuDoSystemu { get; set; }

    public string? Uwagi { get; set; }

    public virtual Inspektorzy Inspektor { get; set; } = null!;

    public virtual Obiekty Obiekt { get; set; } = null!;

    public virtual SltypyPrzegladow TypPrzegladu { get; set; } = null!;
}
