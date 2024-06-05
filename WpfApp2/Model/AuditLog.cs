namespace WpfApp2.Model;

public class AuditLog
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string ModifiedTable { get; set; }
    
    public int ModifiedId { get; set; }
    
    public DateTime Date { get; set; }
}