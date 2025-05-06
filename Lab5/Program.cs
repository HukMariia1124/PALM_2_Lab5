using System.Text;
namespace Lab5
{
    internal class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            byte choiceBlock;
            do
            {
                Console.WriteLine(
                    """
                    ------------------------------------------------------------------------------------------------------------------------
                                                                         ВИБІР ЗАВДАННЯ
                    ------------------------------------------------------------------------------------------------------------------------
                    БЛОК #1:
                    1) MyTime
                    БЛОК #2:
                    2) Скопіювати дані в data_new.txt, зберігши порядок: змінити стать на ‘Ч’/‘Ж’ та вирівняти дані по стовпчиках пробілами.
                    ДОДАТКОВІ ЗАВДАННЯ:
                    3) Вивести ПІБ студенток, чий середній бал > середнього бала чоловіків.
                    4) Розділити всі дані про всіх студентів на ≤12 файлів, відповідно до знаків зодіаку.
                    ------------------------------------------------------------------------------------------------------------------------
                    0) Вийти з програми
                    """);

                choiceBlock = Choice(4);
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

                switch (choiceBlock)
                {
                    case 1:
                        Task1.Processing();
                        break;
                    case 2:
                        Task2.Processing();
                        break;
                    case 3:
                        Additionals.Add1_Processing();
                        break;
                    case 4:
                        Additionals.Add2_Processing();
                        break;
                    case 0:
                        break;
                    default:
                        Error();
                        break;
                }
            } while (choiceBlock != 0);
        }
        public static void Error() => Console.WriteLine("Помилка! Повторіть спробу!");
        public static byte Choice(byte countOfBlocks)
        {
            byte choice;
            do
            {
                try
                {
                    choice = byte.Parse(Console.ReadLine()!);
                    if (choice <= countOfBlocks) return choice;
                    Error();
                }
                catch { Error(); }
            } while (true);
        }
    }
}
