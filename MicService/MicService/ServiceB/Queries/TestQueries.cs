using AutoMapper;
using Resilience.Zeus.Domain.Interfaces;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Queries
{
    public class TestQueries: ITestQueries
    {
        private readonly IMapper _mapper;
        public readonly IRepository<TestModel> _testRepository;
        public  TestQueries(IRepository<TestModel> testRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _mapper = mapper;
        }

        public IQueryable<TestModel>GetAll()
        {
            return _testRepository.GetAll();
        }
    }
}
