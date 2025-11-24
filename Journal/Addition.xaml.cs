using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Addition.xaml
    /// </summary>
    public partial class Addition : Window
    {
        Magazine Magazine { get; set; }
        public Addition(Magazine magazine)
        {
            InitializeComponent();
            this.Magazine = magazine;
            teams.ItemsSource = Header.SelFromGroups();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string titleText = title.Text, teamText = teams.Text,
                emptyString = string.Empty, sql = "insert into currSubj(name, groupId, "
                + $"userId) values('{titleText}', ({Header.SelFromGroupsWhereIsArchIsFalseAndNameIs(teamText)}), "
                + $"{this.Magazine.Identifier})";
            if (titleText == emptyString || teamText == emptyString) return;
            SqlConnection sqlConnection = Header.SqlConnection;
            IDbCommand iDbCommand = new SqlCommand(Header.SelFromCurrSubj(titleText, teamText), sqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            bool isAv = false;
            while (iDataReader.Read()) if (titleText == iDataReader.GetString(1)) isAv = true;
            iDataReader.Close();
            if (isAv)
            {
                Error error = new Error(Header.SubjAv);
                error.Show();
                return;
            }
            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            Header.Load(this.Magazine);
            Close();
        }
    }
}