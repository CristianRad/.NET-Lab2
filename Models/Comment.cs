namespace Lab2.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public Task Task { get; set; }
        public long TaskId { get; set; }
    }
}
