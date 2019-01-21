namespace AspNetBase.Infrastructure.DbInitilizer.Seed.Base
{
  internal interface ISeed
  {
    int ExceutionOrder { get; }
    void Run();
  }
}
