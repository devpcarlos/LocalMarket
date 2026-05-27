using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.DTos.Conversations;
using LocalMarket.Core.DTos.Messages;
using LocalMarket.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalMarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConversationController : BaseController
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        // Cliente — ver mis conversaciones
        [HttpGet("my")]
        public async Task<IActionResult> GetMyConversations()
        {
            var userId = GetUserId();
            var result = await _conversationService.GetMyConversationsAsync(userId);
            return Ok(ApiResponseDto<List<ConversationDto>>.OK(result));
        }

        // Dueño del negocio — ver conversaciones de su negocio
        [HttpGet("business/{businessId:guid}")]
        public async Task<IActionResult> GetBusinessConversations(Guid businessId)
        {
            var userId = GetUserId();
            var result = await _conversationService
                .GetBusinessConversationsAsync(userId, businessId);
            return Ok(ApiResponseDto<List<ConversationDto>>.OK(result));
        }

        // Participante — ver mensajes de una conversación
        [HttpGet("{conversationId:guid}/messages")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var userId = GetUserId();
            var result = await _conversationService
                .GetMessagesAsync(userId, conversationId);
            return Ok(ApiResponseDto<List<MessageDto>>.OK(result));
        }

        // Cliente — enviar mensaje a un negocio (crea conversación si no existe)
        [HttpPost("business/{businessId:guid}/messages")]
        public async Task<IActionResult> SendMessage(
            Guid businessId, [FromBody] SendMessageDto dto)
        {
            var userId = GetUserId();
            var result = await _conversationService
                .SendMessageAsync(userId, businessId, dto);
            return Created($"api/conversation/{result.ConversationId}/messages",
                ApiResponseDto<MessageDto>.OK(result, "Message sent successfully"));
        }

        // Participante — marcar mensajes como leídos
        [HttpPatch("{conversationId:guid}/read")]
        public async Task<IActionResult> MarkAsRead(Guid conversationId)
        {
            var userId = GetUserId();
            await _conversationService.MarkAsReadAsync(userId, conversationId);
            return Ok(ApiResponseDto<string?>.OK(null, "Messages marked as read"));
        }
        // Dueño del negocio — responder en una conversación existente
        [HttpPost("{conversationId:guid}/reply")]
        public async Task<IActionResult> Reply(
            Guid conversationId, [FromBody] SendMessageDto dto)
        {
            var userId = GetUserId();
            var result = await _conversationService.ReplyAsync(userId, conversationId, dto);
            return Created($"api/conversation/{conversationId}/messages",
                ApiResponseDto<MessageDto>.OK(result, "Reply sent successfully"));
        }

    }
}
