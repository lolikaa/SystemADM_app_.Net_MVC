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

    public partial class Building
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Building()
        {
            this.zgloszenia = new HashSet<Ticket>();
            this.users = new HashSet<User>();
        }

        public string FullAdress
        {
            get
            {
                return ulica + ", " + kod_pocztowy + " " + miasto;
            }
        }

        [Display(Name = "Budynek")]
        public int id_budynek { get; set; }
        [Display(Name = "Nazwa Wsp?lnoty")] public string nazwa_wspolnoty { get; set; }
        [Display(Name = "Ulica")] public string ulica { get; set; }
        [Display(Name = "Miasto")] public string miasto { get; set; }
        [Display(Name = "Kod pocztowy")] public string kod_pocztowy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> zgloszenia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> users { get; set; }
    }
}
