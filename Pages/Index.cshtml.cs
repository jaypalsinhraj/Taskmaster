using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Data;
using TaskMaster.Models;

namespace TaskMaster.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TaskDbContext _context;

        public IndexModel(TaskDbContext context)
        {
            _context = context;
        }

        public List<TaskItem> Tasks { get; set; } = new();
        public List<TaskItem> CompletedTasks { get; set; } = new();

        [BindProperty]
        public TaskItem NewTask { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allTasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            Tasks = allTasks.Where(t => !t.IsCompleted).ToList();
            CompletedTasks = allTasks.Where(t => t.IsCompleted).ToList();
        }

        public async Task<IActionResult> OnPostAddTaskAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            _context.Tasks.Add(NewTask);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task.IsCompleted = true;
                task.CompletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}