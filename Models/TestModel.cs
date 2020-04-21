using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Framework.Models
{
    public class TestModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Variables { get; set; }
        public string Urls { get; set; }
        public List<ApiModel> Apis { get; set; }
    }
}
