using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogData
{
    /// <summary>
    /// Dog class - sample data class for virtualization testing with database
    /// </summary>
    /// 
    public class Dog
    {
        public string Title { get; set; }
        public string Breed { get; set; }
        public byte[] Image { get; set; }
    }

    /// <summary>
    /// Dog class - sample data class for virtualization testing with sample data.
    /// </summary>
    /// 
    public class DogMock
    {
        public string Title { get; set; }
        public string Breed { get; set; }
        public string Image { get; set; }
    }
}
