using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareerCloud.ADODataAccessLayer
{
    public class CompanyLocationRepository : IDataRepository<CompanyLocationPoco>
    {
        public void Add(params CompanyLocationPoco[] items)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager
                                                           .ConnectionStrings["dbconnection"]
                                                           .ConnectionString
                                                           ))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                foreach (CompanyLocationPoco item in items)
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[Company_Locations]
                        ([Id]
                        ,[Company]
                        ,[Country_Code]
                        ,[State_Province_Code]
                        ,[Street_Address]
                        ,[City_Town]
                        ,[Zip_Postal_Code]
                        )
                    VALUES 
                        (@Id
                        ,@Company
                        ,@Country_Code
                        ,@State_Province_Code
                        ,@Street_Address
                        ,@City_Town
                        ,@Zip_Postal_Code)";

                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Company", item.Company);
                    cmd.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    cmd.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    cmd.Parameters.AddWithValue("@Street_Address", item.Street);
                    cmd.Parameters.AddWithValue("@City_Town", item.City);
                    cmd.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);

                    conn.Open();
                    int rowEffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyLocationPoco> GetAll(params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection conn = new SqlConnection(
                                                           ConfigurationManager
                                                           .ConnectionStrings["dbconnection"]
                                                           .ConnectionString
                                                           ))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT 
                                        [Id]
                                        ,[Company]
                                        ,[Country_Code]
                                        ,[State_Province_Code]
                                        ,[Street_Address]
                                        ,[City_Town]
                                        ,[Zip_Postal_Code]
                                        ,[Time_Stamp]
                                    FROM
                                        [JOB_PORTAL_DB].[dbo].[Company_Locations]";
                conn.Open();

                int x = 0;
                SqlDataReader rdr = cmd.ExecuteReader();
                CompanyLocationPoco[] appPocos = new CompanyLocationPoco[1000];

                while (rdr.Read())
                {
                    CompanyLocationPoco poco = new CompanyLocationPoco();
                    poco.Id = rdr.GetGuid(0);
                    poco.Company = rdr.GetGuid(1);
                    poco.CountryCode = rdr.GetString(2);
                    //poco.Province = rdr.GetString(3);
                    poco.Province = rdr.IsDBNull(3) ? string.Empty : rdr.GetString(3);
                    //poco.Street = rdr.GetString(4);
                    poco.Street = rdr.IsDBNull(4) ? string.Empty : rdr.GetString(4);
                    //poco.City = rdr.GetString(5);
                    poco.City = rdr.IsDBNull(5) ? string.Empty : rdr.GetString(5);
                    //poco.PostalCode = rdr.GetString(6);
                    poco.PostalCode = rdr.IsDBNull(6) ? string.Empty : rdr.GetString(6);
                    poco.TimeStamp = (byte[])rdr[7];

                    appPocos[x] = poco;
                    x++;
                }
                return appPocos.Where(a => a != null).ToList(); //filtering all the wasited array

                //conn.Close();
            }
        }

        public IList<CompanyLocationPoco> GetList(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyLocationPoco GetSingle(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyLocationPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyLocationPoco[] items)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager
                                                           .ConnectionStrings["dbconnection"]
                                                           .ConnectionString
                                                           ))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                foreach (CompanyLocationPoco item in items)
                {
                    cmd.CommandText = $"DELETE FROM Company_Locations WHERE ID=@ID";
                    cmd.Parameters.AddWithValue("@ID", item.Id);
                    conn.Open();
                    int rowEffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public void Update(params CompanyLocationPoco[] items)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager
                                                           .ConnectionStrings["dbconnection"]
                                                           .ConnectionString
                                                           ))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                foreach (CompanyLocationPoco item in items)
                {
                    cmd.CommandText = @"UPDATE [dbo].[Company_Locations]
                        SET
                        [Company] = @Company,
                        [Country_Code] = @Country_Code,
                        [State_Province_Code] = @State_Province_Code,
                        [Street_Address] = @Street_Address,
                        [City_Town] = @City_Town,
                        [Zip_Postal_Code] = @Zip_Postal_Code
                    WHERE 
                        [Id] = @Id";


                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Company", item.Company);
                    cmd.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    cmd.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    cmd.Parameters.AddWithValue("@Street_Address", item.Street);
                    cmd.Parameters.AddWithValue("@City_Town", item.City);
                    cmd.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);
                    
                    conn.Open();
                    int rowEffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
