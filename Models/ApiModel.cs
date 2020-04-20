using System;

namespace Test_Framework.Models
{
    public class ApiModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }

        public ApiModel(string name, string uriString)
        {
            Name = name;
            Uri = new Uri(uriString);
        }
        public ApiModel() { }
    }
}
