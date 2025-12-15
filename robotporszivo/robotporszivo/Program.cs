using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robotporszivo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int robotSor;
            int robotOszlop;

            char[,] palya = PalyaLetrehozasa(out robotSor, out robotOszlop);

            PalyaMegjelenitese(palya);

            Console.WriteLine();
            Console.WriteLine("Robot kezdo pozicio: sor=" + robotSor + " oszlop=" + robotOszlop);
            Console.ReadLine();
        }

        static char[,] PalyaLetrehozasa(out int robotSor, out int robotOszlop)
        {
            int sorokSzama;
            int oszlopokSzama;
            bool helyes;
            string sorSzoveg;
            string oszlopSzoveg;

            // Meretek bekerese
            do
            {
                helyes = true;

                Console.Write("Add meg a sorok szamat (20-30): ");
                sorSzoveg = Console.ReadLine();

                Console.Write("Add meg az oszlopok szamat (20-30): ");
                oszlopSzoveg = Console.ReadLine();

                try
                {
                    sorokSzama = Convert.ToInt32(sorSzoveg);
                    oszlopokSzama = Convert.ToInt32(oszlopSzoveg);
                }
                catch
                {
                    Console.WriteLine("Hiba: csak egesz szamot adhatsz meg.");
                    helyes = false;
                    sorokSzama = 0;
                    oszlopokSzama = 0;
                    continue;
                }

                if (sorokSzama < 20 || sorokSzama > 30 ||
                    oszlopokSzama < 20 || oszlopokSzama > 30)
                {
                    Console.WriteLine("Hiba: az ertekek 20 es 30 kozott legyenek.");
                    helyes = false;
                }

                if (helyes && sorokSzama == oszlopokSzama)
                {
                    Console.WriteLine("Hiba: a sorok es oszlopok szama nem lehet egyenlo.");
                    helyes = false;
                }

            } while (!helyes);

            // Palya generalas
            Random veletlen = new Random();
            char[,] palya = new char[sorokSzama, oszlopokSzama];

            int szabadDb = 0;
            int koszDb = 0;

            for (int i = 0; i < sorokSzama; i++)
            {
                for (int j = 0; j < oszlopokSzama; j++)
                {
                    int dobas = veletlen.Next(100);
                    char mezo;

                    if (dobas < 50)
                    {
                        mezo = '-';
                        szabadDb++;
                    }
                    else if (dobas < 70)
                    {
                        mezo = 'b';
                    }
                    else
                    {
                        mezo = 'k';
                        koszDb++;
                    }

                    palya[i, j] = mezo;
                }
            }

            // Legalabb 1-ek
            if (szabadDb == 0)
            {
                int s = veletlen.Next(sorokSzama);
                int o = veletlen.Next(oszlopokSzama);
                palya[s, o] = '-';
            }

            if (koszDb == 0)
            {
                int s = veletlen.Next(sorokSzama);
                int o = veletlen.Next(oszlopokSzama);
                palya[s, o] = 'k';
            }

            // Robot elhelyezese
            do
            {
                robotSor = veletlen.Next(sorokSzama);
                robotOszlop = veletlen.Next(oszlopokSzama);

            } while (palya[robotSor, robotOszlop] != '-');

            palya[robotSor, robotOszlop] = 'r';

            return palya;
        }
        static void PalyaMegjelenitese(char[,] palya)
        {
            int sorok = palya.GetLength(0);
            int oszlopok = palya.GetLength(1);

            for (int i = 0; i < sorok; i++)
            {
                for (int j = 0; j < oszlopok; j++)
                {
                    char mezo = palya[i, j];

                    if (mezo == '-') Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (mezo == 'b') Console.ForegroundColor = ConsoleColor.Red;
                    else if (mezo == 'k') Console.ForegroundColor = ConsoleColor.Green;
                    else if (mezo == 'r') Console.ForegroundColor = ConsoleColor.Cyan;
                    else Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(mezo);
                    Console.ResetColor();

                    if (j < oszlopok - 1)
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
