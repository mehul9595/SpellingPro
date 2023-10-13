using System.Timers;
using Timer = System.Threading.Timer;

namespace SpellingMAUI;

public partial class MainPage : ContentPage
{
    int count = 0;
    int correct = 0;
    int incorrect = 0;
    string currentWord = null;
    Timer threadTimer = null;
    List<string> incorrectWords = new List<string>();
    readonly Random random = new Random();
    private readonly TodoItemDatabase database;

    CancellationTokenSource cts;
    string wordsStr = "intense, determine, deny, presentation, nucleus, neutral, intellectual, approach, circular, observe, literature, rotten, exposure, gesture, abstract, substantial, conservation, scent, Budget, alternative, competent, output, damp, potential, combine, oral, agricultural, association, critical, division, distribution, strategy, capacity, management, contest, preparation, prosperity, beneath, opponent, breed, launch, slave, electric, measure, manufacturer, equivalent, sensible, puzzle, virtually, widespread, bend, acceptance, signal, regulation, survive, prohibit, grand, definition, attain, define, evidence, substance, establish, welfare, propose, unite, publication, comparison, external, universal, requirement, characteristic, code, pulse, resource, statistics, operate, variable, faculty, statistical, efficiency, executive, operation, motivate, represent, diverse, criticism, concentration, enthusiasm, hence, participate, philosopher, suitable, rational, questionnaire, emotional, administration, precise, temperature, logical, tendency, accord, crowd, conference, atmosphere, genuine, concern, Republic, personnel, claim, Republican, structure, pack, complex, candidate, impressive, politics, filter, response, retain, reduction, excess, electrical, maintain, efficient, digital, span, application, interfere, commercial, possession, zone, evolve, flash, interpretation, convey, revolution, phrase, engage, peasant, racial, entertainment, magnificent, benefit, security, successfully, locate, concept, reveal, emerge, reform, essential, track, bang, outcome, threaten, exploit, impose, researcher, bind, molecule, magnetic, photograph, oxygen, trail, schedule, reproduce, sponsor, threat, implication, legend, democracy, marine, appliance, perfume, explosive, identify, unconscious, resemble, lean, estimate, recognise, photographic, stroke, density, demonstrate, agriculture, unique, publish, throughout, initial, trace, vibration, scale, sticky, refine, hatch, incident, flexible, criteria";

    public List<string> Words { get; set; }

    public MainPage(TodoItemDatabase todoItemDatabase)
    {
        InitializeComponent();
        Words = wordsStr.Split(',').Select(x => x.Trim()).ToList();
        database = todoItemDatabase;
    }

    private async void TxtSpell_Completed(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        if (!string.IsNullOrEmpty(currentWord))
        {
            if (currentWord.ToLower() == text.ToLower())
            {
                correct++;
                AnswerLbl.TextColor = Color.Parse("Green");
            }
            else
            {
                incorrect++;
                AnswerLbl.TextColor = Color.Parse("Red");
                incorrectWords.Add(currentWord);
            }
        }
        AnswerLbl.Text = currentWord;
        ResultLbl.Text = $"Correct: {correct} | Incorrect: {incorrect} | Current Position: {count}";

        TxtSpell.Text = "";

        currentWord = GetRandomWord();
        await SpeakNowDefaultSettingsAsync(currentWord);
    }

    private async void SpeakBtn_Clicked(object sender, EventArgs e)
    {
        currentWord = GetRandomWord();
        SpeakBtn.IsEnabled = false;
        StopBtn.IsEnabled = true;
        ResultLbl.IsVisible = true;
        AnswerLbl.IsVisible = true;
        StartTimer();
        await SpeakNowDefaultSettingsAsync(currentWord);
    }

    private string GetRandomWord()
    {
        count = random.Next(Words.Count);
        return Words[count];
    }

    private void StartTimer()
    {
        int seconds = 0;

        threadTimer = new Timer(callback =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    seconds++;
                    TimeSpan t = TimeSpan.FromSeconds(seconds);
                    string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds);
                    TimerLbl.Text = answer;
                });
            });
        }, null, 0, 1000);
    }

    private void StopTimer()
    {
        if (threadTimer != null)
        {
            threadTimer.Change(Timeout.Infinite, Timeout.Infinite);
            threadTimer.Dispose();
            threadTimer = null;
        }
    }


    public async Task SpeakNowDefaultSettingsAsync(string text = "Hello World")
    {
        cts = new CancellationTokenSource();
        await TextToSpeech.Default.SpeakAsync(text, cancelToken: cts.Token);

    }

    // Cancel speech if a cancellation token exists & hasn't been already requested.
    public void CancelSpeech()
    {
        if (cts?.IsCancellationRequested ?? true)
            return;

        cts.Cancel();
    }

    private async void StopBtn_Clicked(object sender, EventArgs e)
    {
        // save to database
        StopTimer();

        UserScores scores = new UserScores();
        scores.Correct = correct;
        scores.InCorrect = incorrect;
        scores.Time = TimerLbl.Text;
        scores.IncorrectWords = string.Join(", ", incorrectWords);

        await database.SaveItemAsync(scores);

        SpeakBtn.IsEnabled = true;
        StopBtn.IsEnabled = false;
        currentWord = null;

        // reset stats
        correct = incorrect = count = 0;
        ResultLbl.Text = "";
        ResultLbl.IsVisible = AnswerLbl.IsVisible = false;
        incorrectWords.Clear();
        AnswerLbl.Text = "";
    }
}