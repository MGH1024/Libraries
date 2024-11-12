﻿namespace MGH.Core.Domain.Entity.Features;

public interface IPageable
{
    int Row { get; }
    int TotalCount { get; }
    int CurrentPage { get; }
    int PageSize { get; }
}