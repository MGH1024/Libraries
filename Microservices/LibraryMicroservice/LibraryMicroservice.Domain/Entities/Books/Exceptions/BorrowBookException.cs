﻿using MGH.Core.CrossCutting.Exceptions.Types;
namespace Domain.Entities.Books.Exceptions;

public class BorrowBookException(string message) : BusinessException(message);