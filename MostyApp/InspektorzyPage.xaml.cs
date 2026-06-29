using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MostyApp.Models;

namespace MostyApp
{
    public partial class InspektorzyPage : Page
    {
        public InspektorzyPage()
        {
            InitializeComponent();
            ZaladujKategorie();    // Ładuje listę rozwijaną
            ZaladujInspektorow();  // Ładuje tabelę po prawej
        }

        private void ZaladujKategorie()
        {
            using (var db = new EwidencjaMostowContext())
            {
                //
                CmbKategoria.ItemsSource = db.SlkategorieUprawnien.ToList();
                CmbKategoria.DisplayMemberPath = "NazwaKategorii";
                CmbKategoria.SelectedValuePath = "KategoriaUprawnienId";
            }
        }

        private void ZaladujInspektorow()
        {
            using (var db = new EwidencjaMostowContext())
            {
                var ladniInspektorzy = db.Inspektorzy.Select(i => new
                {
                    ID = i.InspektorId,
                    Imie = i.Imie,
                    Nazwisko = i.Nazwisko,
                    NrUprawnien = i.NrUprawnien,
                    Kategoria = i.KategoriaUprawnien.NazwaKategorii, // zamiast klucza obcego
                    Telefon = i.Telefon,
                    Email = i.Email
                }).ToList();

                InspektorzyDataGrid.ItemsSource = ladniInspektorzy;
            }
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzamy czy użytkownik wpisał chociaż imię, nazwisko i wybrał kategorię
            if (string.IsNullOrWhiteSpace(TxtImie.Text) || string.IsNullOrWhiteSpace(TxtNazwisko.Text) || CmbKategoria.SelectedValue == null)
            {
                MessageBox.Show("Wypełnij przynajmniej Imię, Nazwisko i wybierz kategorię uprawnień!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new EwidencjaMostowContext())
                {
                    var nowyInspektor = new Inspektorzy
                    {
                        Imie = TxtImie.Text,
                        Nazwisko = TxtNazwisko.Text,
                        NrUprawnien = TxtUprawnienia.Text,
                        KategoriaUprawnienId = (int)CmbKategoria.SelectedValue,
                        Telefon = TxtTelefon.Text,
                        Email = TxtEmail.Text
                    };

                    db.Inspektorzy.Add(nowyInspektor);
                    db.SaveChanges(); // Wysyła do bazy SQL

                    MessageBox.Show("Nowy inspektor został dodany!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Czyścimy pola tekstowe po sukcesie
                    TxtImie.Clear();
                    TxtNazwisko.Clear();
                    TxtUprawnienia.Clear();
                    TxtTelefon.Clear();
                    TxtEmail.Clear();

                    // Odświeżamy tabelę, żeby nowy wpis od razu się w niej pojawił
                    ZaladujInspektorow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu (np. brak @ w adresie email, bo masz taki wymóg w bazie): " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}