using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.Product;
using BaseProject.Domain.Models.Dtos.Sales;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.BO.Controllers
{
    public class BasketsController : Controller
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;
        public BasketsController(HttpClient httpClient, ICurrentUserService currentUserService)
        {
            _client = httpClient;
            var token = $"Bearer {currentUserService.GetToken()}";
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
        }
        public async Task<IActionResult> Index()
        {
            var basket = await _client.GetAsync($"Baskets/GetBasket");
            var basketData = await basket.ReadContentAs<BasketDTO>();
            return View(basketData.Data);
        }

        public async Task<IActionResult> ProductList_Read([DataSourceRequest] DataSourceRequest request, string requestFilters)
        {
            request.Filters = null;
            var searchModel = new DataTableApiRequest
            {
                Request = request,
                RequestFilters = requestFilters,
            };
            var response = await _client.PostAsJsonAsync($"Baskets/GetBasketProductsDatasource", searchModel);
            var data = await response.ReadContentAsDataSource<ProductDTO>(request);

            return Json(data.Data);
        }

        public async Task<IActionResult> DeleteProductFromBasket(string Id)
        {
            var response = await _client.DeleteAsync($"Baskets/DeleteProductFromBasket/{Id}");
            return RedirectToAction(nameof(Index), new { message = "Ürün başarıyla sepetten silindi" });
        }

        [HttpPost]
        public async Task<IActionResult> CompleteSale(CompleteSaleDTO model)
        {
            var response = await _client.PostAsJsonAsync($"Baskets/CompleteSale", model);
            var data = await response.ReadContentAs<SalesDTO>();
            if(data.IsSucceed)
            {
                return RedirectToAction("Index", "Products", new { message = "Satış başarıyla tamamlandı" });
            }
            return RedirectToAction(nameof(Index), "Satış tamamlanamadı");
        }
    }
}
