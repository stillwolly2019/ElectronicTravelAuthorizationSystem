using Microsoft.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
namespace TravelAuthorization.Utility
{
    public class DatabaseManager
    {
        private SqlConnection _connection;
        private string _connectionString;

        private SqlTransaction _trans;
        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);
                return _connection;
            }
        }

        public DatabaseManager()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["RefInfoConnectionString"].ConnectionString;
        }
        public DatabaseManager(bool IsMedia)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["RefInfoMediaConnectionString"].ConnectionString;
        }
        public DatabaseManager(bool IsMedia, bool IsActiveDirectory)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["ActiveDirectoryUsersConnectionString"].ConnectionString;
        }
        public DatabaseManager(string ConnectionString)
        {
            this._connectionString = ConnectionString;
        }

        public void close()
        {
            if ((_connection != null) && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public int fillTable(ref SqlCommand cmd, ref DataTable dt)
        {
            int retval = -1;
            dt = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = this.Connection;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (((dt != null)))
                retval = dt.Rows.Count;
            return retval;
        }

        public int fillDS(ref SqlCommand cmd, ref DataSet ds)
        {
            int retval = -1;
            ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = this.Connection;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (((ds != null)))
                retval = ds.Tables[0].Rows.Count;
            return retval;
        }

        public int executeReader(ref SqlCommand cmd, ref SqlDataReader dr)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            return exeReader(ref cmd, ref dr);
        }

        public int executeReader(string sql, ref SqlDataReader dr)
        {
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;
            int retval = executeReader(ref cmd, ref dr);
            return retval;
        }

        private int exeReader(ref SqlCommand cmd, ref SqlDataReader dr)
        {
            int retval = -1;
            cmd.Connection = this.Connection;
            try
            {
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    SqlParameter pr = new SqlParameter("@retval", SqlDbType.Int);
                    pr.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(pr);
                }
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                dr = cmd.ExecuteReader();

                if (cmd.CommandType == CommandType.StoredProcedure)
                    retval = Convert.ToInt16(cmd.Parameters["@retval"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.close();
            }

            return retval;
        }

        public int executeNonQuery(ref SqlCommand cmd)
        {
            int retval = -1;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = this.Connection;
            try
            {
                SqlParameter pr = new SqlParameter("@retval", SqlDbType.Int);
                pr.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(pr);
                this.open();
                if ((_trans != null))
                    cmd.Transaction = _trans;
                cmd.ExecuteNonQuery();
                retval = Convert.ToInt16(cmd.Parameters["@retval"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (_trans == null)
                    this.close();
            }

            return retval;
        }

        public int executeNonQuery(ref string sql)
        {
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;
            int retval = exeNonQuery(ref cmd);
            return retval;
        }

        private int exeNonQuery(ref SqlCommand cmd)
        {
            int retval = -1;
            cmd.Connection = this.Connection;
            try
            {
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    SqlParameter pr = new SqlParameter("@retval", SqlDbType.Int);
                    pr.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(pr);
                }
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                retval = cmd.ExecuteNonQuery();
                if (cmd.CommandType == CommandType.StoredProcedure)
                    retval = Convert.ToInt16(cmd.Parameters["@retval"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.close();
            }

            return retval;
        }

        public object executeScalar(ref SqlCommand cmd)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            return exeScalar(ref cmd);
        }

        public object executeScalar(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;
            int retval = Convert.ToInt16(executeScalar(ref cmd));
            return retval;
        }

        private object exeScalar(ref SqlCommand cmd)
        {
            object retval = null;
            cmd.Connection = this.Connection;
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                retval = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.close();
            }

            return retval;

        }


        public void beginTransaction()
        {
            if (this.Connection.State == ConnectionState.Closed)
                this.Connection.Open();
            _trans = this.Connection.BeginTransaction();
        }

        public void commitTransaction()
        {
            if ((_trans != null))
            {
                _trans.Commit();
                _trans.Dispose();
                _trans = null;
            }
        }

        public void rollbackTransaction()
        {
            if ((_trans != null))
            {
                _trans.Rollback();
                _trans.Dispose();
                _trans = null;
            }
        }
        private void open()
        {
            if (this.Connection.State == ConnectionState.Closed)
                this.Connection.Open();
        }
    }
}