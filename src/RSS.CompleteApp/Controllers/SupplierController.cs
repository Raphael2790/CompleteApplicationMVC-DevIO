﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RSS.Business.Interfaces;
using RSS.Business.Models;
using RSS.CompleteApp.ViewModels;

namespace RSS.CompleteApp.Controllers
{
    public class SupplierController : BaseController
    {
        //Deixamos o repositório injetado para agilizar buscas sem validações
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierRepository supplierRepository, 
                                    ISupplierService supplierService, 
                                    IMapper mapper,
                                    INotifiable notifiable) : base(notifiable)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll()));
        }

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

        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("novo-fornecedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _supplierService.AddSupplier(supplier);

            if (!ValidOperation()) return View(supplierViewModel);

            TempData["Sucesso"] = "Fornecedor cadastrado com êxito";

            return RedirectToAction(nameof(Index));
        }

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

        [HttpPost]
        [Route("editar-cadastro-fornecedor/{id:guid}")]
        [ValidateAntiForgeryToken]
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

        [HttpPost, ActionName("Delete")]
        [Route("excluir-cadastro-fornecedor/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null) return NotFound();

            await _supplierService.RemoveSupplier(id);

            if (!ValidOperation()) return View(supplierViewModel);

            TempData["Sucesso"] = "Fornecedor exclui com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        [Route("endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var supplier = await GetSupplierAddress(id);
            if (supplier == null) return NotFound();
            return PartialView("_AddressDetail", supplier);
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if (supplier == null) return NotFound();

            return PartialView("_UpdateAddress", new SupplierViewModel { Address = supplier.Address });
        }

        [HttpPost]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [ValidateAntiForgeryToken]
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
