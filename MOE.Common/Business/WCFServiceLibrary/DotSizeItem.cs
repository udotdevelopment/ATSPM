namespace MOE.Common.Business.WCFServiceLibrary
{
    public class DotSizeItem
    {
        public DotSizeItem(int id, string value)
        {
            ID = id;
            Value = value;
        }

        public int ID { get; set; }
        public string Value { get; set; }
    }
}