namespace MusicAlbumsShop.FrontEnd
{
    public class LoadingScreenService
    {
        public event Action? OnShow;
        public void ShowAndHide()
        {
            OnShow?.Invoke();
        }
    }
}
