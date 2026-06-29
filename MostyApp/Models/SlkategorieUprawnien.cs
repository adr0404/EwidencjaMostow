using System;
using System.Collections.Generic;

namespace MostyApp.Models;

public partial class SlkategorieUprawnien
{
    public int KategoriaUprawnienId { get; set; }

    public string NazwaKategorii { get; set; } = null!;

    public virtual ICollection<Inspektorzy> Inspektorzy { get; set; } = new List<Inspektorzy>();
}
