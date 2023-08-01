using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace mongo1
{
    public class Person
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
