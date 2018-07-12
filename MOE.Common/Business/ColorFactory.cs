using System.Drawing;

namespace MOE.Common.Business
{
    public class ColorFactory
    {
        private int counter;

        public ColorFactory()
        {
            counter = 0;
        }

        public Color GetNextColor()
        {
            if (counter > 19)
                counter = 0;
            if (counter < 0)
                counter = 0;

            var color = Color.Black;

            switch (counter)
            {
                case 0:
                {
                    color = Color.Red;
                }
                    break;

                case 1:
                {
                    color = Color.Aqua;
                }
                    break;
                case 2:
                {
                    color = Color.Blue;
                }
                    break;
                case 3:
                {
                    color = Color.Coral;
                }
                    break;
                case 4:
                {
                    color = Color.BlueViolet;
                }
                    break;
                case 5:
                {
                    color = Color.Chartreuse;
                }
                    break;
                case 6:
                {
                    color = Color.DarkCyan;
                }
                    break;
                case 7:
                {
                    color = Color.DarkOrange;
                }
                    break;
                case 8:
                {
                    color = Color.Indigo;
                }
                    break;
                case 9:
                {
                    color = Color.LightSkyBlue;
                }
                    break;
                case 10:
                {
                    color = Color.DarkRed;
                }
                    break;
                case 11:
                {
                    color = Color.YellowGreen;
                }
                    break;
                case 12:
                {
                    color = Color.Brown;
                }
                    break;
                case 13:
                {
                    color = Color.Indigo;
                }
                    break;
                case 14:
                {
                    color = Color.DeepSkyBlue;
                }
                    break;
                case 15:
                {
                    color = Color.DarkOliveGreen;
                }
                    break;
                case 16:
                {
                    color = Color.LawnGreen;
                }
                    break;
                case 17:
                {
                    color = Color.Magenta;
                }
                    break;
                case 18:
                {
                    color = Color.MediumTurquoise;
                }
                    break;
                case 19:
                {
                    color = Color.DarkSlateGray;
                }
                    break;
            }
            counter++;
            return color;
        }
    }
}