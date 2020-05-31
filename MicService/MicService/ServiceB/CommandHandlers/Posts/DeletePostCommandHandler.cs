using MediatR;
using Resilience.Zeus.Domain.Core.CommandHandlers;
using Resilience.Zeus.Domain.Interfaces;
using ServiceB.Commands.Posts;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceB.CommandHandlers.Posts
{
    public class DeletePostCommandHandler : CommandHandler
        , IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IRepository<Post> _postRepository;
        public DeletePostCommandHandler(IUnitOfWork uow, IRepository<Post> postRepository) : base(uow)
        {
            _postRepository = postRepository;
        }
        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            _postRepository.Remove(request.Id);
            return await CommitAsync();
        }
    }
}
