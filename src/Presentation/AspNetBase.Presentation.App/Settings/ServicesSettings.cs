using AspNetBase.Common.Utils.Attributes;

namespace AspNetBase.Presentation.App.Settings
{
  [Settings("app:services")]
  public class ServicesSettings
  {
    public EmailSenderSettings EmailSender { get; set; }
  }
}
