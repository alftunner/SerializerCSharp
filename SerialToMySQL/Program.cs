using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DBConnect;

namespace SerialToMySQL
{

    internal static class Program
    {
        static async Task Main()
        {
            DBCon.Open();

            const string SQL = "SELECT * FROM table_student";
            MySqlCommand command = new MySqlCommand();
            var result = DBCon.SelectQuery(SQL);

            List<Student> students = new List<Student>();

            while (result.Read())
            {
                Student student = new Student();
                student.Id = result.GetInt32("id");
                student.Name = result.GetString("name");
                student.IsStudy = result.GetBoolean("is_study");
                student.Birthday = result.GetDateTime("birthday");
                students.Add(student);
            }

            await using (FileStream fileWrite = new FileStream("students.json", FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync(fileWrite, students);
            }
            DBCon.Close();

            
            var studentsTemp = new List<Student>();

            await using (var fileRead = new FileStream("students.json", FileMode.Open))
            {
                studentsTemp = JsonSerializer.DeserializeAsync<List<Student>>(fileRead).Result;
            }
            

            foreach (var student in studentsTemp)
            {
                string SQL_INSERT = $"INSERT INTO table_student (name, is_study, birthday) VALUES ('{student.Name}', '{Convert.ToInt32(student.IsStudy)}', '{student.Birthday.ToString("yy-MM-dd")}');";
                string SQL_CHECK = "SELECT * FROM table_student";
                MySqlCommand command_check = new MySqlCommand();
                DBCon.Open();
                var result_check = DBCon.SelectQuery(SQL_CHECK);
                var isSame = false;

                while (result_check.Read())
                {
                    if(student.Name == result_check.GetString("name") && student.IsStudy == result_check.GetBoolean("is_study") && student.Birthday == result_check.GetDateTime("birthday"))
                    {
                        isSame = true;
                    }
                }
                DBCon.Close();
                if (isSame == false)
                {
                    DBCon.Open();
                    if (DBCon.InsertQuery(SQL_INSERT))
                    {
                        Console.WriteLine("Все данные добавлены в БД");
                    }
                    else
                    {
                        Console.WriteLine("Данные не добавлены в БД");
                    }
                }
                else
                {
                    Console.WriteLine("Данные не добавлены в БД по причине существования идентичных");
                }
                DBCon.Close();
            }
        }
    }
}
