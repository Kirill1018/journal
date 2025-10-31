namespace Journal
{
    internal class Lesson
    {
        int Id { get; set; }
        public string? Date { get; set; }
        DateTime DateTime { get; set; } = new DateTime();
        public string? Group { get; set; }
        public string? Subject { get; set; }
        int SubjId { get; set; }
        public string? Theme { get; set; }
        public Lesson(int id, string? date,
            DateTime dateTime, string? group,
            string? subject, int subjId,
            string? theme)
        {
            Id = id;
            Date = date;
            DateTime = dateTime;
            Group = group;
            Subject = subject;
            SubjId = subjId;
            Theme = theme;
        }
        public int GetId() => this.Id;
        public DateTime GetDateTime() => this.DateTime;
        public int GetSubjId() => this.SubjId;
    }
}