//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVBusService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class busStop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public busStop()
        {
            this.routeStops = new HashSet<routeStop>();
            this.tripStops = new HashSet<tripStop>();
        }
    
        public int busStopNumber { get; set; }
        public string location { get; set; }
        public int locationHash { get; set; }
        public bool goingDowntown { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<routeStop> routeStops { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tripStop> tripStops { get; set; }
    }
}
