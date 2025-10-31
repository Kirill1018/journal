using Microsoft.Data.SqlClient;
using System.Data;

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
    }
}