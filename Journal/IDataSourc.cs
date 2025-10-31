using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Controls;

namespace Journal
{
    internal interface IDataSourc
    {
        public static void Load(Magazine magazine)
        {
            string sql = "select * from passSubj where isArch = 'false'";
            SqlConnection sqlConnection = Header.SqlConnection;
            IDbCommand iDbCommand = new SqlCommand(sql, sqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            List<Subject> subjects = new List<Subject>();
            while (iDataReader.Read()) Add(iDataReader, subjects);
            iDataReader.Close();
            sql = "select * from currSubj";
            iDbCommand = new SqlCommand(sql, sqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) Add(iDataReader, subjects);
            iDataReader.Close();
            foreach (Subject subject in subjects)
            {
                sql = $"select * from groups where Id = {subject.GetGroupId()}";
                iDbCommand = new SqlCommand(sql, sqlConnection);
                iDataReader = iDbCommand.ExecuteReader();
                while (iDataReader.Read()) subject.Group = iDataReader
                        .GetString(1);
                iDataReader.Close();
            }
            magazine.studSubj.ItemsSource = subjects;
            sql = $"select * from currSubj where userId = {magazine.Identifier}";
            iDbCommand = new SqlCommand(sql, sqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            List<int> teamIds = new List<int>(), ids = new List<int>();
            while (iDataReader.Read())
            {
                int groupId = iDataReader.GetInt32(2);
                if (teamIds.Contains(groupId)) teamIds.Remove(groupId);
                teamIds.Add(groupId);
            }
            iDataReader.Close();
            iDbCommand = new SqlCommand(Header.SelFromGroupsWhereIsArchIsFalse, sqlConnection);
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
                    iDbCommand = new SqlCommand(SelFromGroups(id), sqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read()) Add(iDataReader, groupArr,
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
                iDbCommand = new SqlCommand(sql, sqlConnection);
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
                        sql = $"{SelFromSubj(table, lesson.GetSubjId())} and groupId = ({Header.SelFromGroupsWhereIsArchIsFalseAndNameIs(group)})";
                        iDbCommand = new SqlCommand(sql, sqlConnection);
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
                    Add(magazine.panel.Children, dataGrid,
                        button);
                }
                sql = "select * from checking";
                iDbCommand = new SqlCommand(sql, sqlConnection);
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
                    iDbCommand = new SqlCommand(sql, sqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read()) homework.User = iDataReader
                            .GetString(1);
                    iDataReader.Close();
                }
                foreach (Homework homework in works)
                {
                    sql = $"select * from homeworks where Id = {homework.GetHomId()}";
                    iDbCommand = new SqlCommand(sql, sqlConnection);
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
                    iDbCommand = new SqlCommand(sql, sqlConnection);
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
                        iDbCommand = new SqlCommand(SelFromSubj(table, homework.GetSubjId()), sqlConnection);
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
                    iDbCommand = new SqlCommand(SelFromGroups(homework.GetGroupId()), sqlConnection);
                    iDataReader = iDbCommand.ExecuteReader();
                    while (iDataReader.Read()) homework.Group = iDataReader
                            .GetString(1);
                    iDataReader.Close();
                }
                magazine.homeTasks.ItemsSource = actHom;
            }
        }
        public static void Load(EdStudList edStudList)
        {
            string sql = $"select * from users where groupId = {edStudList.Identifier}";
            SqlConnection sqlConnection = Header.SqlConnection;
            IDbCommand iDbCommand = new SqlCommand(sql, sqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            List<string> groupAppr = new List<string>(), freeAppr = new List<string>();
            while (iDataReader.Read()) groupAppr.Add(iDataReader
                .GetString(1));
            iDataReader.Close();
            sql = $"select * from users where isStud = 'true' and groupId is null";
            iDbCommand = new SqlCommand(sql, sqlConnection);
            iDataReader = iDbCommand.ExecuteReader();
            while (iDataReader.Read()) freeAppr.Add(iDataReader
                .GetString(1));
            iDataReader.Close();
            edStudList.apprInGroup.ItemsSource = groupAppr;
            edStudList.apprOutOfGroup.ItemsSource = freeAppr;
        }
        public static string SelFromUs(string apprentice) => $"select Id from users where username = '{apprentice}'";
        public static string SelFromCheck(int userId, int homId) => $"select Id from checking where userId = {userId} and homId = {homId}";
        static void Add(IDataReader dataReader, List<Subject> list)
        {
            Subject subject = new Subject(dataReader.GetInt32(0), dataReader.GetString(1),
                null, dataReader.GetInt32(2));
            list.Add(subject);
        }
        static string SelFromGroups(int? identifier) => $"select * from groups where Id = {identifier} and isArch = 'false'";
        static void Add(IDataReader dataReader, List<string>[] teams,
            int index, int shift)
        {
            string name = dataReader.GetString(1);
            teams[index].Add(name);
            teams[index + shift].Add(name);
        }
        static string SelFromSubj(string schedule, int? identifier) => $"select * from {schedule} where Id = {identifier}";
        static void Add(UIElementCollection uIElColl, DataGrid grid,
            Button key)
        {
            uIElColl.Add(grid);
            uIElColl.Add(key);
        }
        public static int? SelFromGroups(string team)
        {
            string sql = $"select * from groups where Id = ({Header.SelFromGroupsWhereIsArchIsFalseAndNameIs(team)})";
            IDbCommand iDbCommand = new SqlCommand(sql, Header.SqlConnection);
            IDataReader iDataReader = iDbCommand.ExecuteReader();
            int? id = null;
            while (iDataReader.Read()) id = iDataReader.GetInt32(0);
            iDataReader.Close();
            return id;
        }
    }
}