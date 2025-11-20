using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Libraries;

public interface IPublicLibraryRepository : IRepository<PublicLibrary, Guid>
{
    Task<PublicLibrary> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}