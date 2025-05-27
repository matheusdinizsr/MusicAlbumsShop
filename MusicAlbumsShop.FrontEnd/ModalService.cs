using MusicAlbumsShop.FrontEnd;
using System.Reflection.Metadata;


namespace MusicAlbumsShop.FrontEnd
{
    public interface IModalService
    {
        void SetConfirmation(bool confirmation);
        Task<bool> WaitForConfirmationAsync(string title, string text);
        Task<bool> WaitForConfirmationAsync(string title, string text, string buttonTrue, string buttonFalse);
        Task<bool> WaitForConfirmationAsync(string title, string text, string[] buttons);
        string Title { get; set; }
        string Text { get; set; }
        string ButtonTrue { get; set; }
        string ButtonFalse { get; set; }
        string[] ButtonsArray { get; set; }
        bool IsVisible { get; }
        event Action? OnModalChanged;
    }
}


public class ModalService : IModalService
{
    private TaskCompletionSource<bool>? _tcs;

    public string Title { get; set; }
    public string Text { get; set; }
    public string ButtonTrue { get; set; }
    public string ButtonFalse { get; set; }
    public string[] ButtonsArray { get; set; } = new string[0];
    public bool IsVisible { get; private set; }

    public event Action? OnModalChanged;

    public void SetConfirmation(bool confirmation)
    {
        _tcs?.TrySetResult(confirmation);
        IsVisible = false;
        OnModalChanged?.Invoke();
    }


    public Task<bool> WaitForConfirmationAsync(string title, string text, string buttonTrue, string buttonFalse)
    {
        Title = title;
        Text = text;
        ButtonTrue = buttonTrue;
        ButtonFalse = buttonFalse;
        _tcs = new TaskCompletionSource<bool>();
        IsVisible = true;
        OnModalChanged?.Invoke();

        return _tcs?.Task ?? Task.FromResult(false);
    }

    public Task<bool> WaitForConfirmationAsync(string title, string text)
    {
        return WaitForConfirmationAsync(title, text, "Yes", "No");
    }

    public Task<bool> WaitForConfirmationAsync(string title, string text, string[] buttons)
    {
        Title = title;
        Text = text;
        ButtonsArray = buttons;
        _tcs = new TaskCompletionSource<bool>();
        IsVisible = true;
        OnModalChanged?.Invoke();

        return _tcs?.Task ?? Task.FromResult(false);
    }
}

