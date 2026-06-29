using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MostyApp.Models; // Podpinamy modele

namespace MostyApp
{
    public partial class DodajObiektPage : Page
    {
        public DodajObiektPage()
        {
            InitializeComponent();
            ZaladujTypyKonstrukcji(); // Odpalamy ładowanie rozwijanej listy przy starcie
        }

        // Pobieramy ze słownika typy konstrukcji do rozwijanej listy
        private void ZaladujTypyKonstrukcji()
        {
            using (var db = new EwidencjaMostowContext())
            {
                // UWAGA: Sprawdź czy tabela nazywa się "SLTypyKonstrukcji" czy z małych liter np. "SltypyKonstrukcji" w Twoim folderze Models
                CmbTyp.ItemsSource = db.SltypyKonstrukcji.ToList();
                CmbTyp.DisplayMemberPath = "NazwaTypu";       // To widzi użytkownik
                CmbTyp.SelectedValuePath = "TypKonstrukcjiId"; // To idzie do bazy (ID)
            }
        }

        // Akcja po kliknięciu przycisku Zapisz
        private void BtnZapisz_Click(object sender, RoutedEventArgs e)
        {
            // Podstawowe zabezpieczenie, żeby ktoś nie dodał pustego
            if (string.IsNullOrWhiteSpace(TxtNazwa.Text) || CmbTyp.SelectedValue == null)
            {
                MessageBox.Show("Wypełnij wymagane pola (Nazwa i Typ)!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new EwidencjaMostowContext())
                {
                    // Tworzymy nowy klocek z danymi z formularza
                    var nowyObiekt = new Obiekty
                    {
                        NazwaObiektu = TxtNazwa.Text,
                        TypKonstrukcjiId = (int)CmbTyp.SelectedValue,
                        OpisLokalizacji = TxtLokalizacja.Text,
                        StatusEksploatacyjny = ((ComboBoxItem)CmbStatus.SelectedItem).Content.ToString(),
                        NosnoscEwidencyjna = decimal.Parse(TxtNosnosc.Text) // Zamienia tekst z okienka na liczbę
                    };

                    // Wrzucamy do bazy i zapisujemy zmiany
                    db.Obiekty.Add(nowyObiekt);
                    db.SaveChanges();

                    MessageBox.Show("Nowy obiekt został pomyślnie dodany do ewidencji!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Czyścimy okienka po udanym zapisie
                    TxtNazwa.Clear();
                    TxtLokalizacja.Clear();
                    TxtNosnosc.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu (sprawdź czy wpisałeś poprawną liczbę w nośności): " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}