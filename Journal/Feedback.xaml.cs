using Microsoft.Data.SqlClient;
using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Feedback.xaml
    /// </summary>
    public partial class Feedback : Window
    {
        Homework Homework { get; set; }
        Magazine Magazine { get; set; }
        public Feedback(Homework homework, Magazine magazine)
        {
            InitializeComponent();
            this.Homework = homework;
            this.Magazine = magazine;
            List<int> assessments = new List<int>();
            for (int i = 1; i <= 12; i++) assessments.Add(i);
            assessment.ItemsSource = assessments;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string remText = remark.Text, emptyString = string.Empty,
                subQuery = IDataSourc.SelFromCheck(this.Homework
                .GetUserId(), this.Homework.GetHomId()), sql = $"update checking set comment = '{remText}' where Id = ({subQuery})",
                markText = assessment.Text;
            bool isRemFill = true, isMarkFill = true;
            SqlConnection sqlConnection = Header.SqlConnection;
            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
            if (remText == emptyString) isRemFill = false;
            if (markText == emptyString) isMarkFill = false;
            if (isRemFill) await sqlCommand.ExecuteNonQueryAsync();
            if (isMarkFill)
            {
                sql = $"update checking set mark = {int.Parse(markText)} where Id = ({subQuery})";
                sqlCommand = new SqlCommand(sql, sqlConnection);
                await sqlCommand.ExecuteNonQueryAsync();
            }
            IDataSourc.Load(this.Magazine);
            Close();
        }
    }
}