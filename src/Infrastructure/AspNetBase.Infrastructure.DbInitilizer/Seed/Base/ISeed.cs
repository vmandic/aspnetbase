namespace AspNetBase.Infrastructure.DbInitilizer.Seed.Base
{
  internal interface ISeed
  {
    bool Skip { get; }
    int ExecutionOrder { get; }
    void Run();
  }
}
