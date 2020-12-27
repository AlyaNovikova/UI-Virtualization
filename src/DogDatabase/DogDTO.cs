using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogDatabase
{
    public class DogDTO
    {
        public int DogId { get; set; }

        public string Breed { get; set; }

        public byte[] Image_data { get; set; }
    }

    public static class Tables
    {
        public const string Dogs = "DOGS";
    }
}
