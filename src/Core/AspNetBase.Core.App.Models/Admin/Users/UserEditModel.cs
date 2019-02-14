using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;

namespace AspNetBase.Core.App.Models.Admin.Users
{
  public class UserEditModel
  {
    public UserEditModel() { }

    public UserEditModel(AppUser dbUser)
    {
      if (dbUser == null)
        throw new ArgumentNullException(nameof(dbUser));

      Id = dbUser.Id;
      Uid = dbUser.Uid;
      UserName = dbUser.UserName;
      Email = dbUser.Email;
      EmailConfirmed = dbUser.EmailConfirmed;
      PhoneNumber = dbUser.PhoneNumber;
      PhoneNumberConfirmed = dbUser.PhoneNumberConfirmed;
      TwoFactorEnabled = dbUser.TwoFactorEnabled;
      LockoutEnd = dbUser.LockoutEnd;
      LockoutEnabled = dbUser.LockoutEnabled;
      AccessFailedCount = dbUser.AccessFailedCount;
      RoleIds = dbUser.UserRoles.Select(x => x.RoleId);
    }

    public int Id { get; set; }

    [Display(Name = "Unique identifier")]
    public Guid Uid { get; set; }

    [Display(Name = "Username")]
    public string UserName { get; set; }
    public string Email { get; set; }

    [Display(Name = "Email confirmed")]
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Phone number")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [Display(Name = "Is phone number confirmed?")]
    public bool PhoneNumberConfirmed { get; set; }

    [Display(Name = "Is two factor auth enabled?")]
    public bool TwoFactorEnabled { get; set; }

    [Display(Name = "Lockout end date & time")]
    [DataType(DataType.DateTime)]
    public DateTimeOffset? LockoutEnd { get; set; }

    [Display(Name = "Is lockout enabled?")]
    public bool LockoutEnabled { get; set; }

    [Display(Name = "Login fail count")]
    public int AccessFailedCount { get; set; }

    [Display(Name = "Roles (highlighted are assigned)")]
    public IEnumerable<int> RoleIds { get; set; }
  }
}
