using Dapper;
using Microsoft.Extensions.Configuration;
using SimpleStack.Orm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SampleApp.DataAccess.Common
{
    public class DatabaseContext : IDisposable
    {
        private string _transactionId;
        private bool _matchTransactionId = false;

        public DatabaseContext(IConfiguration configuration)
        {
            this._connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        public bool TransInitialized { get; private set; }

        private string _connectionString;

        private OrmConnection _connection;
        public OrmConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var factory = new OrmConnectionFactory(new PostgresCustomDialectProvider(), this._connectionString);
                    this._connection = factory.OpenConnection();
                }

                return _connection;
            }
        }
        public IDbTransaction _currentTransaction;
        public IDbTransaction CurrentTransaction
        {
            get
            {
                return this.Connection.Transaction;
            }
            private set
            {
                this._currentTransaction = value;
            }
        }
        
        /// <summary>
        /// this function will execute Insert Statement with id sequence and async operation type
        /// </summary>
        /// <typeparam name="Task<T>"></typeparam>
        /// <param name="defination"></param>
        /// <returns></returns>
        public async Task<T> InsertAsync<T, TEntity>(TEntity entity)
        {
            var commandDefination = this.Connection.DialectProvider.ToInsertRowStatement(entity);
            var t = await this.Connection.ExecuteScalarAsync<T>(commandDefination);
            return t;
        }

        public string BeginTransaction(bool matchTransactionId = false)
        {

            if (this._matchTransactionId != true)
                this._matchTransactionId = matchTransactionId;

            /* if there is no existing transaction going on*/

            if (this.CurrentTransaction == null)
            {
                /* so this transaction is initialized by this function 
                 * commit it after all CUD is done
                 */
                this.CurrentTransaction = this.Connection.BeginTransaction();
                this.TransInitialized = true;
                _transactionId = Guid.NewGuid().ToString();
            }

            return _transactionId;
        }

        public void CommitTransaction(string transactionId = null)
        {
            if (this.TransInitialized && this.CurrentTransaction != null)
            {
                if (!this._matchTransactionId || (this._matchTransactionId && transactionId == this._transactionId))
                {
                    this.CurrentTransaction.Commit();

                    this._matchTransactionId = false;
                    this._transactionId = "";
                    this.CurrentTransaction = null;
                }
            }
        }

        public void Dispose()
        {
            this.Connection.Close();
            this._connection = null;
        }

        public async Task<IEnumerable<T1>> FunctionAsync<T1>(string functionName, params object[] parameters)
        {
            string functionParam = "";
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    string param = CreateFunctionParameter(parameter);
                    if (string.IsNullOrEmpty(param))
                    {
                        functionParam += "NULL,";
                    }
                    else
                    {
                        functionParam += param + ",";
                    }
                }
                functionParam = functionParam.Substring(0, functionParam.Length - 1);
            }
            string query = "select * from public." + functionName + "(" + functionParam + ")";
            var t = await this.Connection.QueryAsync<T1>(query);
            return t;
        }
        
        private string CreateFunctionParameter(object parameter)
        {
            if (parameter == null)
            {
                return "NULL";
            }
            else if (parameter is string)
            {
                string value = parameter.ToString();
                if (string.IsNullOrEmpty(value))
                {
                    return "NULL";
                }
                else
                {
                    return "'%" + value.ToString() + "%'";
                }
            }
            else if (parameter is DateTime)
            {
                DateTime? value = Convert.ToDateTime(parameter);
                object dvalue = value == DateTime.MinValue ? null : value;
                return dvalue == null ? "NULL" : "'" + dvalue.ToString() + "'";
            }
            else if (parameter is IEnumerable)
            {
                List<long> ids = (List<long>)parameter;
                if (ids.Count > 0)
                {
                    string arrParam = "array [";
                    foreach (long id in ids)
                    {
                        arrParam += id + ",";
                    }
                    arrParam = arrParam.Substring(0, arrParam.Length - 1);
                    arrParam += "]";
                    return arrParam;
                }
                else
                {
                    return "NULL";
                }
            }
            else if (parameter is bool || parameter is long || parameter is int)
            {
                return parameter.ToString();
            }
            else
            {
                return "NULL";
            }
        }
    }
}
