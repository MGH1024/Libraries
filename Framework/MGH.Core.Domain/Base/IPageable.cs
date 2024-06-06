﻿namespace MGH.Core.Domain.Base;

public interface IPageable
{
    int Row { get; }
    int TotalCount { get; }
    int CurrentPage { get; }
    int PageSize { get; }
}