using System.ComponentModel.DataAnnotations;


namespace Test1DAL
{
    public abstract class Entity
    {
        [Key]

        public int ID {get; set; }
    }
}
