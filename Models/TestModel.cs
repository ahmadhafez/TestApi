using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Framework.Models;

namespace Test_Framework
{
    public class TestModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }

        public List<ApiModel> Apis { get; set; }

    }
}
