using System;

namespace Project.Model.DTOs.Base
{
    public class BaseDTO
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
