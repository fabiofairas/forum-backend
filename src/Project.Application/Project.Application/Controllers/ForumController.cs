using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Model.DTOs;
using Project.Model.Events;
using Project.Model.Notifications;
using Project.Model.Queries.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumController : Controller
    {
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notifications;
        private readonly IQuestionQueries _questionQueries;

        public ForumController(IMediator mediator, INotificationHandler<DomainNotification> notifications, IQuestionQueries questionQueries)
        {
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
            _questionQueries = questionQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var questions = await _questionQueries.GetAllQuestionAsync();

            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var question = await _questionQueries.GetQuestionByIdAsync(id);

            return Ok(question);
        }

        [HttpPost]
        [Route("AddQuestion")]
        public async Task<IActionResult> AddQuestionAsync(QuestionDTO question)
        {
            try
            {
                question.Id = Guid.NewGuid().ToString();

                await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

                if (_notifications.HasNotification())
                    return BadRequest(_notifications.GetNotifications().Select(e => new { Error = e.Value }));

                return Ok(question.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAnswer")]
        public async Task<IActionResult> AddAnswerAsync(AnswerDTO answer)
        {
            try
            {
                answer.Id = Guid.NewGuid().ToString();

                await _mediator.Send(new AddAnswerCommand(answer.Id, answer.User, answer.Text, answer.QuestionId));

                if (_notifications.HasNotification())
                    return BadRequest(_notifications.GetNotifications().Select(e => new { Error = e.Value }));

                return Ok(new { answer.QuestionId, AnswerId = answer.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("LikeQuestion")]
        public async Task<IActionResult> LikeQuestionAsync(QuestionDTO question)
        {
            try
            {
                await _mediator.Send(new LikeQuestionCommand(question.Id, question.TypePost));

                if (_notifications.HasNotification())
                    return BadRequest(_notifications.GetNotifications().Select(e => new { Error = e.Value }));

                return Ok(question.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}