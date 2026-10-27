using Microsoft.Maui.Devices.Sensors;

namespace BetOracle;

public partial class MainPage : ContentPage
{
    static readonly string[] Answers =
    {
        "Да",
        "Нет",
        "Конечно",
        "Без сомнений",
        "Спор выигран",
        "Мак не нужен",
        "Ставка за тобой",
        "Спроси GitHub",
        "Покажи лог CI",
        "Windows рулит",
        "Спроси ещё раз",
        "Смутно вижу да",
        "Даже не думай",
        "Тряси сильнее",
        "Компилятор решит",
        "Мой ответ — нет",
    };

    readonly Random _random = new();
    int _count;
    bool _busy;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            Accelerometer.Default.ShakeDetected += OnShakeDetected;
            if (!Accelerometer.Default.IsMonitoring)
                Accelerometer.Default.Start(SensorSpeed.Game);
        }
        catch
        {
            // Устройство без акселерометра (например, ПК) — остаётся кнопка
        }
    }

    protected override void OnDisappearing()
    {
        try
        {
            Accelerometer.Default.ShakeDetected -= OnShakeDetected;
            if (Accelerometer.Default.IsMonitoring)
                Accelerometer.Default.Stop();
        }
        catch
        {
            // Игнорируем: сенсор недоступен
        }
        base.OnDisappearing();
    }

    void OnShakeDetected(object sender, EventArgs e) => AskOracle();

    void OnAskClicked(object? sender, EventArgs e) => AskOracle();

    void OnAskTapped(object? sender, TappedEventArgs e) => AskOracle();

    async void AskOracle()
    {
        if (_busy)
            return;

        _busy = true;

        AnswerLabel.Text = Answers[_random.Next(Answers.Length)];
        _count++;
        CounterLabel.Text = $"Предсказаний: {_count}";

        await Ball.ScaleTo(1.08, 90, Easing.CubicOut);
        await Ball.ScaleTo(1.0, 150, Easing.CubicIn);

        _busy = false;
    }
}
