using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public class MeasuresDefaults
    {
        [Key, Column(Order = 0)]
        public string Measure { get; set; }
        [Key, Column(Order = 1)]
        public string OptionName { get; set; }
        public string Value { get; set; }
    }
}
