using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LTS.UI.Animations
{
    public static class CarrierRenderer
    {
        public static Ellipse CreateCarrier()
        {
            return new Ellipse
            {
                Width = 18,
                Height = 18,

                Fill = Brushes.Cyan,

                Effect = new DropShadowEffect
                {
                    BlurRadius = 15,
                    ShadowDepth = 0,
                    Color = Colors.Cyan,
                    Opacity = 1
                }
            };
        }
    }
}