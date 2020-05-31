using Resillience.Util.ResillienceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Resillience.Util
{
    /// <summary>
    /// <see cref="IQueryable{T}"/> 扩展
    /// </summary>
    public static partial class QueryableExtensions
    {
        #region WhereIf(是否执行指定条件的查询)
        
        #endregion

        #region PageBy(分页)
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PageResult<IQueryable<T>> ToPage<T>(this IQueryable<T> source,int total)
        {
            PageResult<IQueryable<T>> obj = new PageResult<IQueryable<T>>();
            if (source.Count()>0)
            {
                obj.Result = source;
                obj.Total = total;
            }
            return obj;
        }
        #endregion

    }
}
