using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Reflection;
using SusNet.Services.Utils;

namespace SusNet.Services.Da.SusDapper
{
    public class DapperHelp
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string ConnectionString = string.Empty;
        static DapperHelp()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        #region Add
        public static string Add<T>(T obj)
        {
            Object id = null;
            //if (obj is MetaData)
            //{
            //    MetaData metaData = obj as MetaData;
            //    metaData.CompanyId = "c001";
            //    metaData.Deleted = 0;
            //    metaData.InsertDateTime = DateTime.Now;
            //    metaData.InsertUser = UserId;
            //}
            
            var sql = GetInsertSql<T>(obj, out id);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                 connection.Execute(sql,obj);
                log.InfoFormat("Add-->sql={0},参数={1}",sql,Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }

            return id?.ToString();
        }
        #endregion

        #region Edit
        public static int Edit<T>(T obj)
        {
            //if (obj is MetaData)
            //{
            //    MetaData metaData = obj as MetaData;
            //    metaData.CompanyId = "c001";
            //    metaData.UpdateDateTime = DateTime.Now;
            //    metaData.UpdateUser = UserId;

            //    if (!metaData.Deleted.HasValue)
            //        metaData.Deleted = 0;


            //}

            //Stopwatch stopwatch1 = new Stopwatch();


            var sql = GetUpdateSql(obj);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var rows = connection.Execute(sql,obj);
                log.InfoFormat("Edit-->sql={0},参数={1}", sql, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                return rows;
            }
        }


        #endregion

        #region Query
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T QueryForObject<T>(string sql, object obj)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var result= connection.QueryFirstOrDefault<T>(sql, obj);
                log.InfoFormat("QueryForObject-->sql={0},参数={1}", sql, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                return result;
            }
        }
        #endregion

        #region Execute
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(string sql, object obj)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var result= connection.ExecuteScalar<T>(sql, obj);
                log.InfoFormat("ExecuteScalar-->sql={0},参数={1}", sql, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                return result;
            }
        }

        /// <summary>
        /// 例如： SELECT * FROM t WHERE ID = @ID
        /// obj : new Object{ID = ID} 或者对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"> new Object{ID = ID}</param>
        /// <returns></returns>
        public static int Execute(string sql, object obj)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                int rows=connection.Execute(sql, obj);
                log.InfoFormat("Execute-->sql={0},参数={1}", sql, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                return rows;
            }
        }
        #endregion

        #region Query
        /// <summary>
        /// 返回指定实体
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static IEnumerable<U> Query<U>(string sql, object para = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var result= connection.Query<U>(sql, para);
                log.InfoFormat("Query-->sql={0},参数={1}", sql, Newtonsoft.Json.JsonConvert.SerializeObject(para));
                return result;
            }
        }
        /// <summary>
        /// 返回指定实体的List
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static IList<U> QueryForList<U>(string sql, object para = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var result =connection.Query<U>(sql, para).ToList<U>();
                log.InfoFormat("QueryForList-->sql={0},参数={1}", sql, Newtonsoft.Json.JsonConvert.SerializeObject(para));
                return result;
            }
        }

        public static T FirstOrDefault<T>(string sql, object para)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var result= connection.QueryFirstOrDefault<T>(sql, para);
                log.InfoFormat("FirstOrDefault-->sql={0},参数={1}", sql, Newtonsoft.Json.JsonConvert.SerializeObject(para));
                return result;
            }
        }
        #endregion


        #region SqlMap
        public static string GetInsertSql<T>(T t, out Object pk)
        {
            if (null == t)
            {
                var ex = new ApplicationException("实体不能未空!");
                throw ex;
            }
            Type type = t.GetType();
            PropertyInfo[] pds = type.GetProperties().Where(p => p.GetMethod.IsVirtual).ToArray();
            if (pds.Length == 0)
            {
                var ex = new ApplicationException("没有Virtual属性");
                throw ex;
            }

            StringBuilder columns = new StringBuilder();
            StringBuilder valuePara = new StringBuilder();

            foreach (var pi in pds)
            {
                if (columns.Length > 0)
                    columns.Append(",");

                if (valuePara.Length > 0)
                    valuePara.Append(",");

                columns.AppendFormat(pi.Name);

                valuePara.AppendFormat("@{0}", pi.Name);
                //if (!pi.Name.ToLower().Equals("id"))
                //{
                //    sqlStringBuilder.AppendFormat("{0},", pi.Name);
                //}
            }

            pk = GUIDHelper.GetGuidString();
            if (pds != null && pds.Any(o => o.Name.ToLower() == "id"))
            {
                var para = pds.Where(o => o.Name.ToLower() == "id").First();
                para.SetValue(t, pk);
            }
            

            var sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendFormat("INSERT INTO {0} ({1}) values ({2}) ", type.Name, columns, valuePara);

            
            return sqlStringBuilder.ToString();
        }
      
        public static string GetUpdateSql<T>(T t)
        {
            if (null == t)
            {
                var ex = new ApplicationException("实体不能未空!");
                throw ex;
            }
            Type type = t.GetType();
            PropertyInfo[] pds = type.GetProperties().Where(p => p.GetMethod.IsVirtual).ToArray();
            if (pds.Length == 0)
            {
                var ex = new ApplicationException("没有Virtual属性");
                throw ex;
            }

            var sqlStringBuilder = new StringBuilder();
            var builderWhere = new StringBuilder();
            var builderColumn = new StringBuilder();
            foreach (var pi in pds)
            {
                if (pi.Name.ToLower().Equals("id"))
                {
                    if (builderWhere.Length > 0)
                        builderWhere.Append(" And ");

                    builderWhere.Append(" Id = @Id");

                }
                else
                {
                    //if (pi.GetValue(t) != null)
                    builderColumn.AppendFormat("{0}=@{0},", pi.Name);
                }
            }

            sqlStringBuilder.AppendFormat("UPDATE {0} SET {1} WHERE {2}", type.Name, builderColumn.ToString().Trim(','), builderWhere);

            return sqlStringBuilder.ToString();
        }
        #endregion
    }
}
