using System.Collections.ObjectModel;

namespace SpellingMAUI;

public partial class ResultsPage : ContentPage
{
	TodoItemDatabase database;
    public ObservableCollection<UserScores> Items { get; set; } = new();
	public ResultsPage(TodoItemDatabase todoItemDatabase)
	{
		InitializeComponent();
		database = todoItemDatabase;
		BindingContext = this;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);

        });
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // it is empty
    }
}