using Microsoft.Data.SqlClient;
using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Task.xaml
    /// </summary>
    public partial class Task : Window
    {
        int Identifier { get; set; }
        public Task(int identifier)
        {
            InitializeComponent();
            this.Identifier = identifier;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string taskText = assignment.Text, termText = term.Text,
                    emptyString = string.Empty, sql = $"insert into homeworks(task, lessId, "
                    + $"deadline) values('{taskText}', {this.Identifier}, "
                    + $"'{termText}')";
                if (taskText == emptyString || termText == emptyString) return;
                SqlCommand sqlCommand = new SqlCommand(sql, Header.SqlConnection);
                await sqlCommand.ExecuteNonQueryAsync();
                Close();
            }
            catch (SqlException)
            {
                Error error = new Error("ошибка заполнения формы");
                error.Show();
            }
        }

        private void assignment_KeyDown(object sender, System.Windows.Input
            .KeyEventArgs e) { if (e.Key == System.Windows
                .Input.Key.Enter)
            {
                int index = assignment.SelectionStart;
                string text = assignment.Text;
                assignment.Text = text.Substring(0, index) + Environment
                    .NewLine + text.Substring(index);
                assignment.SelectionStart = text.Length + 1;
            }
        }
    }
}