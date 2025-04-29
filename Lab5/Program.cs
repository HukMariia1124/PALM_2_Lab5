using System.Text;
using static System.Console;
namespace Lab5
{
    internal class Program
    {
        public static void Main()
        {
            OutputEncoding = Encoding.Unicode;
            InputEncoding = Encoding.Unicode;
            byte choiceBlock;
            do
            {
                WriteLine(
                    """
                    ------------------------------------------------------------------------------------------------------------------------
                                                                         ВИБІР ЗАВДАННЯ
                    ------------------------------------------------------------------------------------------------------------------------
                    БЛОК #1:
                    1) MyTime
                    БЛОК #2:
                    2) Скопіювати дані в data_new.txt, зберігши порядок: змінити стать на ‘Ч’/‘Ж’ та вирівняти дані по стовпчиках пробілами.
                    ------------------------------------------------------------------------------------------------------------------------
                    0) Вийти з програми
                    """);

                choiceBlock = Choice(2);
                WriteLine("------------------------------------------------------------------------------------------------------------------------");

                switch (choiceBlock)
                {
                    case 1:
                        Task1.Processing();
                        break;
                    case 2:
                        Task2.Processing();
                        break;
                    case 0:
                        break;
                    default:
                        Error();
                        break;
                }
            } while (choiceBlock != 0);
        }
        public static void Error() => WriteLine("Помилка! Повторіть спробу!");
        public static byte Choice(byte countOfBlocks)
        {
            byte choice;
            do
            {
                try
                {
                    choice = byte.Parse(ReadLine()!);
                    if (choice <= countOfBlocks) return choice;
                    Error();
                }
                catch { Error(); }
            } while (true);
        }
    }
}
