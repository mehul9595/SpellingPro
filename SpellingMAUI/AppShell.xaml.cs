namespace SpellingMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SpellingPage), typeof(SpellingPage));
        }
    }
}