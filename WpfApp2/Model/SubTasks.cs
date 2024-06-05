using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp2.Model
{
    public class Subtasks
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? Completed { get; set; }
        public int TaskId { get; set; } 

        [ForeignKey("TaskId")]
        public Tasks Task { get; set; } 
    }
}