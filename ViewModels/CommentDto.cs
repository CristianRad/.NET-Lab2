namespace Lab2.ViewModels
{
    public class CommentDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public long TaskId { get; set; }
    }
}
