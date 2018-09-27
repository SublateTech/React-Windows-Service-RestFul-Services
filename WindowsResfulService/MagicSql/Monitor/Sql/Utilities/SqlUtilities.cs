using SqlMagic.Monitor.Items.Results;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SqlMagic.Monitor.Sql
{
    /// <summary>
    /// A fully managed SQL Server class that takes care of handling connections.
    /// </summary>
    public sealed partial class Sql : IDisposable
    {
        #region Utilities

        /// <summary>
        /// Returns an opened connection object
        /// </summary>
        /// <returns>A SqlConnection object</returns>
        /// <seealso cref="System.Data.SqlClient.SqlConnection"/>
        public SqlConnection CreateConnection()
        {
            return CreateConnection(true);
        }

        /// <summary>
        /// Returns a connection object
        /// </summary>
        /// <param name="aOpened">If true, the connection will be opened before it is returned</param>
        /// <returns>A SqlConnection object</returns>
        /// <seealso cref="System.Data.SqlClient.SqlConnection"/>
        public SqlConnection CreateConnection(Boolean aOpened)
        {
            // Create the connection and open it
            SqlConnection oConnection = new SqlConnection(iConnectionString);

            // Create the Task
            if (aOpened) oConnection.Open();

            // Add the opened connection to the collection of connections
            iOpenConnections.TryAdd(oConnection, null);

            // Monitor the connection
            oConnection.StateChange += fStateChange;

            // Gathering time.
            oConnection.StatisticsEnabled = true;

            // Give it back
            return oConnection;
        }

        /// <summary>
        /// Attempts to create a connection.
        /// </summary>
        /// <param name="aConnection">The connection object that was created</param>
        /// <returns>A boolean value that indicates whether or not the connection was created successfully</returns>
        public Boolean TryCreateConnection(out SqlConnection aConnection)
        {
            return TryCreateConnection(true, out aConnection);
        }

        /// <summary>
        /// Attempts to create a connection.
        /// </summary>
        /// <param name="aConnection">The connection object that was created</param>
        /// <param name="aOpened">If true, the connection will be opened before it is returned</param>
        /// <returns>A boolean value that indicates whether or not the connection was created successfully</returns>
        public Boolean TryCreateConnection(Boolean aOpened, out SqlConnection aConnection)
        {
            aConnection = null;
            Boolean bReturnValue = true;
            try
            {
                aConnection = CreateConnection(aOpened);
            }
            catch (Exception)
            {
                // Set return to false
                bReturnValue = false;
            }
            return bReturnValue;
        }

#if NET45

        /// <summary>
        /// Returns an opened connection object asynchronously
        /// </summary>
        /// <returns>A SqlConnection object</returns>
        /// <seealso cref="System.Data.SqlClient.SqlConnection"/>
        public async Task<SqlConnection> CreateConnectionAsync()
        {
            return await CreateConnectionAsync(true);
        }

        /// <summary>
        /// Returns a connection object asynchronously
        /// </summary>
        /// <param name="aOpened">If true, the connection will be opened before it is returned</param>
        /// <returns>A SqlConnection object</returns>
        /// <seealso cref="System.Data.SqlClient.SqlConnection"/>
        public async Task<SqlConnection> CreateConnectionAsync(Boolean aOpened)
        {
            return await Task.Factory.StartNew<SqlConnection>(() => CreateConnection(aOpened));
        }

#endif

        /// <summary>
        /// Checks the state of the connection.
        /// If the connection state is closed or broken,
        /// remove the connection from the tracking list
        /// and attempt to do some clean up.
        /// </summary>
        /// <param name="aSender">The SqlConnection object that raised the event</param>
        /// <param name="aEventArguments">This object represents the current state of the SqlConnection Object.</param>
        private void fStateChange(Object aSender, StateChangeEventArgs aEventArguments)
        {
            SqlConnection oConnection = aSender as SqlConnection;
            object oHelperObject = null;
            switch (aEventArguments.CurrentState)
            {
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                    // if (iLogging) fAddLogEntry(new SqlLogItem(String.Format("Connection state is {0}", aEventArguments.CurrentState), Statement.Empty, Environment.StackTrace));
                    break;

                case ConnectionState.Broken:
                case ConnectionState.Closed:
                    if (oConnection != null)
                    {
                        iOpenConnections.TryRemove(oConnection, out oHelperObject);
                        oConnection.StateChange -= fStateChange;
                        // if (iLogging) fAddLogEntry(new SqlLogItem(String.Format("Connection cleaned up and removed (State: {0})", aEventArguments.CurrentState), Statement.Empty, Environment.StackTrace));
                    }
                    break;
            }
        }

        /// <summary>
        /// A structure that contains a SQL statement
        /// </summary>
        /// <seealso cref="System.Data.CommandType"/>
        /// <seealso cref="System.Data.SqlClient.SqlParameter"/>
        public struct Statement
        {
            /// <summary>
            /// The SQL statement to execute
            /// </summary>
            public string Sql { get; set; }

            /// <summary>
            /// The type of SQL statement being passed
            /// </summary>
            public CommandType Type { get; set; }

            /// <summary>
            /// A list of parameters that will be used by the statement
            /// </summary>
            public IEnumerable<SqlParameter> Parameters { get; set; }

            /// <summary>
            /// Returns an empty statement object
            /// </summary>
            public static Statement Empty
            {
                get
                {
                    return new Statement { Parameters = null, Sql = null, Type = CommandType.Text };
                }
            }
        }

        #endregion Utilities
    }
}