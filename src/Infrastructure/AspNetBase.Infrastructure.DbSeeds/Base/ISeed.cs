namespace AspNetBase.Infrastructure.DbSeeds.Base
{
  public interface ISeed
  {
    bool Skip { get; }
    int ExecutionOrder { get; }
    void Run();
  }
}
