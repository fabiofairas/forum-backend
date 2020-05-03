using Project.Model.DTOs.Base;
using Project.Model.Enums;
using System.Collections.Generic;

namespace Project.Model.DTOs
{
    public class QuestionDTO : BaseDTO
    {
        public QuestionDTO()
        {
            Answers = new List<AnswerDTO>();
        }
        
        public int QuantityAnswers { get; set; }
        public int QuantityLikes { get; set; }        
        public ETypePost TypePost { get; set; }
        public IList<AnswerDTO> Answers { get; set; }
    }
}
