//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diplom.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class MedCard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedCard()
        {
            this.MedicalOpinions = new HashSet<MedicalOpinions>();
        }
    
        public int MedCardId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Vaccinations { get; set; }
        public string Result { get; set; }
        public int RecruitId { get; set; }
        public string Analyse { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Recruit Recruit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalOpinions> MedicalOpinions { get; set; }
    }
}
