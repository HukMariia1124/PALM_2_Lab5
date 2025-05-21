using System;
using System.Text;
using System.Xml;
using System.Linq;
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
    internal class Additionals
    {
        public static void Add1_Processing()
        {
            List<Person> persons = Task2.ReadFile();
            double average = MaleAverage(persons);
            Console.WriteLine($"Середній бал чоловіків: {average}");
            Output(average, persons);
        }
        static double MaleAverage(List<Person> persons)
        {
            double average = 0;
            int cnt = 0;
            foreach (Person p in persons)
            {
                if (p.Gender == "M" || p.Gender == "М" || p.Gender == "Ч")
                {
                    cnt++;
                    ScoreParse(p, out int maths, out int physics, out int cs);
                    average += (maths + physics + cs) / 3.0;
                }
            }
            average /= cnt;
            return average;
        }

        private static void ScoreParse(Person p, out int maths, out int physics, out int cs)
        {
            if (p.MathsScore == "-") maths = 2;
            else maths = int.Parse(p.MathsScore);
            if (p.PhysicsScore == "-") physics = 2;
            else physics = int.Parse(p.PhysicsScore);
            if (p.ComputerScienceScore == "-") cs = 2;
            else cs = int.Parse(p.ComputerScienceScore);
        }

        static void Output(double average, List<Person> persons)
        {
            double person_average;
            foreach (Person p in persons)
            {
                ScoreParse(p, out int maths, out int physics, out int cs);
                person_average = (maths + physics + cs) / 3.0;
                if ((p.Gender == "Ж" || p.Gender == "F") && person_average > average)
                {
                    Console.WriteLine($"{p.FirstName} {p.LastName} {p.Patronymic}: {person_average}");
                }
            }
        }


        public static void Add2_Processing()
        {
            Dictionary<string, List<Person>> persons = new();
            foreach (Person person in Task2.ReadFile())
            {
                string zodiacSign = GetZodiacSign(DateOnly.Parse(person.DateOfBirth));
                if (!persons.ContainsKey(zodiacSign))
                {
                    persons[zodiacSign] = new List<Person>();
                }
                persons[zodiacSign].Add(person);
            }
            //_ = "𓆏";

            foreach (string zodiacSign in persons.Keys) {

                FileStream file;
                try
                {
                    file = new FileStream($"data_{zodiacSign}.txt", FileMode.Create);
                }
                catch (IOException exc)
                {
                    Console.WriteLine("Помилка створення файлу:\n" + exc.Message);
                    return;
                }
                using StreamWriter fstr_out = new(file);
                foreach(var person in persons[zodiacSign])
                    fstr_out.WriteLine(person);
            }
        }

        public static string GetZodiacSign(DateOnly date)
        {
            if (!File.Exists("ZodiacSigns.txt"))
                return "Unknown Zodiac Sign";

            List<(string Name, (int Month, int Day) Start, (int Month, int Day) End)> zodiacRanges = new();
            foreach (string line in File.ReadLines("ZodiacSigns.txt"))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(':');
                if (parts.Length != 2) continue;
                string name = parts[0].Trim();
                string[] range = parts[1].Trim().Split('-');
                if (range.Length != 2) continue;
                string[] startParts = range[0].Trim().Split('/');
                string[] endParts = range[1].Trim().Split('/');
                if (startParts.Length != 2 || endParts.Length != 2) continue;
                zodiacRanges.Add((name, (int.Parse(startParts[0]), int.Parse(startParts[1])), (int.Parse(endParts[0]), int.Parse(endParts[1]))));
            }

            foreach (var (name, start, end) in zodiacRanges)
            {
                if (start.Month > end.Month) //For Capricorn.
                {
                    if (date.Month == start.Month && date.Day >= start.Day ||
                        date.Month == end.Month && date.Day <= end.Day)
                    {
                        return name;
                    }
                }
                else
                {
                    if ((date.Month > start.Month || (date.Month == start.Month && date.Day >= start.Day)) &&
                        (date.Month < end.Month || (date.Month == end.Month && date.Day <= end.Day)))
                    {
                        return name;
                    }
                }
            }
            return "Unknown Zodiac Sign";
        }
    }
}
