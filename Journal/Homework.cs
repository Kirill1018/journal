namespace Journal
{
    public class Homework
    {
        public string? User { get; set; }
        int UserId { get; set; }
        int HomId { get; set; }
        string? Task { get; set; }
        int? LessId { get; set; }
        public string? Date { get; set; }
        public string? Group { get; set; }
        int? GroupId { get; set; }
        public string? Subject { get; set; }
        int? SubjId { get; set; }
        public string? Theme { get; set; }
        byte[]? BinFile { get; set; }
        string? LodgeName { get; set; }
        string? Content { get; set; }
        string? Comment { get; set; }
        int? Mark { get; set; }
        public Homework(string? user, int userId,
            int homId, string? task,
            int? lessId, string? date,
            string? group, int? groupId,
            string? subject, int? subjId,
            string? theme, byte[]? binFile,
            string? lodgeName, string? content,
            string? comment, int? mark)
        {
            User = user;
            UserId = userId;
            HomId = homId;
            Task = task;
            LessId = lessId;
            Date = date;
            Group = group;
            GroupId = groupId;
            Subject = subject;
            SubjId = subjId;
            Theme = theme;
            BinFile = binFile;
            LodgeName = lodgeName;
            Content = content;
            Comment = comment;
            Mark = mark;
        }
        public int GetUserId() => this.UserId;
        public int GetHomId() => this.HomId;
        public string? GetTask() => this.Task;
        public void SetTask(string task) => this.Task = task;
        public int? GetLessId() => this.LessId;
        public void SetLessId(int lessId) => this.LessId = lessId;
        public int? GetGroupId() => this.GroupId;
        public void SetGroupId(int groupId) => this.GroupId = groupId;
        public int? GetSubjId() => this.SubjId;
        public void SetSubjId(int subjId) => this.SubjId = subjId;
        public byte[]? GetBinFile() => this.BinFile;
        public string? GetLodgeName() => this.LodgeName;
        public string? GetCont() => this.Content;
        public string? GetComment() => this.Comment;
        public int? GetMark() => this.Mark;
    }
}