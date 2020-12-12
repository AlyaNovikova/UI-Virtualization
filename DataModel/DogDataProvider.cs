using DogDatabase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogData
{
    public class DogDataProvider
    {
        public static IList<DogDTO> DataSegment(int start, int cnt)
        {
            var query = SqlQueryConverter.GetCommand(Tables.Dogs, "DataSegment");

            SqlParameter[] args = new SqlParameter[]
            {
                new SqlParameter("@start", start),
                new SqlParameter("@cnt", cnt)
            };

            var data = DogDataMapper.GetData(new DogDataMapper(), query, args);
            return data;
        }

        public static int NumberOfDogs()
        {
            var query = SqlQueryConverter.GetCommand(Tables.Dogs, "NumberOfDogs");

            return DogDataMapper.GetDataSize(new DogDataMapper(), query);
        }

        public static IList<DogDTO> AllDogs()
        {
            var query = SqlQueryConverter.GetCommand(Tables.Dogs, "AllDogs");

            return DogDataMapper.GetData(new DogDataMapper(), query);
        }

        public static DogDTO DogById(int dogId)
        {
            string query = SqlQueryConverter.GetCommand(Tables.Dogs, "DogById");
            SqlParameter args = new SqlParameter("@dogId", dogId);

            return DogDataMapper.GetItem(new DogDataMapper(), query, args);
        }
    }
}
