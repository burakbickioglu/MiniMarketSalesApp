using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Service.Services.Product.Commands;

public class DeleteProductCommand : CommandBase<CommandResult<ProductDTO>>
{
    public Guid Id { get; set; }

    public DeleteProductCommand(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteProductCommand, CommandResult<ProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Product> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Product> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<ProductDTO>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productResponse = await _repository.GetFirstTracked(p => p.Id == request.Id);
            if (productResponse == null)
                return CommandResult<ProductDTO>.GetFailed("Ürün silinemedi");

            var deleteResult = await _repository.Delete(productResponse.Id);
            return deleteResult.IsSucceed
                ? CommandResult<ProductDTO>.GetSucceed(_mapper.Map<ProductDTO>(deleteResult.Data))
                : CommandResult<ProductDTO>.GetFailed("Ürün silinemedi");
        }
    }
}
