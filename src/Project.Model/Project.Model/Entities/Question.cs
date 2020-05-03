using System.Collections.Generic;
using System;
using Project.Model.Enums;
using System.Text.Json.Serialization;

namespace Project.Model.Entities
{
    public class Question : Base
    {
        public Question()
        {   
            Answers = new List<Answer>();
        }
        public int QuantityAnswers { get; set; }
        public int QuantityLikes { get; set; }
        public string PartitionName { get; set; }
        [JsonIgnore]
        public ETypePost TypePost { get; set; }
        public IList<Answer> Answers { get; set; }
    }
}
