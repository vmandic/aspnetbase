using System;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppUserToken : IdentityUserToken<int>, IEntityBase<int?>
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    public Guid Uid { get; set; }
  }
}
