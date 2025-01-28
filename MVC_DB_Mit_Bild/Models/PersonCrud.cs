
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace MVC_DB_Mit_Bild.Models
{
    public class PersonCrud : IAccessable
    {
        private readonly SqlConnection conn;
        public PersonCrud(string connString)
        {
            conn = new SqlConnection(connString);
        }
        public bool DeletePersonById(int pid)
        {
            string sqlDeleteString = "DELETE FROM Person WHERE id = @pid";
            SqlCommand deleteCmd = new SqlCommand(sqlDeleteString, conn);
            deleteCmd.Parameters.AddWithValue("@pid", pid);
            conn.Open();
            int rowsAffected = deleteCmd.ExecuteNonQuery();
            conn.Close();
            return rowsAffected == 1;
        }

        public List<Person> GetAllPerson()
        {
            List<Person> personList = new List<Person>();

            string sqlSelectAll = "SELECT * FROM Person";
            SqlCommand selectCmd = new SqlCommand(sqlSelectAll, conn);

            conn.Open();

            SqlDataReader reader = selectCmd.ExecuteReader();

            //Lesen der Ergebnisse und Hinzufügen der Boote zur Liste
            while (reader.Read())
            {
                Person person = new Person() {
                    PID = (int)reader["id"],
                    Vorname = reader["Vorname"].ToString(),
                    Nachname = reader["Nachname"].ToString(),
                    Geburtsdatum = reader["Geburtsdatum"] as DateTime?,
                    groesse = reader["groesse"] as int?,
                    //Bild = (string)reader["Bild"],
                    Gewicht = reader["Gewicht"] as double?
                };
                if (reader["Bild"].ToString() != "")
                {
                    person.Bild = reader["Bild"].ToString();
                } else
                {
                    person.Bild = "keinBild.png";
                }

                //Hinzufügen des Bootes zur Liste
                personList.Add(person);
            }

            //Schließen der Verbindung zur Datenbank
            conn.Close();

            //Rückgabe der Liste
            return personList;
        }

        public Person GetPersonById(int pid)
        {
            string sqlSelectById = "SELECT * FROM Person WHERE id = @pid";
            SqlCommand selectCmd = new SqlCommand(sqlSelectById, conn);
            selectCmd.Parameters.AddWithValue("@pid", pid);
            conn.Open();
            SqlDataReader reader = selectCmd.ExecuteReader();
            reader.Read();
            Person person = new Person
            {
                PID = (int)reader["id"],
                Vorname = reader["Vorname"].ToString(),
                Nachname = reader["Nachname"].ToString(),
                Geburtsdatum = reader["Geburtsdatum"] as DateTime?,
                groesse = reader["groesse"] as int?,
                //Bild = (string)reader["Bild"],
                Gewicht = reader["Gewicht"] as double?
            };
            if (reader["Bild"].ToString() != "")
            {
                person.Bild = reader["Bild"].ToString();
            }
            else
            {
                person.Bild = "keinBild.png";
            }
            conn.Close();
            return person;
        }

        public int InsertPerson(Person person)
        {
            string sqlInsertString = "INSERT INTO Person (Vorname, Nachname, Geburtsdatum, Groesse, Bild, Gewicht) " +
                "VALUES (@Vorname, @Nachname, @Geburtsdatum, @Groesse, @Bild, @Gewicht); SELECT SCOPE_IDENTITY()";
            SqlCommand insertCmd = new SqlCommand(sqlInsertString, conn);
            insertCmd.Parameters.AddWithValue("@Vorname", person.Vorname);
            insertCmd.Parameters.AddWithValue("@Nachname", person.Nachname);
            insertCmd.Parameters.AddWithValue("@Geburtsdatum", person.Geburtsdatum == null ? DBNull.Value : person.Geburtsdatum);
            insertCmd.Parameters.AddWithValue("@Groesse", person.groesse == null ? DBNull.Value : person.groesse);
            insertCmd.Parameters.AddWithValue("@Bild", (object)person.Bild == null ? DBNull.Value : person.Bild);
            insertCmd.Parameters.AddWithValue("@Gewicht", person.Gewicht == null ? DBNull.Value : person.Gewicht);
            //insertCmd.Parameters.AddWithValue("@Gewicht", (object)boat.ConstructionYear ?? DBNull.Value);
            conn.Open();
            int newId = int.Parse(insertCmd.ExecuteScalar().ToString());
            conn.Close();
            return newId;
        }

        public bool UpdatePerson(Person person)
        {
            string sqlUpdateString = "UPDATE Person SET Vorname = @Vorname, Nachname = @Nachname, Geburtsdatum = @Geburtsdatum, Groesse = @Groesse, Bild = @Bild, Gewicht = @Gewicht  WHERE id = @pid";
            SqlCommand updateCmd = new SqlCommand(sqlUpdateString, conn);
            updateCmd.Parameters.AddWithValue("@Vorname", person.Vorname);
            updateCmd.Parameters.AddWithValue("@Nachname", person.Nachname);
            updateCmd.Parameters.AddWithValue("@Geburtsdatum", (object)person.Geburtsdatum ?? DBNull.Value);
            updateCmd.Parameters.AddWithValue("@Groesse", (object)person.groesse ?? DBNull.Value);
            updateCmd.Parameters.AddWithValue("@Bild", (object)person.Bild ?? DBNull.Value);
            updateCmd.Parameters.AddWithValue("@Gewicht", (object)person.Gewicht ?? DBNull.Value);
            updateCmd.Parameters.AddWithValue("@pid", person.PID);
            conn.Open();
            int rowsAffected = updateCmd.ExecuteNonQuery();
            conn.Close();
            return rowsAffected == 1;
        }
    }
}
