﻿using Moda.BackEnd.Common.DTO.Response;
using System.Linq.Expressions;

namespace Moda.BackEnd.Application.IRepositories;

public interface IRepository<T> where T : class
{
    Task<PagedResult<T>> GetAllDataByExpression(Expression<Func<T, bool>>? filter,
    int pageNumber, int pageSize,
    Expression<Func<T, object>>? orderBy = null,
    bool isAscending = true,
    params Expression<Func<T, object>>[]? includes);
    Task<T> GetById(object id);
    Task<T?> GetByExpression(Expression<Func<T?, bool>> filter,
        params Expression<Func<T, object>>[]? includeProperties);
    Task<T> Insert(T entity);
    Task<List<T>> InsertRange(IEnumerable<T> entities);
    Task<List<T>> DeleteRange(IEnumerable<T> entities);
    Task<T> Update(T entity);
    Task<T?> DeleteById(object id);
}
