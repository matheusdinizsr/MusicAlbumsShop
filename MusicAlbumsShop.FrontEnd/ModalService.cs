using MusicAlbumsShop.FrontEnd;
using System.Reflection.Metadata;


namespace MusicAlbumsShop.FrontEnd
{
    public interface IModalService
    {
        void SetPressedButton(string button);
        Task<string> ShowModalWithButtonsAsync(string title, string text, string[] buttons);
        Task<bool> ShowConfirmation(string title, string text);
        Task<bool> ShowConfirmation(string title, string text, string buttonTrue, string buttonFalse);
        string Title { get; set; }
        string Text { get; set; }
        string[] ButtonsArray { get; set; }
        string ButtonPressed { get; set; }

        bool IsVisible { get; }
        event Action? OnModalChanged;
    }
}


public class ModalService : IModalService
{
    private TaskCompletionSource<string>? _tcs;
    public string Title { get; set; }
    public string Text { get; set; }
    public string[] ButtonsArray { get; set; } = new string[0];
    public string ButtonPressed { get; set; }
    public bool IsVisible { get; private set; }
    public event Action? OnModalChanged;

    public async Task<string> ShowModalWithButtonsAsync(string title, string text, string[] buttons)
    {
        Title = title;
        Text = text;
        ButtonsArray = buttons;
        IsVisible = true;
        _tcs?.SetResult("No");
        _tcs = new TaskCompletionSource<string>();
        OnModalChanged?.Invoke();

        return await _tcs.Task;
    }

    public async Task<bool> ShowConfirmation(string title, string text, string buttonTrue, string buttonFalse)
    {
        var button = await ShowModalWithButtonsAsync(title, text, [buttonTrue, buttonFalse]);

        return buttonTrue == ButtonPressed;
    }

    public async Task<bool> ShowConfirmation(string title, string text)
    {
        var button = await ShowModalWithButtonsAsync(title, text, ["Yes", "No"]);

        return button == "Yes";
    }

    public void SetPressedButton(string button)
    {
        ButtonPressed = button;
        IsVisible = false;
        OnModalChanged?.Invoke();

        _tcs.TrySetResult(button);
    }
}

