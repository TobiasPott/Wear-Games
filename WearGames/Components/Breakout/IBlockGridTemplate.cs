using Android.Views;

namespace WearGames.Breakout
{
    public interface IBlockGridTemplate
    {
        void Create(ViewGroup parent, float xOffset = 0, float yOffset = 0, float spacing = 3);
    }

}