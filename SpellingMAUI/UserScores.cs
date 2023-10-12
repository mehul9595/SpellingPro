using SQLite;

namespace SpellingMAUI;

public class UserScores
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int Correct { get; set; }
    public int InCorrect { get; set; }
    public string Time { get; set; }
    public bool Done { get; set; }
}
