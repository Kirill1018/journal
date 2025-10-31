using Microsoft.Data.SqlClient;
using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для EdStudList.xaml
    /// </summary>
    public partial class EdStudList : Window
    {
        static SqlConnection SqlConnection { get; } = Header.SqlConnection;
        public int? Identifier { get; set; }
        public EdStudList(int? identifier)
        {
            InitializeComponent();
            this.Identifier = identifier;
            IDataSourc.Load(this);
        }
        
        private async void OnClickExc(object sender, RoutedEventArgs e)
        {
            string? student = apprInGroup.SelectedItem?.ToString();
            string sql = $"update users set groupId = null where Id = ({IDataSourc.SelFromUs(student!)})";
            SqlCommand sqlCommand = new SqlCommand(sql, SqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            IDataSourc.Load(this);
        }

        private async void OnClickIncl(object sender, RoutedEventArgs e)
        {
            string? student = apprOutOfGroup.SelectedItem?.ToString();
            string sql = $"update users set groupId = {this.Identifier} where Id = ({IDataSourc.SelFromUs(student!)})";
            SqlCommand sqlCommand = new SqlCommand(sql, SqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            IDataSourc.Load(this);
        }
    }
}