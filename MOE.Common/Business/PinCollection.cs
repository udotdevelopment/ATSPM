using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    /// <summary>
    /// Default constructer for pin collection
    /// </summary>
    public class PinCollection
    {
        public List<Pin> Items = new List<Pin>();

        /// <summary>
        /// Adds pin objects to the pin collecton
        /// </summary>
        /// <param name="pin"></param>
        public void addItem(MOE.Common.Business.Pin pin)
        {
            Items.Add(pin);
        }
    }
}