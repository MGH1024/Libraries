﻿using Library.Domain.Libraries;
using Library.Infrastructure.Contexts;
using MGH.Core.Infrastructure.Persistence.EF.Base.Repository;

namespace Library.Infrastructure.Repositories;

public class LibraryRepository(LibraryDbContext libraryDbContext) :AggregateRepository<Domain.Libraries.Library,Guid>,ILibraryRepository
{
    
}