namespace AspNetBase.Infrastructure.DbInitilizer.Seed.Base
{
  internal interface ISeed
  {
    int ExecutionOrder { get; }
    void Run();
  }
}
