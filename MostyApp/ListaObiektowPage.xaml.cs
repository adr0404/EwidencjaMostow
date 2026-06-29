using System.Linq;
using System.Windows.Controls;
using MostyApp.Models; // podpinamy nasze modele z bazy

namespace MostyApp
{
    /// <summary>
    /// Logika interakcji dla strony ListaObiektowPage.xaml
    /// </summary>
    public partial class ListaObiektowPage : Page
    {
        public ListaObiektowPage()
        {
            InitializeComponent();
            ZaladujDane(); // Odpala się od razu po wejściu na tę zakładkę
        }

        // funkcja pobierająca dane
        private void ZaladujDane()
        {
            using (var db = new EwidencjaMostowContext())
            {
                // Używamy "Select", żeby stworzyć własne, ładne kolumny w locie
                var ladneDane = db.Obiekty.Select(o => new
                {
                    ID = o.ObiektId,
                    Nazwa = o.NazwaObiektu,
                    Typ = o.TypKonstrukcji.NazwaTypu, // zamiast ID, pobieramy gotowy tekst z innej tabeli
                    Lokalizacja = o.OpisLokalizacji,
                    Status = o.StatusEksploatacyjny,
                    Nosnosc = o.NosnoscEwidencyjna + " ton" // Dodajemy słówko dla lepszego efektu
                }).ToList();

                ObiektyDataGrid.ItemsSource = ladneDane;
            }
        }
    }
}