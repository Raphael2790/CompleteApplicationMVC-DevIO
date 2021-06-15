using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KissLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSS.Business.Interfaces;
using RSS.Business.Models;
using RSS.CompleteApp.Extensions;
using RSS.CompleteApp.ViewModels;

namespace RSS.CompleteApp.Controllers
{
    [Authorize]
    public class SupplierController : BaseController
    {
        //Deixamos o repositório injetado para agilizar buscas sem validações
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SupplierController(ISupplierRepository supplierRepository, 
                                    ISupplierService supplierService, 
                                    IMapper mapper,
                                    INotifiable notifiable,
                                    ILogger logger) : base(notifiable, logger)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll()));
        }

        [AllowAnonymous]
        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);
            if (supplierViewModel == null)
            {
                return NotFound();
            }

            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Add")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Supplier", "Add")]
        [Route("novo-fornecedor")]
        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _supplierService.AddSupplier(supplier);

            if (!ValidOperation()) return View(supplierViewModel);

            TempData["Sucesso"] = "Fornecedor cadastrado com êxito";

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("editar-cadastro-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplierViewModel = await GetSupplierProductsAddress(id);
            if (supplierViewModel == null)
            {
                return NotFound();
            }
            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [HttpPost]
        [Route("editar-cadastro-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return NotFound();

            if(!ModelState.IsValid) return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.UpdateSupplier(supplier);

            if (!ValidOperation()) return View(await GetSupplierProductsAddress(id));

            TempData["Sucesso"] = "Cadastro atualizado com êxito";

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Supplier", "Remove")]
        [Route("excluir-cadastro-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);
            if (supplierViewModel == null)
            {
                return NotFound();
            }

            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Remove")]
        [HttpPost, ActionName("Delete")]
        [Route("excluir-cadastro-fornecedor/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null) return NotFound();

            await _supplierService.RemoveSupplier(id);

            if (!ValidOperation()) return View(supplierViewModel);

            TempData["Sucesso"] = "Fornecedor exclui com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var supplier = await GetSupplierAddress(id);
            if (supplier == null) return NotFound();
            return PartialView("_AddressDetail", supplier);
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if (supplier == null) return NotFound();

            return PartialView("_UpdateAddress", new SupplierViewModel { Address = supplier.Address });
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [HttpPost]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> UpdateAddress(SupplierViewModel supplierViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("IdentificationDocument");

            if (!ModelState.IsValid) return PartialView("_UpdateAddress", supplierViewModel);

            await _supplierService.UpdateSupplierAddress(_mapper.Map<Address>(supplierViewModel.Address));

            if (!ValidOperation()) return PartialView("_UpdateAddress", supplierViewModel);

            var url = Url.Action("GetAddress", "Supplier", new { id = supplierViewModel.Address.SupplierId });

            return Json(new { success = true, url });
        }

        private async Task<SupplierViewModel> GetSupplierAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddress(id));
        }

        private async Task<SupplierViewModel> GetSupplierProductsAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierProductsAddress(id));
        }
    }
}
