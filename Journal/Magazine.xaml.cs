using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Magazine.xaml
    /// </summary>
    public partial class Magazine : Page
    {
        static SqlConnection SqlConnection { get; } = Header.SqlConnection;
        public int? Identifier { get; set; }
        public Magazine(int? identifier)
        {
            InitializeComponent();
            this.Identifier = identifier;
            Header.Load(this);
        }
        
        private void OnClickAddNewSubj(object sender, RoutedEventArgs e)
        {
            Addition addition = new Addition(this);
            addition.Show();
        }

        private void OnClickEdChosSubj(object sender, RoutedEventArgs e)
        {
            try
            {
                Subject subject = (Subject)studSubj.SelectedItem;
                Editing editing = new Editing(subject, this);
                editing.Show();
            }
            catch (NullReferenceException) { }
        }

        private async void OnClickTranslChosSubjToArchStat(object sender, RoutedEventArgs e)
        {
            Subject? subject = (Subject?)studSubj.SelectedItem;
            if (subject is null) return;
            string sql = "select * from currSubj";
            IDbCommand iDbCommand = new SqlCommand(sql, SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            bool isCurr = false;
            int id = subject.GetId();
            while (iDataReader.Read()) if (iDataReader.GetInt32(0) == id) isCurr = true;
            iDataReader.Close();
            if (isCurr)
            {
                Error error = new Error("предмет должен быть пройденным");
                error.Show();
                return;
            }
            sql = $"update passSubj set isArch = 'true' where Id = {id}";
            SqlCommand sqlCommand = new SqlCommand(sql, SqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            Header.Load(this);
        }

        private void OnClickCreatGroup(object sender, RoutedEventArgs e)
        {
            CreatGroup creatGroup = new CreatGroup(this);
            creatGroup.Show();
        }

        private void OnClickEdChosGroup(object sender, RoutedEventArgs e)
        {
            try
            {
                string? group = actGroups.SelectedItem.ToString();
                int? id = IDataSourc.SelFromGroups(group!);
                EdGroup edGroup = new EdGroup(id, group!,
                    this);
                edGroup.Show();
            }
            catch (NullReferenceException) { }
        }

        private void OnClickEdListOfStudOfChosGroup(object sender, RoutedEventArgs e)
        {
            try
            {
                string? group = actGroups.SelectedItem.ToString();
                int? id = IDataSourc.SelFromGroups(group!);
                EdStudList edStudList = new EdStudList(id);
                edStudList.Show();
            }
            catch (NullReferenceException) { }
        }

        private async void OnClickChangStatOfChosGroupToArchOne(object sender, RoutedEventArgs e)
        {
            string? group = actGroups.SelectedItem?.ToString();
            string sql = $"update groups set isArch = 'true' where Id = ({Header.SelFromGroupsWhereIsArchIsFalseAndNameIs(group!)})";
            SqlCommand sqlCommand = new SqlCommand(sql, SqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            Header.Load(this);
        }

        private void OnClickCheckChosWork(object sender, RoutedEventArgs e)
        {
            try
            {
                Homework homework = (Homework)homeTasks.SelectedItem;
                if (homework.GetBinFile() is null)
                {
                    Work work = new Work(homework);
                    work.Show();
                }
                else
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = homework.GetLodgeName();
                    if (sfd.ShowDialog() == true) File.WriteAllBytes(sfd
                        .FileName!, homework.GetBinFile()!);
                }
            }
            catch (NullReferenceException) { }
        }

        private void OnClickLookAtTaskForChosWork(object sender, RoutedEventArgs e)
        {
            try
            {
                Homework homework = (Homework)homeTasks.SelectedItem;
                Target target = new Target(homework);
                target.Show();
            }
            catch (NullReferenceException) { }
        }

        private void OnClickEvChosWork(object sender, RoutedEventArgs e)
        {
            Homework? homework = (Homework?)homeTasks.SelectedItem;
            if (homework is null) return;
            Feedback feedback = new Feedback(homework, this);
            feedback.Show();
        }
    }
}