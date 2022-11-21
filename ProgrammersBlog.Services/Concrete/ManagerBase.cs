using AutoMapper;
using ProgrammersBlog.Data.Abstract;

namespace ProgrammersBlog.Services.Concrete
{
    public class ManagerBase
    {
        protected IUnitOfWork UnitOfWork { get; }
        protected IMapper Mapper { get; }
        public ManagerBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }


    }
}
