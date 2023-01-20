using BaseProject.Domain.Models.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Service.Services.Product.Queries;

public class GetProductsQuery :CommandBase<CommandResult<List<ProductDTO>>>
{
    public class Handler : IRequestHandler<GetProductsQuery, CommandResult<List<ProductDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Product> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Product> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<List<ProductDTO>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllFiltered().ToListAsync();
            if(products != null)
                return CommandResult<List<ProductDTO>>.GetSucceed(_mapper.Map<List<ProductDTO>>(products));
            return CommandResult<List<ProductDTO>>.NotFound();
        }
    }
}
