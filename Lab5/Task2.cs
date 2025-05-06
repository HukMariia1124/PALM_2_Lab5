using System;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.PortableExecutable;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using Person =
(
    string FirstName,
    string LastName,
    string Patronymic,
    string Gender,
    string DateOfBirth,
    string MathsScore,
    string PhysicsScore,
    string ComputerScienceScore,
    string Scholarship
);


namespace Lab5
{
    public static class Task2
    {
        public static void Processing()
        {
            List<Person> persons = ReadFile();
            GenderChange(ref persons);
            RewriteFile(persons);
        }

        public static List<Person> ReadFile()
        {
            List<Person> data_list = new();
            StreamReader? file = null;
            try
            {
                file = new("input.txt");
            }
            catch (IOException exc)
            {
                Console.WriteLine("Помилка читання файлу:\n" + exc.Message);
                Program.Main();
            }
            string line;
            while ((line = file?.ReadLine()!) != null)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                Person row = (parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6], parts[7], parts[8]);
                data_list.Add(row);
            }
            return data_list;
        }
        public static Person[] ParsePersonData(List<string[]> data_list)
        {
            int n = data_list.Count;
            Person[] persons = new Person[n];
            for (int i = 0; i < n; i++)
            {
                persons[i] = (
                    data_list[i][0],
                    data_list[i][1],
                    data_list[i][2],
                    data_list[i][3],
                    data_list[i][4],
                    data_list[i][5],
                    data_list[i][6],
                    data_list[i][7],
                    data_list[i][8]
                );
            }
            return persons;
        }

        static void GenderChange(ref List<Person> persons)
        {
            for (int i = 0; i < persons.Count; i++)
            {
                var person = persons[i];
                person.Gender = person.Gender switch
                {
                    "M" or "М" => "Ч",
                    "F" => "Ж",
                    _ => person.Gender
                };
                persons[i] = person;
            }
        }


        static string GetTable(ref List<Person> persons)
        {
            int[] maxLengths = [ 9, 8, 10 ]; //FirstName-9 characters, LastName-8, Patronymic-10.

            foreach (var person in persons)
            {
                maxLengths[0] = Math.Max(maxLengths[0], person.FirstName.Length);
                maxLengths[1] = Math.Max(maxLengths[1], person.LastName.Length);
                maxLengths[2] = Math.Max(maxLengths[2], person.Patronymic.Length);
            }


            var header =
                $"|{"FirstName".PadLeft((maxLengths[0] + 9) / 2 + 1).PadRight(maxLengths[0] + 2)}|" +
                $"{"LastName".PadLeft((maxLengths[1] + 8) / 2 + 1).PadRight(maxLengths[1] + 2)}|" +
                $"{"Patronymic".PadLeft((maxLengths[2] + 10) / 2 + 1).PadRight(maxLengths[2] + 2)}|" +
                $" Gender |" +
                $" DateOfBirth |" +
                $" MathsScore |" +
                $" PhysicsScore |" +
                $" ComputerScienceScore |" +
                $" Scholarship |";


            string border = new('-', header.Length - 2);
            var sb = new StringBuilder(
                $"┌{border}┐\n" +
                $"{header}\n" +
                $"|{border}|\n");

            foreach (Person p in persons)
                sb.Append($"| {p.FirstName.PadRight(maxLengths[0])} |" +
                    $" {p.LastName.PadRight(maxLengths[1])} |" +
                    $" {p.Patronymic.PadRight(maxLengths[2])} |" +
                    $" {p.Gender.PadLeft(3).PadRight(6)} |" +
                    $" {p.DateOfBirth.PadLeft(11)} |" +
                    $" {p.MathsScore.PadLeft(5).PadRight(10)} |" +
                    $" {p.PhysicsScore.PadLeft(6).PadRight(12)} |" +
                    $" {p.ComputerScienceScore.PadLeft(10).PadRight(20)} |" +
                    $" {p.Scholarship.PadLeft(6)} грн. |\n");

            sb.Append($"└{border}┘");

            return sb.ToString();
        }
        static void RewriteFile(List<Person> persons)
        {
            FileStream file;
            try
            {
                file = new FileStream("data_new.txt", FileMode.Create);
            }
            catch (IOException exc)
            {
                Console.WriteLine("Помилка створення файлу:\n" + exc.Message);
                return;
            }
            using StreamWriter fstr_out = new(file);
            fstr_out.WriteLine(GetTable(ref persons));

        }
    }
}
