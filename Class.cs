using System.ComponentModel.DataAnnotations;

namespace mongo1
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Namespaces { get; set; }
    }
}
