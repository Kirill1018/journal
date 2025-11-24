using Microsoft.Data.SqlClient;
using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для CreatGroup.xaml
    /// </summary>
    public partial class CreatGroup : Window
    {
        Magazine Magazine { get; set; }
        public CreatGroup(Magazine magazine)
        {
            InitializeComponent();
            this.Magazine = magazine;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string titleText = title.Text, sql = $"insert into groups(name, isArch) values('{titleText}', 'false')";
            if (titleText == string.Empty) return;
            if (Header.SelFromGroups().Contains(titleText))
            {
                Error error = new Error(Header.GroupAv);
                error.Show();
                return;
            }
            SqlCommand sqlCommand = new SqlCommand(sql, Header.SqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            Header.Load(this.Magazine);
            Close();
        }
    }
}