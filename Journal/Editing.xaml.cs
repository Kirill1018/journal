using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Editing.xaml
    /// </summary>
    public partial class Editing : Window
    {
        Subject Subject { get; set; }
        Magazine Magazine { get; set; }
        public Editing(Subject subject, Magazine magazine)
        {
            InitializeComponent();
            this.Subject = subject;
            this.Magazine = magazine;
            title.Text = this.Subject
                .Name;
            teams.ItemsSource = Header.SelFromGroups();
            teams.Text = this.Subject
                .Group;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string titleText = title.Text, teamText = teams.Text;
            if (titleText == string.Empty) return;
            if (titleText == this.Subject.Name && teamText == this
                .Subject.Group)
            {
                Close();
                return;
            }
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
            foreach (string schedule in Header.Tables)
            {
                string sql = $"update {schedule} set name = '{titleText}', groupId = ({Header.SelFromGroupsWhereIsArchIsFalseAndNameIs(teamText)}) where Id = {this.Subject
                    .GetId()}";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                await sqlCommand.ExecuteNonQueryAsync();
            }
            Header.Load(this.Magazine);
            Close();
        }
    }
}