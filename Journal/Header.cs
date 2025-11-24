using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Controls;

namespace Journal
{
    internal class Header
    {
        public static SqlConnection SqlConnection { get; } = new SqlConnection("Data Source=desktop-neuqaj1\\sqlexpress;Initial Catalog=\"electronic diary\";Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        public static string SubjAv { get; } = "этот предмет уже есть у данной группы";
        public static string GroupAv { get; } = "эта группа уже существует";
        public static string[] Tables { get; } = { "passSubj", "currSubj" };
        public static string SelFromGroupsWhereIsArchIsFalse { get; } = "select * from groups where isArch = 'false'";
        public static List<string> SelFromGroups()
        {
            IDbCommand iDbCommand = new SqlCommand(SelFromGroupsWhereIsArchIsFalse, SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            List<string> crews = new List<string>();
            while (iDataReader.Read()) crews.Add(iDataReader
                .GetString(1));
            iDataReader.Close();
            return crews;
        }
        public static string SelFromGroupsWhereIsArchIsFalseAndNameIs(string team) => $"select Id from groups where name = '{team}' and isArch = 'false'";
        public static string SelFromCurrSubj(string title, string team) => "select * from currSubj where Id = " + $"(select Id from currSubj where name = '{title}' and "
                + $"groupId = ({SelFromGroupsWhereIsArchIsFalseAndNameIs(team)}))";
        public static void Load(Magazine magazine)
        {
            string sql = "select * from passSubj where isArch = 'false'";
            IDbCommand iDbCommand = new SqlCommand(sql, SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            List<Subject> subjects = new List<Subject>();
            while (iDataReader.Read()) IDataSourc.Add(iDataReader, subjects);
            iDataReader.Close();
            sql = "select * from currSubj";
            iDbCommand = new SqlCommand(sql, SqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) IDataSourc.Add(iDataReader, subjects);
            iDataReader.Close();
            foreach (Subject subject in subjects)
            {
                sql = $"select * from groups where Id = {subject.GetGroupId()}";
                iDbCommand = new SqlCommand(sql, SqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                while (iDataReader.Read()) subject.Group = iDataReader
                        .GetString(1);
                iDataReader.Close();
            }
            magazine.studSubj.ItemsSource = subjects;
            sql = $"select * from currSubj where userId = {magazine.Identifier}";
            iDbCommand = new SqlCommand(sql, SqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            List<int> teamIds = new List<int>(), ids = new List<int>();
            while (iDataReader.Read())
            {
                int groupId = iDataReader.GetInt32(2);
                teamIds.Remove(groupId);
                teamIds.Add(groupId);
            }
            iDataReader.Close();
            iDbCommand = new SqlCommand(SelFromGroupsWhereIsArchIsFalse, SqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) ids.Add(iDataReader
                .GetInt32(0));
            iDataReader.Close();
            for (int i = 0; i < ids.Count; i++) foreach (int id in teamIds) if (id == ids[i]) ids.RemoveAt(i);
            List<int>[] idArr = { teamIds, ids };
            List<string> groups1 = new List<string>(), groups2 = new List<string>(),
                allGroups = new List<string>();
            List<string>[] groupArr = { groups1, groups2,
                allGroups };
            int sweep = groupArr.Length - 1;
            for (int i = 0; i < idArr.Length; i++)
            {
                foreach (int id in idArr[i])
                {
                    iDbCommand = new SqlCommand(IDataSourc.SelFromGroups(id), SqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read()) IDataSourc.Add(iDataReader, groupArr,
                        i, sweep);
                    iDataReader.Close();
                }
                sweep--;
            }
            magazine.actGroups.ItemsSource = groupArr
                .First();
            magazine.othGroups.ItemsSource = groupArr[1];
            magazine.panel.Children
                .Clear();
            foreach (string group in groupArr.Last())
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = group;
                magazine.panel.Children
                    .Add(textBlock1);
                List<Lesson> tutList = new List<Lesson>();
                sql = "select * from lessons where isPass = 'true'";
                iDbCommand = new SqlCommand(sql, SqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                while (iDataReader.Read())
                {
                    DateTime dateTime = iDataReader.GetDateTime(3);
                    Lesson lesson = new Lesson(iDataReader.GetInt32(0), $"{dateTime.Day}.{dateTime
                        .Month}.{dateTime.Year}",
                        dateTime, group,
                        null, iDataReader.GetInt32(2),
                        iDataReader.GetString(4));
                    tutList.Add(lesson);
                }
                iDataReader.Close();
                string[] tables = Header.Tables;
                List<string> subjNam = new List<string>();
                foreach (string table in tables) foreach (Lesson lesson in tutList)
                    {
                        sql = $"{IDataSourc.SelFromSubj(table, lesson.GetSubjId())} and groupId = ({SelFromGroupsWhereIsArchIsFalseAndNameIs(group)})";
                        iDbCommand = new SqlCommand(sql, SqlConnection);
                        iDataReader = iDbCommand.ExecuteReader();
                        while (iDataReader.Read())
                        {
                            string subjName = iDataReader.GetString(1);
                            for (int i = 0; i < subjNam.Count; i++) if (subjName == subjNam[i]) subjNam.Remove(subjName);
                            lesson.Subject = subjName;
                            subjNam.Add(subjName);
                        }
                        iDataReader.Close();
                    }
                foreach (string subjName in subjNam)
                {
                    TextBlock textBlock2 = new TextBlock();
                    textBlock2.Text = subjName;
                    magazine.panel.Children
                        .Add(textBlock2);
                    List<Lesson> lessBySubj = new List<Lesson>();
                    foreach (Lesson lesson in tutList) if (lesson.Subject == subjName) lessBySubj.Add(lesson);
                    IEnumerable<Lesson> iEnumerable = lessBySubj.OrderBy(lesson => lesson.GetDateTime());
                    DataGrid dataGrid = new DataGrid();
                    dataGrid.ItemsSource = iEnumerable;
                    Button button = new Button();
                    AddTask addTask = new AddTask(dataGrid);
                    button.Content = "написать домашнее задание для выбранного урока";
                    button.Click += addTask.Add;
                    IDataSourc.Add(magazine.panel
                        .Children, dataGrid,
                        button);
                }
                sql = "select * from checking";
                iDbCommand = new SqlCommand(sql, SqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                List<Homework> works = new List<Homework>();
                while (iDataReader.Read())
                {
                    DBNull dBNull = DBNull.Value;
                    byte[]? lodge = (iDataReader["binFile"] == dBNull) ? null : (byte[]?)iDataReader["binFile"];
                    string? filename = (iDataReader["lodgeName"] == dBNull) ? null : iDataReader.GetString(4), maintenance = (iDataReader["content"] == dBNull) ? null : iDataReader.GetString(5),
                        remark = (iDataReader["comment"] == dBNull) ? null : iDataReader.GetString(6);
                    int? assessment = (iDataReader["mark"] == dBNull) ? null : iDataReader.GetInt32(7);
                    Homework homework = new Homework(null, iDataReader.GetInt32(1),
                        iDataReader.GetInt32(2), null,
                        null, null,
                        null, null,
                        null, null,
                        null, lodge,
                        filename, maintenance,
                        remark, assessment);
                    if (homework.GetComment() is null && homework.GetMark() is null) works
                            .Add(homework);
                }
                iDataReader.Close();
                foreach (Homework homework in works)
                {
                    sql = $"select * from users where Id = {homework.GetUserId()}";
                    iDbCommand = new SqlCommand(sql, SqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read()) homework.User = iDataReader
                            .GetString(1);
                    iDataReader.Close();
                }
                foreach (Homework homework in works)
                {
                    sql = $"select * from homeworks where Id = {homework.GetHomId()}";
                    iDbCommand = new SqlCommand(sql, SqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read())
                    {
                        homework.SetTask(iDataReader.GetString(1));
                        homework.SetLessId(iDataReader.GetInt32(2));
                    }
                    iDataReader.Close();
                }
                foreach (Homework homework in works)
                {
                    sql = $"select * from lessons where Id = {homework.GetLessId()}";
                    iDbCommand = new SqlCommand(sql, SqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read())
                    {
                        DateTime dateTime = iDataReader.GetDateTime(3);
                        homework.Date = $"{dateTime.Day}.{dateTime.Month}.{dateTime
                            .Year}";
                        homework.SetSubjId(iDataReader.GetInt32(2));
                        homework.Theme = iDataReader.GetString(4);
                    }
                    iDataReader.Close();
                }
                List<Homework> actHom = new List<Homework>();
                foreach (string table in tables) foreach (Homework homework in works)
                    {
                        iDbCommand = new SqlCommand(IDataSourc.SelFromSubj(table, homework.GetSubjId()), SqlConnection);
                        iDataReader = iDbCommand.ExecuteReader();
                        while (iDataReader.Read())
                        {
                            homework.Subject = iDataReader.GetString(1);
                            homework.SetGroupId(iDataReader.GetInt32(2));
                            if ((int)iDataReader["userId"] == magazine.Identifier) actHom.Add(homework);
                        }
                        iDataReader.Close();
                    }
                foreach (Homework homework in actHom)
                {
                    iDbCommand = new SqlCommand(IDataSourc.SelFromGroups(homework.GetGroupId()), SqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read()) homework.Group = iDataReader
                            .GetString(1);
                    iDataReader.Close();
                }
                magazine.homeTasks.ItemsSource = actHom;
            }
        }
    }
}