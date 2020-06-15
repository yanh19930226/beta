using Resilience.Zeus.Domain.Core.Commands;
using Resillience.SmsService.Abstractions.DTOs.RequestsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.Commands
{
    public class SendMessageCommand : Command
    {
        public  SendMessageCommand(SendMessageRequestDTO dto)
        {
            sendMessageRequestDTO = dto;
        }
        public SendMessageRequestDTO sendMessageRequestDTO { get; set; }
    }
}
