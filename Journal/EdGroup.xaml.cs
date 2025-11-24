using Microsoft.Data.SqlClient;
using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для EdGroup.xaml
    /// </summary>
    public partial class EdGroup : Window
    {
        int? Identifier { get; set; }
        string GroupBeh { get; set; }
        Magazine Magazine { get; set; }
        public EdGroup(int? identifier, string groupBeh,
            Magazine magazine)
        {
            InitializeComponent();
            this.Identifier = identifier;
            this.GroupBeh = groupBeh;
            this.Magazine = magazine;
            title.Text = this.GroupBeh;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string titleText = title.Text, sql = $"update groups set name = '{titleText}' where Id = {this.Identifier}";
            if (titleText == string.Empty) return;
            if (titleText == this.GroupBeh)
            {
                Close();
                return;
            }
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