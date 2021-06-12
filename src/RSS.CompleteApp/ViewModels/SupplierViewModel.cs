using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RSS.CompleteApp.ViewModels
{
    public class SupplierViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "o campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Name { get; set; }

        [DisplayName("Documento Identificação")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "o campo {0} precisa ter entre {2} e {1}", MinimumLength = 11)]
        public string IdentificationDocument { get; set; }

        [DisplayName("Tipo")]
        public int SupplierType { get; set; }

        [DisplayName("Ativo?")]
        public bool Active { get; set; }

        /*EF Relations*/
        public AddressViewModel Address { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
