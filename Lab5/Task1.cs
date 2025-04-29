using System.Reflection.PortableExecutable;
using static System.Console;
using MyTime = (int hour, int min, int sec);

namespace Lab5
{
    public static class Task1
    {
        public static void Processing()
        {
            MyTime time = InputTuple();

            WriteLine($"\nМетод MyTimeToString формує рядкове подання часу з кортежа у форматі H:mm:s:\n{MyTimeToString(time)}");
            MyTime normalized_time = Normalize(time);
            WriteLine($"Метод Normalize приводить години до проміжку 0...23, хвилини й секунди до проміжку 0...59:\n{MyTimeToString(normalized_time)}");
            WriteLine($"Метод ToSecSinceMidnight перетворює час {normalized_time} у кількість секунд, що пройшли від початку доби:\n{ToSecSinceMidnight(time)}");
            WriteLine($"Метод FromSecSinceMidnight перетворює кількість секунд, що пройшли від початку доби ({ToSecSinceMidnight(normalized_time)}), у кортеж MyTime:\n{MyTimeToString(FromSecSinceMidnight(ToSecSinceMidnight(time)))}");
            WriteLine($"Метод AddOneSecond додає до часу, поданого кортежем {normalized_time}, одну секунду:\n{MyTimeToString(AddOneSecond(time))}");
            WriteLine($"Метод AddOneMinute додає до часу, поданого кортежем {normalized_time}, одну хвилину:\n{MyTimeToString(AddOneMinute(time))}");
            WriteLine($"Метод AddOneHour додає до часу, поданого кортежем {normalized_time}, одну годину:\n{MyTimeToString(AddOneHour(time))}");
            WriteLine($"Метод AddSeconds додає до структури {normalized_time} вказану кількість секунд s:");
            //WriteLine(MyTimeToString(AddSeconds(time,30)));
            WriteLine(MyTimeToString(AddSeconds(time)));
            WriteLine($"Метод Difference вертає різницю між двома моментами, заданими кортежами:");
            //WriteLine(Difference(time, (1,1,1)));
            WriteLine(Difference(time) + "c");
            WriteLine($"Метод ToSecSinceMidnight формує рядок згідно розкладу дзвінків:\n{WhatLesson(time)}");
        }

        private static MyTime InputTuple()
        {
            int a = Input("Введіть години: ");
            int b = Input("Введіть хвилини: ");
            int c = Input("Введіть секунди: ");
            return (a, b, c);
        }
        static int Input(string s)
        {
            do
            {
                Write(s);
                if (int.TryParse(ReadLine(), out int a)) return a;
                else Program.Error();
            }
            while (true);
        }

        static string MyTimeToString(MyTime time) => $"{time.hour}:{time.min:D2}:{time.sec:D2}";
        // => (лямбда-оператор) вказує, що метод повертає результат виразу, який знаходиться праворуч від оператора.
        static MyTime Normalize(MyTime time)
        {
            time.min += time.sec / 60;
            time.sec = (time.sec % 60 + 60) % 60;

            time.hour += time.min / 60;
            time.min = (time.min % 60 + 60) % 60;

            time.hour = (time.hour % 24 + 24) % 24;

            return time;
        }
        static int ToSecSinceMidnight(MyTime time)
        {
            time = Normalize(time);
            return time.hour * 3600 + time.min * 60 + time.sec;
        }
        static MyTime FromSecSinceMidnight(int seconds) => Normalize((seconds / 3600, seconds / 60 % 60, seconds % 60));
        static MyTime AddOneSecond(MyTime time)
        {
            time.sec += 1;
            return Normalize(time);
        }
        static MyTime AddOneMinute(MyTime time)
        {
            time.min += 1;
            return Normalize(time);
        }
        static MyTime AddOneHour(MyTime time)
        {
            time.hour += 1;
            return Normalize(time);
        }
        static MyTime AddSeconds(MyTime time, int? s = null)
        {
            if (s == null) s = Input("Введіть скільки секунд додати: ");
            WriteLine($"{MyTimeToString(Normalize(time))} + {s}c");
            s+=ToSecSinceMidnight(time);
            return FromSecSinceMidnight((int)s);
        }
        static int Difference(MyTime time1, MyTime? time2 = null)
        {
            if (time2 == null) time2 = InputTuple();
            WriteLine($"{MyTimeToString(Normalize(time1))} - {MyTimeToString(Normalize((MyTime)time2))}");
            return ToSecSinceMidnight(time1) - ToSecSinceMidnight((MyTime)time2);
        }
        static readonly (int Start, int End)[] schedule =
        {
           (ToSecSinceMidnight((8, 0, 0)), ToSecSinceMidnight((9, 20, 0))),
           (ToSecSinceMidnight((9, 40, 0)), ToSecSinceMidnight((11, 0, 0))),
           (ToSecSinceMidnight((11, 20, 0)), ToSecSinceMidnight((12, 40, 0))),
           (ToSecSinceMidnight((13, 0, 0)), ToSecSinceMidnight((14, 20, 0))),
           (ToSecSinceMidnight((14, 40, 0)), ToSecSinceMidnight((16, 0, 0))),
           (ToSecSinceMidnight((16, 10, 0)), ToSecSinceMidnight((17, 30, 0)))
        };
        static string WhatLesson(MyTime time)
        {
            int seconds = ToSecSinceMidnight(time);

            for (int i = 0; i < schedule.Length; i++)
            {
                if (seconds >= schedule[i].Start && seconds < schedule[i].End)
                {
                    return $"{i + 1}-а пара.";
                }

                if (i < schedule.Length - 1 && seconds >= schedule[i].End && seconds < schedule[i + 1].Start)
                {
                    return $"Перерва між {i + 1}-ю та {i + 2}-ю парами";
                }
            }

            if (seconds < schedule[0].Start)
            {
                return "Пари ще не почалися.";
            }

            return "Пари вже скінчилися";
        }
    }
}
