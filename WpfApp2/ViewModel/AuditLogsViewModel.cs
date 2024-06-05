using WpfApp2.Data;
using WpfApp2.Model;

namespace WpfApp2.ViewModels;

public class AuditLogsViewModel
{
    private readonly ApplicationDbContext _context;

    public AuditLogsViewModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddAuditLog(string actionName, string table, int id)
    {
        var auditLogsTable = _context.AuditLogs;

        auditLogsTable.Add(new AuditLog
        {
            ModifiedId = id,
            ModifiedTable = table,
            Name = actionName,
            Date = DateTime.UtcNow
        });

        _context.SaveChanges();
    }
    
    public List<AuditLog> GetAllAuditLogs()
    {
        try
        {
            var auditLogs = _context.AuditLogs.ToList();
            return auditLogs;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while retrieving audit logs: {ex.Message}");
            return new List<AuditLog>();
        }
    }
}