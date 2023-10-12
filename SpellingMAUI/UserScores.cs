using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellingMAUI
{
    public class UserScores
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int Correct { get; set; }
        public int InCorrect { get; set; }
        public string Time { get; set; }
        public bool Done { get; set; }
    }
}
