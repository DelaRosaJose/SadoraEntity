using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Sadora.Clases
{
    public static class ClassData
    {
        // NEXT********************************************
        #region creando la cadena de conexion
        public static SqlConnection sqlConnection = new SqlConnection("Server=localhost;DataBase=Sadora;integrated Security=true");
        //"Server=10.0.0.250;DataBase=Sadora;User ID =Admin; Password=llGranmaestro"
        //public static string connectionString(string stringParameterConnection = "connection-string", string stringParameterDB = "SQLServer")
        //{
        //    return ClassAplication.findNodoXMLFile("AppConfig.XML", stringParameterDB, stringParameterConnection);
        //}
        #endregion creando la cadena de conexion

        /*
        #region Abrir y cerrar conexion
        static void OpenConnection()
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();
            }
            catch (Exception ex)
            {
                //new Administracion.FrmCompletarCamposHost(string.Format("Error de conexion: \n {0}",ex)).ShowDialog();
            }
        }
        #endregion
        */

        // NEXT********************************************
        #region para ejecutar consultas como: insert, update y delete
        public static void runSqlCommand(string stringParameterSqlQuery, List<SqlParameter> listSqlParameter = null, string stringParameterComandType = "CommandText")
        {
            Task.Run(() =>
            {
                SqlCommand sqlcommand = new SqlCommand(stringParameterSqlQuery, sqlConnection);

                try
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }

                    sqlcommand.CommandText = stringParameterSqlQuery;
                    sqlcommand.CommandType = stringParameterComandType == "CommandText" ? CommandType.Text : CommandType.StoredProcedure;

                    if (stringParameterComandType != "CommandText")
                    {
                        if (listSqlParameter.Count > 0)
                        {
                            foreach (SqlParameter sqlparameter in listSqlParameter)
                            {
                                sqlcommand.Parameters.Add(sqlparameter);
                            }
                        }
                    }
                    sqlcommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    ClassVariables.GetSetError = string.Format("Ha ocurrido un error: \n {0}", exception.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    sqlcommand.Dispose();
                }
            }).Wait();
            //SqlCommand.Wait();

        }
        #endregion para ejecutar consultas como: insert, update y delete


        // NEXT********************************************
        #region creando datareader
        public static SqlDataReader runSqlDataReader(string stringParameterSqlQuery, List<SqlParameter> listSqlParameter = null, string stringParameterCommandType = "CommandType")
        {
            var DataReader = Task.Run(() =>
            {
                SqlDataReader sqlDataReader = default;
                SqlCommand sqlCommand = new SqlCommand(stringParameterSqlQuery, sqlConnection);

                try
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }

                    sqlCommand.CommandType = stringParameterCommandType == "CommandText" ? CommandType.Text : CommandType.StoredProcedure;

                    if (listSqlParameter != null)
                    {
                        sqlDataReader = null;
                        foreach (SqlParameter sqlParameter in listSqlParameter)
                        {
                            sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    sqlDataReader = sqlCommand.ExecuteReader();
                }
                catch (Exception exception)
                {
                    //ClassVariables.GetSetError = "error" + exception.ToString();
                    ClassVariables.GetSetError = string.Format("Ha ocurrido un error: \n {0}", exception.ToString());
                    //throw new Exception(ClassVariables.GetSetError = string.Format("Ha ocurrido un error: \n {0}", exception.ToString()));
                }
                finally
                {
                    sqlCommand.Dispose();
                }
                return sqlDataReader;
            });
            DataReader.Wait();
            return DataReader.Result;
        }
        #endregion creando data reader


        // NEXT********************************************
        #region creando data Table
        public static DataTable runDataTable(string stringParameterSqlQuery, List<SqlParameter> listParameter = null, string stringParameterCommandType = "CommandText")
        {
            var Datatable = Task.Run(() =>
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable datatable = new DataTable();

                try
                {
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();

                    sqlDataAdapter.SelectCommand = new SqlCommand(stringParameterSqlQuery, sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandType = stringParameterCommandType == "CommandText" ? CommandType.Text : CommandType.StoredProcedure;

                    if (listParameter != null)
                    {
                        foreach (SqlParameter sqlParameter in listParameter)
                        {
                            sqlDataAdapter.SelectCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    sqlDataAdapter.Fill(datatable);
                }
                catch (Exception exception)
                {
                    ClassVariables.GetSetError = string.Format("Ha ocurrido un error: \n {0}", exception.ToString());
                }
                finally
                {
                    sqlDataAdapter.Dispose();
                    sqlConnection.Close();
                }
                return datatable;
            });
            Datatable.Wait();
            return Datatable.Result;
        }
        #endregion creando data table


        // NEXT********************************************
        #region creando data set
        public static DataSet runDataSet(string stringParameterSqlQuery, List<SqlParameter> listSqlParameter = null, string stringParameterCommandType = "CommandText")
        {
            var DataSet = Task.Run(() =>
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataSet dataSet = new DataSet();

                try
                {
                    sqlDataAdapter.SelectCommand = new SqlCommand(stringParameterSqlQuery, sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandType = stringParameterCommandType == "CommandText" ? CommandType.Text : CommandType.StoredProcedure;

                    if (listSqlParameter != null)
                    {
                        foreach (SqlParameter sqlParamter in listSqlParameter)
                        {
                            sqlDataAdapter.SelectCommand.Parameters.Add(listSqlParameter);
                        }
                    }
                    sqlDataAdapter.Fill(dataSet);
                }
                catch (Exception exception)
                {
                    ClassVariables.GetSetError = string.Format("Ha ocurrido un error: \n {0}", exception.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    sqlDataAdapter.Dispose();
                }
                return dataSet;
            });
            DataSet.Wait();
            return DataSet.Result;
        }
        #endregion creando data set


        #region ingresar datatable a Sql Server
        public static void runDataTableSql(string stringParameterSqlQuery, List<ClassVariables> list, string Item)
        {
            var DataTableSql = Task.Run(() =>
            {
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable datatable = new DataTable();

                try
                {
                    datatable = new System.Data.DataTable() { Locale = System.Globalization.CultureInfo.InvariantCulture };
                    datatable.Columns.Add("FormularioID", typeof(int));
                    datatable.Columns.Add("Nombre", typeof(string));
                    datatable.Columns.Add("Modulo", typeof(string));
                    datatable.Columns.Add("Titulo", typeof(string));


                    int contador = 0;
                    foreach (ClassVariables user in list)
                    {
                        datatable.Rows.Add();
                        var id = datatable.Rows.Count - 1;
                        datatable.Rows[contador]["FormularioID"] = contador + 1;//"BarCode" + i;
                        datatable.Rows[contador]["Nombre"] = user.Formulario;//"BarCode" + i;
                        datatable.Rows[contador]["Modulo"] = user.Modulo; //"Name" + i;
                        datatable.Rows[contador]["Titulo"] = user.Titulo;//"Description" + i;
                        contador++;
                    }



                    SqlCommand command = new SqlCommand(stringParameterSqlQuery, sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = new SqlParameter //Esta declaracion es Clave en este Metodo
                    {
                        SqlDbType = SqlDbType.Structured, //El tipo Structured es el que se tomara la estructura del Data Table para
                        ParameterName = Item, //Hacer match con el Tipo Table en SQL, respetando el nombre declarado en el StoreProcedure   //DTItemns
                        Value = datatable
                    };
                    command.Parameters.Add(parameter);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                catch (Exception exception)
                {
                    ClassVariables.GetSetError = string.Format("Ha ocurrido un error: \n {0}", exception.ToString());
                }
                finally
                {
                    //sqlDataAdapter.Dispose();
                    datatable.Clear();
                    datatable.Dispose();
                    sqlConnection.Close();

                }
            });




        }
        #endregion creando data table


    }
}
