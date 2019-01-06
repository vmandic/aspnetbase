using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppUserLogin : IdentityUserLogin<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
  }
}
