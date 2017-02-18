using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDataAccessLibrary
{
    /// <summary>
    /// Following classes represent structure of each table 
    /// Using them you could put information into DataBase as an object of current class 
    /// </summary>
    public class TypeOfPrint
    {
        public int PrintId { get; set; }
        public string NameOfPrint { get; set; }
    }

    public class Books
    {
        public int PrintedInstanceId { get; set; }
        public int PrintId { get; set; }
        public string Title { get; set; }
        public bool IsOccupied { get; set; }
        public int Price { get; set; }
    }

    public class Customers
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
    }

    public class Authors
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class BooksAndAuthors
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
    }

    public class Shops
    {
        public int ShopId { get; set; }
        public string ShopTitle { get; set; }
        public string ShopAddress { get; set; }
        public string ShopMail { get; set; }
        public string ShopPhone { get; set; }
        public string ShopBankAccount { get; set; }
    }

    public class Sales
    {
        public int SalesId { get; set; }
        public int PrintedUnitId { get; set; }
        public int CustomerId { get; set; }
        public int ShopId { get; set; }
        public DateTime DateOfOperation { get; set; }
    }

    /// <summary>
    /// Following classe represent information about titles of each columns in tables in BookStoreDB
    /// </summary>
    public static class DataProvider
    {
        public static string SqlServer
        {
            get { return string.Format("System.Data.SqlClient"); }
        }

        public static string OleDb
        {
            get { return string.Format("System.Data.OleDb"); }
        }

        public static string Odbc
        {
            get { return string.Format("System.Data.Odbc"); }
        }
    }

    public static class TableFromBookStoreDb
    {
        public static string Authors
        {
            get { return string.Format("Authors"); }
        }

        public static string BooksAndAuthors
        {
            get { return string.Format("BooksAndAuthors"); }
        }

        public static string Customers
        {
            get { return string.Format("Customers"); }
        }

        public static string TypeOfPrint
        {
            get { return string.Format("TypeOfPrint"); }
        }

        public static string Shops
        {
            get { return string.Format("Shops"); }
        }

        public static string Books
        {
            get { return string.Format("Books"); }
        }

        public static string Sales
        {
            get { return string.Format("Sales"); }
        }
    }

    public static class BooksTableField
    {
        public static string BookId
        {
            get { return string.Format("BookId"); }
        }

        public static string Title
        {
            get { return string.Format("Title"); }
        }
    }

    public static class AuthorsTableField
    {
        public static string AuthorId
        {
            get { return string.Format("AuthorId"); }
        }

        public static string LastName
        {
            get { return string.Format("LastName"); }
        }
    }

    public static class CustomersTableField
    {
        public static string CustomerId
        {
            get { return string.Format("CustomerId"); }
        }

        public static string Cus_LastName
        {
            get { return string.Format("Cus_LastName"); }
        }
    }

    public static class ShopsTableField
    {
        public static string ShopId
        {
            get { return string.Format("ShopId"); }
        }

        public static string ShopTitle
        {
            get { return string.Format("ShopTitle"); }
        }
    }

    public static class BooksAndAuthorsTableField
    {
        public static string BookId
        {
            get { return string.Format("BookId"); }
        }

        public static string AuthorId
        {
            get { return string.Format("AuthorId"); }
        }
    }

    public static class SalesTableField
    {

        public static string BookId
        {
            get { return string.Format("BookId"); }
        }
        public static string ShopId
        {
            get { return string.Format("ShopId"); }
        }

        public static string CustomerId
        {
            get { return string.Format("CustomerId"); }
        }

        public static string DateOfOperation
        {
            get { return string.Format("DateOfOperation"); }
        }

    }

    /// <summary>
    /// Class DataService represents logic of working with different types of Data Providers. 
    /// Class uses data providers factories 
    /// Class is independent of current type of dataadapter 
    /// </summary>
    public class DataService
    {
        private IDbConnection connection = null;

        private string strConnectionString;

        /// <summary>
        /// Method returns current type of data connection 
        /// </summary>
        /// <param name="strProvider">class for Providers</param>
        /// <returns>connection element</returns>
        private IDbConnection GetConnection(string strProvider)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(strProvider);
            IDbConnection connectionDb = factory.CreateConnection();
            return connectionDb;
        }

        /// <summary>
        /// Method returns your current connection string from App.config file 
        /// </summary>
        /// <param name="strProvider">Your data provider</param>
        /// <returns></returns>
        public void SetConnectionString(string strProvider)
        {
            strConnectionString = ConfigurationManager.ConnectionStrings[strProvider].ConnectionString;
        }

        /// <summary>
        /// Method opens connection to current DB
        /// </summary>
        /// <param name="connectionString">string for connection with DB</param>
        /// <param name="strProvider">class for Providers</param>
        public void OpenConnection(string strProvider)
        {
            connection = GetConnection(strProvider);
            connection.ConnectionString = strConnectionString;
            connection.Open();
        }


        /// <summary>
        /// Method closes current connection
        /// </summary>
        public void CloseConnection()
        {
            connection.Close();
        }

        /// <summary>
        /// Method returns rows and columns information from inserted datatable 
        /// </summary>
        /// <param name="strTableName">Title of table </param>
        /// <returns></returns>
        public DataTable SelectAllFromTable(string strTableName)
        {
            DataTable dataTable = new DataTable();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM " +
                                                strTableName);
                var dr = cmd.ExecuteReader();
                dataTable.Load(dr);
            }
            return dataTable;
        }

        /// <summary>
        /// Method returns data from direct table using direct column
        /// </summary>
        /// <param name="strTableName">Title of direct table </param>
        /// <param name="strFieldName">Name of column in table </param>
        /// <param name="strFieldValue">Value in current tables column</param>
        /// <returns></returns>
        public DataTable SelectFromSingleTable(string strTableName, string strFieldName, string strFieldValue)
        {
            DataTable dataTable = new DataTable();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM " +
                                                strTableName +
                                                " WHERE " +
                                                strFieldName +
                                                " = @Value");
                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = string.Format("@Value");
                param.Value = strFieldValue;
                param.DbType = DbType.String;
                param.Size = 50;
                cmd.Parameters.Add(param);

                var dr = cmd.ExecuteReader();
                dataTable.Load(dr);
            }
            return dataTable;
        }

        /// <summary>
        /// Method returns data from direct table using direct column
        /// </summary>
        /// <param name="strTableName">Title of direct table</param>
        /// <param name="strFieldName">Name of column in table</param>
        /// <param name="iFieldValue">Value in current tables column</param>
        /// <returns></returns>
        public DataTable SelectFromSingleTable(string strTableName, string strFieldName, int iFieldValue)
        {
            DataTable dataTable = new DataTable();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM " +
                                                strTableName +
                                                " WHERE " +
                                                strFieldName +
                                                " = @Value");
                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = string.Format("@Value");
                param.Value = iFieldValue;
                param.DbType = DbType.Int32;
                cmd.Parameters.Add(param);

                var dr = cmd.ExecuteReader();
                dataTable.Load(dr);
            }
            return dataTable;
        }

        /// <summary>
        /// Method returns data from table using relations "many-to-many"
        /// </summary>
        /// <param name="strParentTableName">Title of parental table</param>
        /// <param name="strParentFieldName">Name of field in parental table for connection with "many-to-many" table</param>
        /// <param name="strFieldValue">Value of searched field </param>
        /// <param name="strChildTableName"> Title of child table </param>
        /// <param name="strChildFieldName"> Name of column in child table with searched value</param>
        /// <param name="strManyToManyTable">Title of "many-to-many" table </param>
        /// <param name="strManyToManyField">Field in "many-to-many" table for connection with related table</param>
        /// <returns></returns>
        public DataTable SelectFromTablesManyToMany(string strParentTableName, string strParentFieldName,
            string strFieldValue, string strChildTableName, string strChildFieldName, string strManyToManyTable,
            string strManyToManyField)
        {
            DataTable dataTable = new DataTable();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM " +
                                                strParentTableName +
                                                " WHERE " +
                                                strParentFieldName +
                                                " IN (SELECT " +
                                                strParentFieldName +
                                                " FROM " +
                                                strManyToManyTable +
                                                " WHERE " +
                                                strManyToManyField +
                                                " IN (SELECT " +
                                                strManyToManyField +
                                                " FROM " +
                                                strChildTableName +
                                                " WHERE " +
                                                strChildFieldName +
                                                " = @" +
                                                strFieldValue +
                                                "))");

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = string.Format("@" + strFieldValue);
                param.Value = strFieldValue;
                param.DbType = DbType.String;
                param.Size = 50;
                cmd.Parameters.Add(param);

                var dr = cmd.ExecuteReader();
                dataTable.Load(dr);
            }
            return dataTable;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnectedTableName"> Table for connection other two tables</param>
        /// <param name="strConnectedFieldFirst"> Field in conn table for connection with first table </param>
        /// <param name="strConnectedFieldSecond"> Field in conn table for connection with second table </param>
        /// <param name="strConnectedFieldAdditional"> Addittional field</param>
        /// <param name="strTableNameFirst">Title of first table </param>
        /// <param name="strFieldTableFirst"> Column name in first table </param>
        /// <param name="strTableNameSecond"> Title of second table </param>
        /// <param name="strFieldTableSecond"> Column name in second table </param>
        /// <param name="strValue"> inserted value </param>
        /// <returns></returns>
        public DataTable JoinFromTables(string strConnectedTableName, string strConnectedFieldFirst, string strConnectedFieldSecond,
            string strConnectedFieldAdditional, string strTableNameFirst, string strFieldTableFirst, string strTableNameSecond, 
            string strFieldTableSecond, string strValue)
        {
            DataTable dataTable = new DataTable();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT first." +
                                                strFieldTableFirst +
                                                ", first." +
                                                strConnectedFieldFirst +
                                                ", second." +
                                                strFieldTableSecond +
                                                ", connect." +
                                                strConnectedFieldAdditional +
                                                " FROM " +
                                                strTableNameFirst +
                                                " first " +
                                                " JOIN " +
                                                strConnectedTableName +
                                                " connect ON connect." +
                                                strConnectedFieldFirst +
                                                " = first." +
                                                strConnectedFieldFirst +
                                                " JOIN " +
                                                strTableNameSecond +
                                                " second ON second." +
                                                strConnectedFieldSecond +
                                                "= connect." +
                                                strConnectedFieldSecond +
                                                " WHERE second." +
                                                strFieldTableSecond +
                                                "=@" + strValue);
                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = string.Format("@" + strValue);
                param.Value = strValue;
                param.DbType = DbType.String;
                param.Size = 50;
                cmd.Parameters.Add(param);

                var dr = cmd.ExecuteReader();
                dataTable.Load(dr);
            }
            return dataTable;
        }
    }
}