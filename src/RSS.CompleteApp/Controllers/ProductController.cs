using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using KissLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSS.Business.Interfaces;
using RSS.Business.Models;
using RSS.CompleteApp.Extensions;
using RSS.CompleteApp.ViewModels;

namespace RSS.CompleteApp.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProductController(IProductRepository productRepository, 
                                    ISupplierRepository supplierRepository, 
                                    IProductService productService, 
                                    IMapper mapper,
                                    INotifiable notifiable,
                                    ILogger logger) : base (notifiable, logger)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliers()));
        }

        [AllowAnonymous]
        [Route("detalhes-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Add")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var productviewModel = await PopulateSuppliersList(new ProductViewModel());
            return View(productviewModel);
        }

        [ClaimsAuthorize("Product", "Add")]
        [HttpPost]
        [Route("novo-produto")]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await PopulateSuppliersList(productViewModel);
            if (!ModelState.IsValid) return View(productViewModel);

            var imgPrefix = Guid.NewGuid() + "_";

            if (!await UploadFile(productViewModel.UploadImage, imgPrefix))
                return View(productViewModel);

            productViewModel.Image = imgPrefix + productViewModel.UploadImage.FileName;

            await _productService.AddProduct(_mapper.Map<Product>(productViewModel));

            if (!ValidOperation()) return View(productViewModel);

            TempData["Sucesso"] = "Produto criado com sucesso!";
           
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "Edit")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            if (productViewModel == null)
            {
                return NotFound();
            }
            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Edit")]
        [HttpPost]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            var updateProduct = await GetProduct(id);
            productViewModel.Supplier = updateProduct.Supplier;
            productViewModel.Image = updateProduct.Image;
            if (!ModelState.IsValid) return View(productViewModel);

            if(productViewModel.UploadImage != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";

                if(!await UploadFile(productViewModel.UploadImage, imgPrefix))
                {
                    return View(productViewModel);
                }
                updateProduct.Image = imgPrefix + productViewModel.UploadImage.FileName;
            }

            //Usado o objeto rastreado pelo entity na modificação
            //Controlando também a troca das informações
            updateProduct.Name = productViewModel.Name;
            updateProduct.Description = productViewModel.Description;
            updateProduct.Price = productViewModel.Price;
            updateProduct.Active = productViewModel.Active;

            await _productService.UpdateProduct(_mapper.Map<Product>(updateProduct));

            if (!ValidOperation()) return View(productViewModel);

            TempData["Sucesso"] = "Produto atualizado com sucesso!";
           
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "Remove")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProduct(id);
        
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Remove")]
        [HttpPost, ActionName("Delete")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //verificar caso o produto já tenha sido deletado entre o intervalo da visualização e da confirmação
            var productViewModel = await GetProduct(id);

            if (productViewModel == null)
            {
                return NotFound();
            }

            await _productService.RemoveProduct(id);

            if (!ValidOperation()) return View(productViewModel);

            TempData["Sucesso"] = "Produto excluído com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplier(id));
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
            return product;
        }

        private async Task<ProductViewModel> PopulateSuppliersList(ProductViewModel product)
        {
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
            return product;
        }

        private async Task<bool> UploadFile(IFormFile file, string imgPrefix)
        {
            if (file.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefix + file.FileName);

            //verifica se o arquivo já existe
            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}
