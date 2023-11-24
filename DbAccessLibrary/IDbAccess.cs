using System;
using System.Collections.Generic;
using System.Data;

namespace DbAccessLibrary
{
    public interface IDbAccess : IDisposable
    {
        /// <summary>
        /// トランザクション開始
        /// </summary>
        void BeginTran();

        /// <summary>
        /// コミット
        /// </summary>
        void Commit();

        /// <summary>
        /// ロールバック
        /// </summary>
        void Rollback();

        ///// <summary>
        ///// Select文実行
        ///// </summary>
        ///// <param name="sql">SQL文</param>
        ///// <param name="sqlParams">SQLパラメータ</param>
        ///// <returns>実行結果</returns>
        //DataTable ExecuteSelect(string sql, List<SqlParameter> sqlParams = null);

        ///// <summary>
        ///// Insert・Update・Delete文実行
        ///// </summary>
        ///// <param name="sql">SQL文</param>
        ///// <param name="sqlParams">SQLパラメータ</param>
        ///// <returns>影響を受けた件数</returns>
        //int ExecuteUpdate(string sql, List<SqlParameter> sqlParams = null);
    }
}
