using System.Collections.ObjectModel;

namespace SpellingMAUI;

public partial class YourListPage : ContentPage
{
    SpellingsDatabase database;
    public ObservableCollection<Spelling> ObservableSpellingList { get; set; } = new();
    public YourListPage(SpellingsDatabase todoItemDatabase)
    {
        InitializeComponent();
        database = todoItemDatabase;
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetSpellingsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ObservableSpellingList.Clear();
            foreach (var item in items)
                ObservableSpellingList.Add(item);

        });
    }
    private async void OnItemAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SpellingPage), true, new Dictionary<string, object>
        {
            ["Item"] = new Spelling()
        });
    }

    private async void OnItemUpdated(object sender, EventArgs e)
    {
        if (SpellingCollection.SelectedItem is not Spelling item)
            return;

        await Shell.Current.GoToAsync(nameof(SpellingPage), true, new Dictionary<string, object>
        {
            ["Item"] = item
        });
    }

    private async void OnPracticeClicked(object sender, EventArgs e)
    {
        if (SpellingCollection.SelectedItem is not Spelling item)
            return;

        // absolute route '//MainPage'
        await Shell.Current.GoToAsync(nameof(MainPage), true, new Dictionary<string, object>
        {
            ["Spelling"] = item
        });
    }
}