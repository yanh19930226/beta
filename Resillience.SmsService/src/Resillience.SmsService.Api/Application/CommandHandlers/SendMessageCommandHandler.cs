using MediatR;
using Resilience.Zeus.Domain.Core.CommandHandlers;
using Resilience.Zeus.Domain.Interfaces;
using Resillience.SmsService.Api.Application.Commands;
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
        public SendMessageCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }
        public Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
