using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using DogDatabase;

namespace DogData
{
    public interface IDataMapper<out T>
    {
        T MapData(SqlDataReader reader);
        int MapNumber(SqlDataReader reader);
    }

    class DogDataMapper : IDataMapper<DogDTO>
    {
        public DogDTO MapData(SqlDataReader reader)
        {
            return new DogDTO
            {
                DogId = (int) reader["dogId"],
                Breed = (string) reader["breed"],
                Image_data = (byte[]) reader["image_data"]
            };
        }

        public int MapNumber(SqlDataReader reader)
        {
            return (int) reader["count"];
        }

        public static List<T> GetData<T>(IDataMapper<T> mapper, string queryString, params SqlParameter[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var result = new List<T>();

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddRange(args);

                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    result.Add(mapper.MapData(reader));
                }

                reader.Close();

                return result;
            }
        }

        public static int GetDataSize<T>(IDataMapper<T> mapper, string queryString, params SqlParameter[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var result = 0;

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddRange(args);

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();

                result = mapper.MapNumber(reader);

                reader.Close();

                return result;
            }
        }

        public static T GetItem<T>(IDataMapper<T> mapper, string queryString, params SqlParameter[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                T result = default;

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddRange(args);

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                result = mapper.MapData(reader);

                reader.Close();

                return result;
            }
        }
    }
}
