using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Controls;

namespace Journal
{
    internal interface IDataSourc
    {
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
        public static void Add(IDataReader dataReader, List<Subject> list)
        {
            Subject subject = new Subject(dataReader.GetInt32(0), dataReader.GetString(1),
                null, dataReader.GetInt32(2));
            list.Add(subject);
        }
        public static string SelFromGroups(int? identifier) => $"select * from groups where Id = {identifier} and isArch = 'false'";
        public static void Add(IDataReader dataReader, List<string>[] teams,
            int index, int shift)
        {
            string name = dataReader.GetString(1);
            teams[index].Add(name);
            teams[index + shift].Add(name);
        }
        public static string SelFromSubj(string schedule, int? identifier) => $"select * from {schedule} where Id = {identifier}";
        public static void Add(UIElementCollection uIElColl, DataGrid grid,
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