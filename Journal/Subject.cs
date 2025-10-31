namespace Journal
{
    public class Subject
    {
        int Id { get; set; }
        public string? Name { get; set; }
        public string? Group { get; set; }
        int GroupId { get; set; }
        public Subject(int id, string? name,
            string? group, int groupId)
        {
            Id = id;
            Name = name;
            Group = group;
            GroupId = groupId;
        }
        public int GetId() => this.Id;
        public int GetGroupId() => this.GroupId;
    }
}