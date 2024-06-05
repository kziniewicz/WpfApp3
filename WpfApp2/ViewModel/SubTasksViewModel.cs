using System.Collections.Generic;
using System.Linq;
using WpfApp2.Data;
using WpfApp2.Model;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.ViewModels
{
    public class SubTasksViewModel
    {
        private readonly ApplicationDbContext _context;

        public SubTasksViewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Subtasks> GetAllSubTasks()
        {
            return _context.SubTasks.ToList();
        }

        public List<Subtasks> GetSubTasksForTask(int taskId)
        {
            return _context.SubTasks.Where(st => st.TaskId == taskId).ToList();
        }

        public void AddSubTask(Subtasks newSubTask)
        {
            _context.SubTasks.Add(newSubTask);
            _context.SaveChanges();
        }

        public void UpdateSubTask(Subtasks updatedSubTask)
        {
            _context.Entry(updatedSubTask).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteSubTask(int id)
        {
            var subTask = _context.SubTasks.Find(id);
            if (subTask != null)
            {
                _context.SubTasks.Remove(subTask);
                _context.SaveChanges();
            }
        }
    }
}