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
    public class UpdatePostCommandHandler : CommandHandler
        , IRequestHandler<UpdatePostCommand, bool>
    {

        private readonly IRepository<Post> _postRepository;
        public UpdatePostCommandHandler(IUnitOfWork uow, IRepository<Post> postRepository) : base(uow)
        {
            _postRepository = postRepository;
        }
        public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var update = await _postRepository.GetByIdAsync(request.Post.Id);
            update.Title = request.Post.Title;
            update.Content = request.Post.Content;
            _postRepository.Update(update);
            return await CommitAsync();
        }
    }
}
