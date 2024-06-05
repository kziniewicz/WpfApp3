using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp2.Model
{
    public class Tasks
    {
        public int TaskId { get; set; }
       
        [Required(ErrorMessage = "Nazwa zadania jest wymagana.")] 
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int PriorityId { get; set; }
        public bool Completed { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("PriorityId")]
        public Priority Priority { get; set; }

        public ICollection<Subtasks> SubTasks { get; set; } 
    }
}