using System.Reflection.PortableExecutable;
using MyTime = (int hour, int min, int sec);

namespace Lab5
{
    public static class Task1
    {
        public static void Processing()
        {
            MyTime time = InputTuple();

            Console.WriteLine(
                """
                    ------------------------------------------------------------------------------------------------------------------------
                                                                         ВИБІР МЕТОДУ
                    ------------------------------------------------------------------------------------------------------------------------
                    1)  MyTimeToString формує рядкове подання часу з кортежа у форматі H:mm:s.
                    2)  Normalize приводить години до проміжку 0...23, хвилини й секунди до проміжку 0...59.
                    3)  ToSecSinceMidnight перетворює час у кількість секунд, що пройшли від початку доби.
                    4)  FromSecSinceMidnight перетворює кількість секунд, що пройшли від початку доби, у кортеж MyTime.
                    5)  AddOneSecond додає до часу, поданого кортежем, одну секунду.
                    6)  AddOneMinute додає до часу, поданого кортежем, одну хвилину.
                    7)  AddOneHour додає до часу, поданого кортежем, одну годину.
                    8)  AddSeconds додає до структури вказану кількість секунд s.
                    9)  Difference вертає різницю між двома моментами, заданими кортежами.
                    10) ToSecSinceMidnight формує рядок згідно розкладу дзвінків.
                    ------------------------------------------------------------------------------------------------------------------------
                    0) Вийти з програми
                    ------------------------------------------------------------------------------------------------------------------------
                    """);

            byte choiceBlock;
            do
            {
                Console.Write("Оберіть метод: "); 
                choiceBlock = Program.Choice(10);

                if (choiceBlock != 0)
                {
                    Console.Write("Результат виконання: ");
                    Console.WriteLine(
                        choiceBlock switch
                        {
                            1  => MyTimeToString(time),
                            2  => MyTimeToString(time = Normalize(time)),
                            3  => ToSecSinceMidnight(time),
                            4  => MyTimeToString(time = FromSecSinceMidnight(ToSecSinceMidnight(time))),
                            5  => MyTimeToString(time = AddOneSecond(time)),
                            6  => MyTimeToString(time = AddOneMinute(time)),
                            7  => MyTimeToString(time = AddOneHour(time)),
                            8  => MyTimeToString(time = AddSeconds(time)),
                            9  => Difference(time) + "c", 
                            10 => WhatLesson(time),
                        });
                }
            } while (choiceBlock != 0);
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
                Console.Write(s);
                if (int.TryParse(Console.ReadLine(), out int a)) return a;
                else Program.Error();
            }
            while (true);
        }

        static string MyTimeToString(MyTime time) => $"{time.hour}:{time.min:D2}:{time.sec:D2}";
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
            if (s == null) s = Input("\nВведіть скільки секунд додати: ");
            Console.WriteLine($"{MyTimeToString(Normalize(time))} + {s}c");
            s+=ToSecSinceMidnight(time);
            return FromSecSinceMidnight((int)s);
        }
        static int Difference(MyTime time1, MyTime? time2 = null)
        {
            Console.WriteLine();
            if (time2 == null) time2 = InputTuple();
            Console.WriteLine($"{MyTimeToString(Normalize(time1))} - {MyTimeToString(Normalize((MyTime)time2))}");
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
