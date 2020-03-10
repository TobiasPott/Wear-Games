using Android.Graphics;
using Android.Util;
using Android.Views;
using WearGames.Breakout;

namespace WearGames.Components.Breakout
{
    public class SimpleBlockGrid : IBlockGridTemplate
    {
        public int X { get; set; } = 3;
        public int Y { get; set; } = 3;
        public int Width { get; set; } = 25;
        public int Height { get; set; } = 16;
        public byte Structure { get; set; } = 1;


        public void Create(ViewGroup parent, float xOffset = 0, float yOffset = 0, float spacing = 3)
        {
            float widthWithSpacing = this.Width + spacing;
            float heightWithSpacing = this.Height + spacing;

            xOffset += ((this.X * widthWithSpacing - spacing) / 2);
            yOffset += ((this.Y * heightWithSpacing - spacing) / 2);

            DisplayMetrics dm = parent.Context.Resources.DisplayMetrics;
            for (int x = 0; x < this.X; x++)
            {
                for (int y = 0; y < this.Y; y++)
                {
                    // ! ! ! !
                    //  ->  Replace Px-based positioning and sizing with display independent resolution
                    float pX = TypedValue.ApplyDimension(ComplexUnitType.Px, parent.Width / 2 - xOffset + (x * widthWithSpacing), dm);
                    float pY = TypedValue.ApplyDimension(ComplexUnitType.Px, parent.Height / 2 - yOffset + (y * heightWithSpacing), dm);
                    int wd = (int)TypedValue.ApplyDimension(ComplexUnitType.Px, this.Width, dm);
                    int hg = (int)TypedValue.ApplyDimension(ComplexUnitType.Px, this.Height, dm);
                    BlockView block = BlockView.Create(parent, pX, pY, wd, hg, Color.White);
                    block.Structure = this.Structure;
                    block.ColorMask = ColorMask.G | ColorMask.B;
                }
            }
        }
    }

}