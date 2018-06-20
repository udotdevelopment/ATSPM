using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Tests
{
    [TestClass()]
    public class ColorFactoryTests
    {
        [TestMethod()]
        public void GetNextColorTest()
        {
            ColorFactory cf = new ColorFactory();

            var color = cf.GetNextColor();

            Assert.IsTrue(color == Color.Red);
        }

        [TestMethod()]
        public void ConsecutiveColorsShouldBeDifferent()
        {
            ColorFactory cf = new ColorFactory();

            var color = cf.GetNextColor();
            var color1 = cf.GetNextColor();
            var color2 = cf.GetNextColor();
            var color3 = cf.GetNextColor();
            var color4 = cf.GetNextColor();
            var color5 = cf.GetNextColor();

            Assert.IsTrue(color == Color.Red);
            Assert.IsTrue(color1 == Color.Aqua);
            Assert.IsTrue(color2 == Color.Blue);
            Assert.IsTrue(color3 == Color.Coral);
            Assert.IsTrue(color4 == Color.BlueViolet);
            Assert.IsTrue(color5 == Color.Chartreuse);


        }

        [TestMethod()]
        public void TheTwentiethColorShouldBeRedAgain()
        {
            ColorFactory cf = new ColorFactory();

            Color color = Color.Black;

            for (int i = 0; i < 21; i++)
            {
                color = cf.GetNextColor();
            }

            Assert.IsTrue(color == Color.Red);
        }
    }
}
