namespace WpfApp2.Model;

public class Priority
{
    public Priority()
    {
    }

    public Priority(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    
    public override string ToString()
    {
        return Name;
    }
}