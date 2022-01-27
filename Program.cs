using System;

namespace Cinema
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            // Ввод количества рядов и мест в ряду
            DataStorage.GetRowsNColumns();

            // Создание переменной, в которой хранится схема мест с ценами
            int[,] prices = DataStorage.GetPrices(DataStorage.rows, DataStorage.columns);

            // Визуализация схемы кинозала с ценами на места
            DataStorage.PricesVisualising(DataStorage.rows, DataStorage.columns, prices);

            // Создание схемы кинозала с досутпными местами
            BuyingTicket.GenerateFreeCinema();

            // Далее Пользователь выбирает от чьего лица войти в интерфейс

            while (true)
            {
                try
                {
                    // Если выбрал путь "клиент"
                    if (Interface.GetUser() == "User")
                    {
                        Interface.ClientWelcomeScreen();
                        // Работа пользователя по бронированию билетов с помощью схемы зала
                        BuyingTicket.Booking(prices);
                    }
                    // Если выбрал путь "администратор"
                    else
                    {   //проверка пароля
                        if(Interface.PasswordCheck())
                        {   //Если выбрал опцию "посмотреть информацию"
                            if (Interface.AdminWelcomeScreen() == 1)
                            {
                                Interface.Info();
                            }
                            // Если выбрал опцию "изменить цены на места"
                            else
                            {
                                prices = DataStorage.GetPrices(DataStorage.rows, DataStorage.columns);
                            }
                        }
                    }


                

                }
                catch (Exception)
                {

                    Console.WriteLine("Неверный формат ввода! попробуйте еще раз"); ;
                }
            }
        }

    }

   





    public class DataStorage
    {
        public static int rows;
        public static int columns;
        public static void GetRowsNColumns()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите  количество рядов:");
                    rows = int.Parse(Console.ReadLine());

                    Console.WriteLine("Введите  количество мест в ряду:");
                    columns = int.Parse(Console.ReadLine());

                    break;
                }
                catch (Exception)
                {

                    Console.WriteLine("Неверный формат ввода! Необходимо ввести целочисленное значение. Попробуйте еще раз");
                }
            }
            

        }
        public static int[,] GetPrices(int rows, int columns)
        {
            while (true)
            {
                try
                {
                    // Создание массива мест            
                    int[,] prices = new int[rows, columns];
                    // Введение данных о цене мест администратором
                    Console.WriteLine("Стоимости билетов:");
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            Console.Write("ряд " + (i + 1) + ", место " + (j + 1) + ": ");
                            prices[i, j] = int.Parse(Console.ReadLine());
                        }
                    }
                    Console.WriteLine();

                    return prices;
                }
                catch (Exception)
                {

                    Console.WriteLine("Неверный формат ввода! Необходимо ввести целочисленное значение. Попробуйте еще раз");
                    throw;
                }
            }
        }

        // вывод схемы зала с ценами
        public static void PricesVisualising(int rows, int columns, int[,] places)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Схема зала с ценами на билеты:");
                    Console.WriteLine();
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            Console.Write(places[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    break;
                }
                catch (Exception)
                {

                    Console.WriteLine("Неверный формат ввода! Необходимо ввести целочисленное значение. Попробуйте еще раз");
                }
            }
        }
    }

    class BuyingTicket
    {
        public static int Profit = 0;
        public static int rows = DataStorage.rows;
        public static int columns = DataStorage.columns;
        public static string[,] reserved = new string[rows, columns];
        public static int Balance()
        {
            Console.Write("введите баланс пользователя: ");
            int balance = int.Parse(Console.ReadLine());
            return balance;
        }

        // Генерация пустого зала
        public static void GenerateFreeCinema()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    reserved[i, j] = "O";
                }
            }
        }
        // Пользовательский интерфейс для бронирования мест
        public static void Booking(int[,] prices)
        {
            // Запрос баланса пользователя с помощью метода balance класса BuyingTicket
            int Balance = BuyingTicket.Balance();

            Console.WriteLine();
            Console.WriteLine("Схема зала. Свободные места - 0, забронированные места - Х");
            Console.WriteLine();

            ConsoleKeyInfo cki = new ConsoleKeyInfo();

            // Цикличный вывод схемы кинозала с местами для бронирования
            do
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Console.Write(reserved[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                //  Процесс бронирования
                Console.WriteLine("Если желаете отменить операцию, нажмите Escape");
                Console.WriteLine("Если желаете перейти к бронированию, нажмите любую другую клавишу");
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape)
                {
                    break;
                }
                Console.WriteLine("Выберите ряд и место, которые вы бы хотели забронировать");
                Console.Write("ряд: ");
                int BookingRow = (int.Parse(Console.ReadLine()) - 1);
                Console.Write("место: ");
                int BookingColumn = (int.Parse(Console.ReadLine()) - 1);


                try
                {
                    if (reserved[BookingRow, BookingColumn] == "O")
                    {
                        if (prices[BookingRow, BookingColumn] <= Balance)
                        {
                            reserved[BookingRow, BookingColumn] = "X";
                            Balance -= prices[BookingRow, BookingColumn];
                            Profit += prices[BookingRow, BookingColumn];
                            Console.WriteLine("Место успешно забронировано! Желаете продолжить бронирование?");
                            Console.WriteLine("Если нет - нажмите N, если да - любую другую клавишу");
                            cki = Console.ReadKey(true);
                            if (cki.Key == ConsoleKey.N)
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("На счете недостаточно средств! Для выбора другого места нажмите пробел или отмените операцию, нажав Escape");
                            cki = Console.ReadKey(true);
                            if (cki.Key == ConsoleKey.Escape)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Это место уже забронировано! Пожалуйста, выберите место из предложенных, обозначенное символом  'O' ");
                    }
                }
                catch (IndexOutOfRangeException)
                {

                    Console.WriteLine("Места, которое вы пытаетесь забронировать, не существует. Попробуйте еще раз.");
                    Console.WriteLine(""); ;
                }

            } while (true);

        }

        public static int GetFreePlaces()
        {
            int freePlaces = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (reserved[i, j] == "O")
                    {
                        freePlaces++;
                    }
                }
                Console.WriteLine();
            }
            return freePlaces;
        }

        public static int GetReservedPlaces()
        {
            int reservedPlaces = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (reserved[i, j] == "X")
                    {
                        reservedPlaces++;
                    }
                }
                Console.WriteLine();
            }
            return reservedPlaces;
        }

    }

    class Interface
    {
        public static string GetUser()
        {
            
            Console.WriteLine("Желаете продолжить от лица Администратора или Клиента?");
            Console.WriteLine("");
            Console.WriteLine("Чтобы продолжить как Администратор, нажмите А (RUS раскладка)");
            Console.WriteLine("Чтобы продолжить как Клиент, нажмите К (RUS раскладка)");
            Console.WriteLine("");

            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.F)
            {
                return "Admin";
            }
            else
            {
                return "User";
            }

        }
    
        public static bool PasswordCheck()
        {
            do
            {


            int ValidPassword = 123;
            Console.Write("Введите пароль администратора (123): ");
            int EnteredPassword = int.Parse(Console.ReadLine());
            if (EnteredPassword == ValidPassword)
            {
                    return true;
            }
            else
            {
                    Console.WriteLine("Неверный пароль! Попробуйте еще раз");
            }
            } while (true);
        }

        public static int AdminWelcomeScreen()
        {
            Console.WriteLine("");
            Console.WriteLine("*********************************");
            Console.WriteLine("Вы перешли в режим администратора");
            Console.WriteLine("*********************************");
            Console.WriteLine("");
            Console.WriteLine("Чтобы посмотреть доступную информацию нажмите 1");
            Console.WriteLine("Чтобы изменить цены на билеты нажмите 2");
            int AdminChoice = int.Parse(Console.ReadLine());
            
            return AdminChoice;
        }

        public static void ClientWelcomeScreen()
        {
            Console.WriteLine("");
            Console.WriteLine("*******************************************");
            Console.WriteLine("Добро пожаловать на сайт нашего кинотеатра!");
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
        }
        public static void Info()
        {
            int Revenue = BuyingTicket.Profit;
            int FreePlaces = BuyingTicket.GetFreePlaces();
            int ReservedPlaces = BuyingTicket.GetReservedPlaces();
            Console.WriteLine("Прибыль равна " + Revenue);
            Console.WriteLine("Свободных мест: " + FreePlaces);
            Console.WriteLine("Выкупленно мест: " + ReservedPlaces);
            
        }
    }
}


