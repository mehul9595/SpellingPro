using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellingMAUI
{
    public class TodoItemDatabase
    {
        SQLiteAsyncConnection Database;

        public TodoItemDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTablesAsync<UserScores, Spelling>();
        }
        public async Task<List<UserScores>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<UserScores>().ToListAsync();
        }
        
        public async Task<List<Spelling>> GetSpellingsAsync()
        {
            await Init();
            return await Database.Table<Spelling>().ToListAsync();
        }

        public async Task<List<UserScores>> GetItemsNotDoneAsync()
        {
            await Init();
            return await Database.Table<UserScores>().Where(t => t.Done).ToListAsync();

            // SQL queries are also possible
            //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public async Task<UserScores> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<UserScores>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(UserScores item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }
        
        public async Task<int> SaveSpellingAsync(Spelling item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(UserScores item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
        
        public async Task<int> DeleteSpellingAsync(Spelling item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
