using System.Windows;

namespace MostyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 
        }

        // Funkcja która odpali się po kliknięciu "Lista Obiektów" w lewym menu
        private void BtnLista_Click(object sender, RoutedEventArgs e)
        {
            //załaduje nową stronę do naszej ramki
            MainFrame.Content = new ListaObiektowPage();
        }

        //Funkcjaktóra odpali się po kliknięciu "Dodaj Obiekt"
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            // Ta linijka załaduje nam formularz do wpisywania nowych danych
            MainFrame.Content = new DodajObiektPage();
        }

        //Funkcjaktóra odpali się po kliknięciu "Inspektorzy"
        private void BtnInspektorzy_Click(object sender, RoutedEventArgs e)
        {
            //załaduje nam stronę z listą inspektorów
            MainFrame.Content = new InspektorzyPage();
        }

        private void BtnPrzeglady_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PrzegladyPage();
        }
    }
}