using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();
        private readonly ProjectContext _context;
        public ProjectRepository(ProjectContext context)
        {
            _context = context;
        }
        public Domain.AggregatesModel.Project Add(Domain.AggregatesModel.Project project)
        {
            if (project.IsTransient())
            {
                return _context.Add(project).Entity;
            }
            else
            {
                return project;
            }
        }

        public async Task<Domain.AggregatesModel.Project> GetAsync(int id)
        {
            var project = await _context.Projects
                .Include(q => q.Viewers)
                .Include(q => q.Contributors)
                .Include(q => q.VisibleRule)
                .SingleOrDefaultAsync(q => q.Id == id);
            return project;
        }

        public Domain.AggregatesModel.Project Update(Domain.AggregatesModel.Project project)
        {
            return _context.Update(project).Entity;
        }
    }
}
