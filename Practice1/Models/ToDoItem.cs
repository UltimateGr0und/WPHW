namespace Practice1.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public string Deadline { get; set; }
        public bool IsFinished { get; set; }
    }
}
