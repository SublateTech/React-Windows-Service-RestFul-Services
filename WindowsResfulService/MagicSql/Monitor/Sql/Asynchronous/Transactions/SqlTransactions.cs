using SqlMagic.Monitor.Items.Results;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SqlMagic.Monitor.Sql
{
#if NET45

    /// <summary>
    /// A fully managed SQL Server class that takes care of handling connections.
    /// </summary>
    public sealed partial class Sql : IDisposable
    {
    #region Begin/End Transaction Async

        /// <summary>
        /// Begins a new transaction, with the isolation level set to serializable, on a new connection. The returned transaction object
        /// contains a connection that should not be closed for the lifetime of the transaction.
        /// </summary>
        /// <returns>A SqlTransaction object</returns>
        /// <remarks>
        /// When using the transaction systems, make sure that the method overloads being used, whether they are
        /// asynchronous or not, include overloads to pass a connection and to leave the connection open after
        /// execution. Failure to follow these rules can and will lead to unexpected or undesired side effects
        /// </remarks>
        public async Task<SqlTransaction> BeginTransactionAsync()
        {
            return await BeginTransactionAsync(IsolationLevel.Serializable);
        }

        /// <summary>
        /// Begins a new transaction on a new connection. The returned transaction object
        /// contains a connection that should not be closed for the lifetime of the transaction.
        /// </summary>
        /// <returns>A SqlTransaction object</returns>
        /// <remarks>
        /// When using the transaction systems, make sure that the method overloads being used, whether they are
        /// asynchronous or not, include overloads to pass a connection and to leave the connection open after
        /// execution. Failure to follow these rules can and will lead to unexpected or undesired side effects
        /// </remarks>
        public async Task<SqlTransaction> BeginTransactionAsync(IsolationLevel aIsolationLevel)
        {
            return await Task.Factory.StartNew<SqlTransaction>(() => { return BeginTransaction(aIsolationLevel); });
        }

        /// <summary>
        /// Ends the transaction, commits all changes, and closes the connection
        /// </summary>
        /// <param name="aTransaction">The transaction to end</param>
        public async Task EndTransactionAsync(SqlTransaction aTransaction)
        {
            await EndTransactionAsync(aTransaction, true);
        }

        /// <summary>
        /// Ends the transaction and closes the connection.
        /// </summary>
        /// <param name="aCommit">If true, the transaction will be committed. Otherwise, the transaction will be rolled back</param>
        /// <param name="aTransaction">The transaction to end</param>
        public async Task EndTransactionAsync(SqlTransaction aTransaction, Boolean aCommit)
        {
            await Task.Factory.StartNew(() => EndTransaction(aTransaction, aCommit));
        }

        #endregion Begin/End Transaction Async
    }

#endif
}