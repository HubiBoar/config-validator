using Cocona;
using Cocona.Builder;

namespace ConfigValidator.Console;

public interface IModule
{
    public static abstract void Configure(ICoconaCommandsBuilder app, Action<CallBuilder> finishedCallBack);
}

public static class ModulesExtensions
{
    public static void AddModule<TModule>(this ICoconaCommandsBuilder app, Action<CallBuilder> finishedCallBack)
        where TModule : IModule
    {
        TModule.Configure(app, finishedCallBack);
    }
}