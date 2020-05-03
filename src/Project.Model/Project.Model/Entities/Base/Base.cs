using Newtonsoft.Json;
using System;

namespace Project.Model.Entities
{
    public abstract class Base
    {
        public Base()
        {
            CreationDate = DateTime.Now;
        }
        
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime CreationDate { get; set; }
    }
}