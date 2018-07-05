using System.Collections.Generic;

namespace MOE.Common.Business
{
    /// <summary>
    ///     Default constructer for pin collection
    /// </summary>
    public class PinCollection
    {
        public List<Pin> Items = new List<Pin>();

        /// <summary>
        ///     Adds pin objects to the pin collecton
        /// </summary>
        /// <param name="pin"></param>
        public void addItem(Pin pin)
        {
            Items.Add(pin);
        }
    }
}