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
            if (p.MathsScore == "-") maths = 0;
            else maths = int.Parse(p.MathsScore);
            if (p.PhysicsScore == "-") physics = 0;
            else physics = int.Parse(p.PhysicsScore);
            if (p.ComputerScienceScore == "-") cs = 0;
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
            var persons = new Dictionary<string, List<Person>>();
            foreach (var person in Task2.ReadFile())
            {
                string zodiacSign = GetZodiacSign(DateOnly.Parse(person.DateOfBirth));
                if (!persons.ContainsKey(zodiacSign))
                {
                    persons[zodiacSign] = new List<Person>();
                }
                persons[zodiacSign].Add(person);
            }
            _ = "𓆏";

            foreach (var zodiacSign in persons.Keys) {

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
            int day = date.Day;
            int month = date.Month;

            return (month, day) switch
            {
                (1, >= 20) or (2, <= 18) => "Aquarius",
                (2, >= 19) or (3, <= 20) => "Pisces",
                (3, >= 21) or (4, <= 19) => "Aries",
                (4, >= 20) or (5, <= 20) => "Taurus",
                (5, >= 21) or (6, <= 20) => "Gemini",
                (6, >= 21) or (7, <= 22) => "Cancer",
                (7, >= 23) or (8, <= 22) => "Leo",
                (8, >= 23) or (9, <= 22) => "Virgo",
                (9, >= 23) or (10, <= 22) => "Libra",
                (10, >= 23) or (11, <= 21) => "Scorpio",
                (11, >= 22) or (12, <= 21) => "Sagittarius",
                (12, >= 22) or (1, <= 19) => "Capricorn",
                _ => "Unknown Zodiac Sign"
            };
        }
    }
}
