using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.Domain
{
    public class Account : IdentityUser
    {
      public string Type {  get; set; }
       
    }
}
