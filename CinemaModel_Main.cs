using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
                // Указание размера кинозала
            int rows;
            int columns;
            
            Console.WriteLine("Введите  количесво рядов в зале:");
            rows = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите  количесво мест в ряду:");
            columns = int.Parse(Console.ReadLine());

            int[,] places = ArrayCreating(rows, columns);
            arrayVisualising(rows, columns, places);                       
        }
        static int[,] ArrayCreating(int rows, int columns)
        {
            // Создание массива мест         
            int[,] places = new int[rows, columns];
            // Введение данных о цене мест администратором
            Console.WriteLine("Стоимости билетов:");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write("ряд " + (i + 1) + ", место " + (j + 1) + ": ");
                    places[i, j] = int.Parse(Console.ReadLine());
                }
            }
            Console.WriteLine();
            return places;
        }

        static void arrayVisualising(int rows, int columns, int[,] places)
        {
            // вывод схемы зала с ценами
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
        }
        
    }
}
