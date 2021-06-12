using Microsoft.AspNetCore.Http;
using RSS.CompleteApp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RSS.CompleteApp.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Fornecedor")]
        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        public Guid SupplierId { get; set; }

        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "o campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Name { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "o campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Description { get; set; }

        [DisplayName("Imagem do produto")]
        public IFormFile UploadImage { get; set; }

        public string Image { get; set; }

        [Currency]
        [DisplayName("Preço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Price { get; set; }

        //evite que ao gerar o model na view o atributo seja usado no scaffold
        [ScaffoldColumn(false)]
        public DateTime RegisterDate { get; set; }

        [DisplayName("Ativo?")]
        public bool Active { get; set; }

        [DisplayName("Fornecedor")]
        public SupplierViewModel Supplier { get; set; }
        public IEnumerable<SupplierViewModel> Suppliers { get; set; } 
    }
}
