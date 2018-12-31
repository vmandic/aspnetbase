namespace AspNetBase.Core.Models.Identity.AccountManagement
{
  public class UserProfileDto
  {
    public UserProfileDto(
      string userName,
      string email,
      string phoneNumber,
      bool isEmailConfirmed)
    {
      this.UserName = userName;
      this.Email = email;
      this.PhoneNumber = phoneNumber;
      this.IsEmailConfirmed = isEmailConfirmed;

    }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
  }
}
