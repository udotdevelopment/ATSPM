using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.WCFServiceLibrary
{
    public class DotSizeItem
    {
        public int ID { get; set; }
        public string Value { get; set; }

        public DotSizeItem(int id, string value)
        {
            ID = id;
            Value = value;
        }
    }
}
