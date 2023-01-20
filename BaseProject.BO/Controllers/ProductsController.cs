using AutoMapper;
using BaseProject.Domain.Models.Dtos;
using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.Product;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.BO.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;
        public ProductsController(HttpClient httpClient, ICurrentUserService currentUserService)
        {
            _client = httpClient;
            var token = $"Bearer {currentUserService.GetToken()}";
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
        }
        public async Task<IActionResult> Index(string? message)
        {
            if (message != null)
                ViewBag.Message = message;
            var products = await _client.GetAsync($"Products/GetProducts");
            var canvasResponseData = await products.ReadContentAs<List<ProductDTO>>();
            return View();
        }

        public async Task<IActionResult> ProductList_Read([DataSourceRequest] DataSourceRequest request, string requestFilters)
        {
            request.Filters = null;
            var searchModel = new DataTableApiRequest
            {
                Request = request,
                RequestFilters = requestFilters,
            };
            var response = await _client.PostAsJsonAsync($"Products/GetProductsDatasource", searchModel);
            var data = await response.ReadContentAsDataSource<ProductDTO>(request);

            return Json(data.Data);
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> ProductList_Delete([DataSourceRequest] DataSourceRequest request, ProductDTO product)
        {
            if (product != null)
            {
                var response = await _client.DeleteAsync($"Products/DeleteProduct/{product.Id}");
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        public async Task<IActionResult> AddProductToBasket(string Id)
        {
            var basket = await _client.GetAsync($"Baskets/GetBasket");
            var basketData = await basket.ReadContentAs<BasketDTO>();

            var response = await _client.PostAsJsonAsync($"Baskets/AddProductToBasket", new AddBasketProductDTO { ProductId=Guid.Parse(Id), BasketId=basketData.Data.Id});
            var data = await response.ReadContentAs<BasketProductDTO>();
            if(data.IsSucceed)
                return RedirectToAction(nameof(Index), new { message = "Ürün başarıyla sepete eklendi" });
            return RedirectToAction(nameof(Index), new { message = "Ürün stokta olmadığından sepete eklenemedi" });

        }

        public async Task<IActionResult> Add()
        {
            var model = new AddProductDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Products/AddProduct",product);
                var data = await response.ReadContentAs<ProductDTO>();
                if(data.IsSucceed)
                    return RedirectToAction(nameof(Index), new { message = "Ürün başarıyla eklendi" });
                return RedirectToAction(nameof(Index), new { message = "Ürün eklenemedi" });
            }
            return View(product);
        }

        public async Task<IActionResult> Update(Guid Id)
        {
            var product = await _client.GetAsync($"Products/GetProduct/{Id}");
            var productData = (await product.ReadContentAs<ProductDTO>()).Data;
            var model = new UpdateProductDTO { Id = productData.Id, Name = productData.Name, Price = productData.Price, Stock = productData.Stock, Type=productData.Type };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsJsonAsync($"Products/UpdateProduct", product);
                var data = await response.ReadContentAs<ProductDTO>();
                if (data.IsSucceed)
                    return RedirectToAction(nameof(Index), new { message = "Ürün başarıyla güncellendi" });
                return RedirectToAction(nameof(Index), new { message = "Ürün güncellenemedi" });
            }
            return View(product);
        }
    }
}
