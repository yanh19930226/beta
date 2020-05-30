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
    public class CreatePostCommandHandler : CommandHandler
        , IRequestHandler<CreatePostCommand, bool>
    {
        private readonly IRepository<Post> _postRepository;
        public CreatePostCommandHandler(IUnitOfWork uow, IRepository<Post> postRepository) : base(uow)
        {
            _postRepository = postRepository;
        }
        public async Task<bool> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            Post model = new Post();
            model.BlogId = request.BlogId;
            model.Content = request.Content;
            model.Title = request.Title;
            _postRepository.Add(model);
            return await CommitAsync();
        }
    }
}
