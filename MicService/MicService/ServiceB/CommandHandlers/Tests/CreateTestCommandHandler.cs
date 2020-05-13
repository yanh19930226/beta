using MediatR;
using Resilience.Zeus.Domain.Core.CommandHandlers;
using Resilience.Zeus.Domain.Interfaces;
using ServiceB.Commands.Tests;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceB.Tests.CommandHandlers
{
    public class CreateTestCommandHandler : CommandHandler
        , IRequestHandler<CreateTestCommand, bool>
    {
        private readonly IRepository<TestModel> _testRepository;
        public CreateTestCommandHandler( IUnitOfWork uow, IRepository<TestModel> testRepository) : base(uow)
        {
            _testRepository = testRepository;
        }
        public async Task<bool> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            TestModel model = new TestModel();
            model.Name = request.Name;
            _testRepository.Add(model);
             return await CommitAsync();
        }
    }
}
