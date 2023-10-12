using SQLite;

namespace SpellingMAUI;

public class Spelling
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Words { get; set; }
}
