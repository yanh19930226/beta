using MediatR;
using Resilience.Zeus.Domain.Core.CommandHandlers;
using Resilience.Zeus.Domain.Interfaces;
using Resillience.EventBus.Abstractions;
using Resillience.SmsService.Abstractions.Enums;
using Resillience.SmsService.Api.Application.Commands;
using Resillience.SmsService.Api.Application.IntegrationEvents;
using Resillience.SmsService.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.CommandHandlers
{
    public class SendMessageCommandHandler: CommandHandler
        , IRequestHandler<SendMessageCommand, bool>
    {
        private readonly IEventBus _eventBus;
        public SendMessageCommandHandler(IUnitOfWork uow, IEventBus eventBus) : base(uow)
        {
            _eventBus = eventBus;
        }
        public Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            if (request.sendMessageRequestDTO.TimeSendDateTime==null)
            {
                foreach (var item in request.sendMessageRequestDTO.Mobiles)
                {
                    //添加状态为发送中数据
                    SmsMessage model = new SmsMessage
                    {
                        Content = request.sendMessageRequestDTO.Content,
                        Type = request.sendMessageRequestDTO.Type,

                    };
                }

                //发送消息到消息队列
                SendMessageIntegrationEvent @event = new SendMessageIntegrationEvent();
                _eventBus.Publish(@event);
            }
            else
            {
                //添加状态为定时发送

            }
            throw new NotImplementedException();
        }
    }
}
