using MusicAlbumsShop.FrontEnd;
using System.Reflection.Metadata;


namespace MusicAlbumsShop.FrontEnd
{
    public interface IModalService
    {
        void NewModal(string title, string text);
        void SetConfirmation(bool confirmation); //
        Task<bool> WaitForConfirmationAsync(); //
        string Title { get; set; }
        string Text { get; set; }
    }
}


public class ModalService : IModalService
{
    private TaskCompletionSource<bool>? _tcs;

    public string Title { get; set; }
    public string Text { get; set; }


    public void NewModal(string title, string text)
    {
        Title = title;
        Text = text;
        _tcs = new TaskCompletionSource<bool>();
    }

    public void SetConfirmation(bool confirmation)
    {
        _tcs?.TrySetResult(confirmation);
    }

    public Task<bool> WaitForConfirmationAsync()
    {
        return _tcs?.Task ?? Task.FromResult(false);
    }
}

