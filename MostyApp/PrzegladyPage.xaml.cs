using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MostyApp.Models;

namespace MostyApp
{
    public partial class PrzegladyPage : Page
    {
        public PrzegladyPage()
        {
            InitializeComponent();
            ZaladujListyRozwijane();
            ZaladujHistorie();
        }

        private void ZaladujListyRozwijane()
        {
            using (var db = new EwidencjaMostowContext())
            {
                // Obiekty
                CmbObiekt.ItemsSource = db.Obiekty.ToList();
                CmbObiekt.DisplayMemberPath = "NazwaObiektu";
                CmbObiekt.SelectedValuePath = "ObiektId";

                // Inspektorzy (Imię + Nazwisko)
                var listaInspektorow = db.Inspektorzy.Select(i => new {
                    ID = i.InspektorId,
                    PelneImie = i.Imie + " " + i.Nazwisko
                }).ToList();

                CmbInspektor.ItemsSource = listaInspektorow;
                CmbInspektor.DisplayMemberPath = "PelneImie";
                CmbInspektor.SelectedValuePath = "ID";

                // Typy Przeglądów (Sprawdź czy nazywa się SltypyPrzegladow)
                CmbTypPrzegladu.ItemsSource = db.SltypyPrzegladow.ToList();
                CmbTypPrzegladu.DisplayMemberPath = "NazwaTypu";
                CmbTypPrzegladu.SelectedValuePath = "TypPrzegladuId";
            }
        }

        private void ZaladujHistorie()
        {
            using (var db = new EwidencjaMostowContext())
            {
                var historia = db.Przeglady.Select(p => new
                {
                    Data = p.DataWykonania.ToString("dd.MM.yyyy"),
                    Ważność = p.DataWaznosci.ToString("dd.MM.yyyy"),
                    Obiekt = p.Obiekt.NazwaObiektu,
                    Inspektor = p.Inspektor.Nazwisko,
                    Stan = p.StanTechniczny,
                    Koszt = p.KosztRozliczeniowy + " zł",
                    Uwagi = p.Uwagi
                }).ToList();

                PrzegladyDataGrid.ItemsSource = historia;
            }
        }

        private void BtnZapisz_Click(object sender, RoutedEventArgs e)
        {
            // Walidacja podstawowa
            if (CmbObiekt.SelectedValue == null || CmbInspektor.SelectedValue == null || CmbTypPrzegladu.SelectedValue == null ||
                DpDataWykonania.SelectedDate == null || DpDataWaznosci.SelectedDate == null ||
                string.IsNullOrWhiteSpace(TxtZatwierdzonaNosnosc.Text) || string.IsNullOrWhiteSpace(TxtKoszt.Text))
            {
                MessageBox.Show("Wypełnij wszystkie wymagane pola i daty!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Walidacja z bazą (Data ważności musi być późniejsza)
            if (DpDataWaznosci.SelectedDate.Value <= DpDataWykonania.SelectedDate.Value)
            {
                MessageBox.Show("Data ważności przeglądu musi być późniejsza niż data jego wykonania!", "Błąd logiki", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var db = new EwidencjaMostowContext())
                {
                    var nowyPrzeglad = new Przeglady
                    {
                        ObiektId = (int)CmbObiekt.SelectedValue,
                        InspektorId = (int)CmbInspektor.SelectedValue,
                        TypPrzegladuId = (int)CmbTypPrzegladu.SelectedValue,

                        // ZWYKŁE, PROSTE PRZYPISANIE DATY:
                        DataWykonania = DateOnly.FromDateTime(DpDataWykonania.SelectedDate.Value),
                        DataWaznosci = DateOnly.FromDateTime(DpDataWaznosci.SelectedDate.Value),

                        StanTechniczny = int.Parse(((ComboBoxItem)CmbStan.SelectedItem).Content.ToString()),
                        ZatwierdzonaNosnosc = decimal.Parse(TxtZatwierdzonaNosnosc.Text),
                        KosztRozliczeniowy = decimal.Parse(TxtKoszt.Text),
                        DataWpisuDoSystemu = DateTime.Now,
                        Uwagi = TxtUwagi.Text
                    };

                    db.Przeglady.Add(nowyPrzeglad);
                    db.SaveChanges();

                    MessageBox.Show("Zarejestrowano nowy przegląd!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Czyszczenie
                    TxtUwagi.Clear();
                    TxtKoszt.Clear();
                    TxtZatwierdzonaNosnosc.Clear();
                    DpDataWaznosci.SelectedDate = null;
                    DpDataWykonania.SelectedDate = null;

                    ZaladujHistorie();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu (upewnij się, że w nośności i koszcie wpisałeś poprawne liczby z przecinkiem): " + ex.InnerException?.Message ?? ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzamy czy coś jest zaznaczone
            if (PrzegladyDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Najpierw kliknij na wiersz w tabeli, który chcesz usunąć!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Pytamy o potwierdzenie
            if (MessageBox.Show("Czy na pewno chcesz usunąć ten przegląd?", "Potwierdź", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new EwidencjaMostowContext())
                    {
                        // Musimy wyciągnąć ID z zaznaczonego wiersza
                        // Uwaga: Twoja tabela w DataGridzie wyświetla dane w formacie anonimowym, 
                        // więc najpierw musimy znaleźć przegląd w bazie po unikalnych danych
                        dynamic zaznaczony = PrzegladyDataGrid.SelectedItem;
                        string dataPrzegladu = zaznaczony.Data;
                        string uwagi = zaznaczony.Uwagi;

                        var doUsuniecia = db.Przeglady.FirstOrDefault(p =>
                            p.DataWpisuDoSystemu.ToString() == dataPrzegladu || p.Uwagi == uwagi);

                        if (doUsuniecia != null)
                        {
                            db.Przeglady.Remove(doUsuniecia);
                            db.SaveChanges();
                            ZaladujHistorie(); // Odśwież tabelę
                            MessageBox.Show("Usunięto pomyślnie.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd usuwania: " + ex.Message);
                }
            }
        }
    }
}