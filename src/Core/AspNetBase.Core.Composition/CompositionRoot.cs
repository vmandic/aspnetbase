using DryIoc;
//using DryIoc.MefAttributedModel;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetBase.Core.Composition
{
  public class CompositionRoot
  {
    //public static IServiceProvider Initialize(IServiceCollection services)
    //{
    //  return new Container()
    //      // optional: to support MEF attributed services discovery
    //      //.WithMef()
    //      // setup DI adapter
    //      //.WithDependencyInjectionAdapter(services,
    //      //    // optional: propagate exception if specified types are not resolved, and prevent fallback to default Asp resolution
    //      //    throwIfUnresolved: type => type.Name.EndsWith("Controller"))
    //      // add registrations from CompositionRoot classs
    //      .ConfigureServiceProvider<CompositionRoot>();
    //}

    //public CompositionRoot(IContainer registry)
    //{

    //}
  }
}
