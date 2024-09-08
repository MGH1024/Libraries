using AutoMapper;

namespace Application;

public class CancellationTokenResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, CancellationToken>
{
    public CancellationToken Resolve(TSource source, TDestination destination, CancellationToken destMember, ResolutionContext context)
    {
        return context.Items.TryGetValue("CancellationToken", out var token) && token is CancellationToken ct
            ? ct
            : CancellationToken.None;
    }
}
