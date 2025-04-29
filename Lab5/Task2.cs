using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.PortableExecutable;
using static System.Console;
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
            List<string[]> data_list = ReadFile();
            Person[] persons = ParsePersonData(data_list);
            GenderChange(ref persons);
            string header = FormatColumnsAndHeaderReturn(ref persons);
            RewriteFile(persons, header);
        }

        public static List<string[]> ReadFile()
        {
            List<string[]> data_list = new();
            StreamReader? file = null;
            try
            {
                file = new("input.txt");
            }
            catch (IOException exc)
            {
                WriteLine("Помилка читання файлу:\n" + exc.Message);
                Program.Main();
            }
            string line;
            while ((line = file?.ReadLine()!) != null)
            {
                string[] row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                data_list.Add(row);
            }
            return data_list;
        }
        static Person[] ParsePersonData(List<string[]> data_list)
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

        static void GenderChange(ref Person[] persons)
        {
            for (int i = 0; i < persons.Length; i++)
            {
                if (persons[i].Gender == "M" || persons[i].Gender == "М")
                    persons[i].Gender = "Ч";
                else if (persons[i].Gender == "F")
                    persons[i].Gender = "Ж";
            }
        }

        static string FormatColumnsAndHeaderReturn(ref Person[] persons)
        {
            int[] maxLengths = new int[3];

            foreach (var person in persons)
            {
                maxLengths[0] = Math.Max(maxLengths[0], person.FirstName.Length);
                maxLengths[1] = Math.Max(maxLengths[1], person.LastName.Length);
                maxLengths[2] = Math.Max(maxLengths[2], person.Patronymic.Length);
            }
            maxLengths[0] = Math.Max(maxLengths[0], 9);
            maxLengths[1] = Math.Max(maxLengths[1], 8);
            maxLengths[2] = Math.Max(maxLengths[2], 10);

            for (int i = 0; i < persons.Length; i++)
            {
                persons[i].FirstName = $" {persons[i].FirstName.PadRight(maxLengths[0])} ";
                persons[i].LastName = $" {persons[i].LastName.PadRight(maxLengths[1])} ";
                persons[i].Patronymic = $" {persons[i].Patronymic.PadRight(maxLengths[2])} ";
                persons[i].Gender = $"    {persons[i].Gender}   ";
                persons[i].DateOfBirth = $"  {persons[i].DateOfBirth} ";
                persons[i].MathsScore = $"     {persons[i].MathsScore}      ";
                persons[i].PhysicsScore = $"      {persons[i].PhysicsScore}       ";
                persons[i].ComputerScienceScore = $"          {persons[i].ComputerScienceScore}           ";
                persons[i].Scholarship = $" {persons[i].Scholarship.PadLeft(6)} грн. ";
            }

            return $"|{"FirstName".PadLeft((maxLengths[0] + 9) / 2 + 1).PadRight(maxLengths[0] + 2)}|" +
                $"{"LastName".PadLeft((maxLengths[1] + 8) / 2 + 1).PadRight(maxLengths[1] + 2)}|" +
                $"{"Patronymic".PadLeft((maxLengths[2] + 10) / 2 + 1).PadRight(maxLengths[2] + 2)}|" +
                $" Gender |" +
                $" DateOfBirth |" +
                $" MathsScore |" +
                $" PhysicsScore |" +
                $" ComputerScienceScore |" +
                $" Scholarship |";
        }
        static void RewriteFile(Person[] persons, string header)
        {
            FileStream file;
            try
            {
                file = new FileStream("data_new.txt", FileMode.Create);
            }
            catch (IOException exc)
            {
                WriteLine("Помилка створення файлу:\n" + exc.Message);
                return;
            }
            StreamWriter fstr_out = new StreamWriter(file);
            string border = new string('-', header.Length - 2);
            fstr_out.WriteLine($"┌{border}┐");
            fstr_out.WriteLine(header);
            fstr_out.WriteLine($"|{border}|");
            foreach (Person p in persons)
            {
                fstr_out.Write("|" + p.FirstName + "|");
                fstr_out.Write(p.LastName + "|");
                fstr_out.Write(p.Patronymic + "|");
                fstr_out.Write(p.Gender + "|");
                fstr_out.Write(p.DateOfBirth + "|");
                fstr_out.Write(p.MathsScore + "|");
                fstr_out.Write(p.PhysicsScore + "|");
                fstr_out.Write(p.ComputerScienceScore + "|");
                fstr_out.Write(p.Scholarship + "|");
                fstr_out.WriteLine();
            }
            fstr_out.WriteLine($"└{border}┘");
            fstr_out.Close();
        }
    }
}
