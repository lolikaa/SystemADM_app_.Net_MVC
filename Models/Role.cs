//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SystemADM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            this.users = new HashSet<User>();
        }

        [Display(Name = "Uprawnienia")]
        public int id_rola { get; set; }
        [Display(Name = "Uprawnienia")]
        public string typ_uzytkownika { get; set; }
        [Display(Name = "Dodawanie zgłoszeń")]
        public bool dodawanie_zgloszen { get; set; }
        [Display(Name = "Zamykanie zgłoszeń")]
        public bool zamykanie_zgloszen { get; set; }
        [Display(Name = "Wszystkie nieruchomości")]
        public bool wszystkie_nieruchomosci { get; set; }
        [Display(Name = "Dodawanie nieruchomości")]
        public bool dodawanie_nieruchomosci { get; set; }
        [Display(Name = "Dodawanie użytkowników")]
        public bool dodawanie_uzytkownikow { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> users { get; set; }
    }
}
