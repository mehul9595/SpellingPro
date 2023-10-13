namespace SpellingMAUI;

[QueryProperty("Item", "Item")]
public partial class SpellingPage : ContentPage
{
	TodoItemDatabase database;

    public Spelling Item
	{
		get => BindingContext as Spelling;
        set => BindingContext = value;
	}

	public SpellingPage(TodoItemDatabase todoItemDatabase)
	{
		InitializeComponent();
		database = todoItemDatabase;

	}

	async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
            return;
        }

        await database.SaveSpellingAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Item.ID == 0)
            return;
        await database.DeleteSpellingAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}