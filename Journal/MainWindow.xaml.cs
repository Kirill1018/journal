using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Navigation;

namespace Journal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection sqlConnection = Header.SqlConnection;
                await sqlConnection.CloseAsync();
                await sqlConnection.OpenAsync();
                string sql = "select * from users";
                IDbCommand iDbCommand = new SqlCommand(sql, sqlConnection);
                IDataReader iDataReader = iDbCommand.ExecuteReader();
                int? id = null;
                bool isEnt = false;
                while (iDataReader.Read()) if (iDataReader.GetString(1) == user
                        .Text && iDataReader.GetString(2) == parole.Text
                        && iDataReader.GetBoolean(4))
                    {
                        id = iDataReader.GetInt32(0);
                        isEnt = true;
                    }
                iDataReader.Close();                
                if (isEnt)
                {
                    NavigationWindow navigationWindow = new NavigationWindow();
                    navigationWindow.Content = new Magazine(id);
                    navigationWindow.Show();
                    Close();
                }
            }
            catch (TaskCanceledException) { }
        }
    }
}