namespace MusicAlbumsShop.FrontEnd
{
    public class LoadingScreenService
    {
        public event Action? OnShow;
        public event Action? OnHide;

        public void Show()
        {
            OnShow?.Invoke();
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }
    }
}
