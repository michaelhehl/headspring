//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployeeDirectory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Location
    {
        public Location()
        {
            this.Employees = new HashSet<Employee>();
        }

        [Display(Name = "Location")]
        public int locationID { get; set; }
        [Display(Name = "Location")]
        public string locationname { get; set; }
        [Display(Name = "Street Address")]
        public string locationaddress { get; set; }
        [Display(Name = "City")]
        public string locationcity { get; set; }
        [Display(Name = "Postal Code")]
        public string locationzipcode { get; set; }
        [Display(Name = "State")]
        public string locationState { get; set; }
        [Display(Name = "Country")]
        public string locationcountry { get; set; }
        [Display(Name = "Phone Number")]
        public string locationphone { get; set; }
        [Display(Name = "Web Address")]
        public string locationweb { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
