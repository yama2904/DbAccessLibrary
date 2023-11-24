using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Data.Common;
using System.Collections;

namespace DbAccessLibrary
{
    public class DbAccessForSqlServer : IDbAccess
    {
        /// <summary>
        /// DB接続管理クラス
        /// </summary>
        SqlConnection _sqlConn = null;

        /// <summary>
        /// DBトランザクション管理クラス
        /// </summary>
        SqlTransaction _sqlTran = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="address">DBサーバのアドレス</param>
        /// <param name="database">参照するデータベース</param>
        /// <param name="id">ユーザID</param>
        /// <param name="password">パスワード</param>
        /// <param name="timeout">接続タイムアウト時間（秒）</param>
        public DbAccessForSqlServer(string address, string database, string id, string password, int timeout = 15)
        {
            string connectString = $"Data Source={address};Initial Catalog={database};User ID={id};Password={password};Connect Timeout={timeout};";
            _sqlConn = new SqlConnection(connectString);
            _sqlConn.Open();
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTran()
        {
            _sqlTran = _sqlConn.BeginTransaction();
        }

        /// <summary>
        /// コミット
        /// </summary>
        public void Commit()
        {
            _sqlTran.Commit();
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        public void Rollback()
        {
            _sqlTran.Rollback();
        }

        /// <summary>
        /// Select文実行
        /// </summary>
        /// <param name="sql">SQL文</param>
        /// <param name="sqlParams">SQLパラメータ</param>
        /// <returns>実行結果</returns>
        public DataTable ExecuteSelect(string sql, List<SqlParameter> sqlParams = null)
        {
            DataTable ret = new DataTable();

            using (var sqlCommand = _sqlConn.CreateCommand())
            {
                // SQL文セット
                sqlCommand.CommandText = sql;
                // SQLパラメータセット
                if (sqlParams != null)
                {
                    sqlCommand.Parameters.AddRange(sqlParams.ToArray());
                }

                // SQL実行
                if (_sqlTran != null) sqlCommand.Transaction = _sqlTran;
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                {
                    adapter.Fill(ret);
                }
            }

            return ret;
        }

        /// <summary>
        /// Insert・Update・Delete文実行
        /// </summary>
        /// <param name="sql">SQL文</param>
        /// <param name="sqlParams">SQLパラメータ</param>
        /// <returns>影響を受けた件数</returns>
        public int ExecuteUpdate(string sql, List<SqlParameter> sqlParams = null)
        {
            int ret = -1;

            using (var sqlCommand = _sqlConn.CreateCommand())
            {
                // SQL文セット
                sqlCommand.CommandText = sql;
                // SQLパラメータセット
                if (sqlParams != null)
                {
                    sqlCommand.Parameters.AddRange(sqlParams.ToArray());
                }

                // SQL実行
                if (_sqlTran != null) sqlCommand.Transaction = _sqlTran;
                ret = sqlCommand.ExecuteNonQuery();
            }

            return ret;
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            _sqlConn?.Dispose();
            _sqlTran?.Dispose();
            _sqlConn = null;
            _sqlTran = null;
        }
    }
}
