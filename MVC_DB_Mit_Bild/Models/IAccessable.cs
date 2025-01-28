using System;

namespace MVC_DB_Mit_Bild.Models
{
    public interface IAccessable
    {
        List<Person> GetAllPerson();
        Person GetPersonById(int pid);
        bool DeletePersonById(int pid);
        bool UpdatePerson(Person person);
        int InsertPerson(Person person);
    }
}
