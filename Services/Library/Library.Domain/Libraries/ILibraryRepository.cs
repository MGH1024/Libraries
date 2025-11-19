using MGH.Core.Infrastructure.Persistence.Base;

namespace Library.Domain.Libraries;

public interface ILibraryRepository : IRepository<Library, Guid>
{
    Task<Library> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}