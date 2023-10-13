using System.Collections.ObjectModel;

namespace SpellingMAUI;

public partial class ResultsPage : ContentPage
{
    SpellingsDatabase database;
    public ObservableCollection<UserScores> Items { get; set; } = new();
    public ResultsPage(SpellingsDatabase todoItemDatabase)
    {
        InitializeComponent();
        database = todoItemDatabase;
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetItemsAsync();

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            Items.Clear();

            foreach (var item in items.OrderByDescending(x => x.ID))
                Items.Add(item);
        });
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // it is empty
    }
}