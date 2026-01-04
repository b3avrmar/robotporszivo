using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Console.WriteLine("Robot kezdo pozicio: sor = " + robotSor + " oszlop = " + robotOszlop);
            Console.WriteLine();
            Console.WriteLine("Nyomjd meg az ENTER-t a takaritas inditasahoz...");
            Console.ReadKey();

            RobotTakaritas(palya, robotSor, robotOszlop);

            Console.WriteLine();
            Console.WriteLine("Takaritas befejezve!");
            Console.WriteLine("Nyomjd meg az ENTER-t a kilepeshez...");
            Console.ReadKey();
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

                if (sorokSzama < 20 || sorokSzama > 30 || oszlopokSzama < 20 || oszlopokSzama > 30)
                {
                    Console.WriteLine("Hiba: az ertekek 20 es 30 kozott legyenek.");
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

                    switch (mezo)
                    {
                        case '-':
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case 'b':
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case 'k':
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 'r':
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

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

        static void RobotTakaritas(char[,] palya, int robotSor, int robotOszlop)
        {
            int sorok = palya.GetLength(0);
            int oszlopok = palya.GetLength(1);
            Random veletlen = new Random();
            int felszedettKoszok = 0;
            bool folytatodik = true;

            do
            {
                // Lehetseges lepesek szamolasa
                int[] lehetoSor = new int[4];
                int[] lehetoOszlop = new int[4];
                int[] lehetoIrany = new int[4];
                int lehetsegesDb = 0;

                if (robotSor > 0 && palya[robotSor - 1, robotOszlop] != 'b')
                {
                    lehetoSor[lehetsegesDb] = -1;
                    lehetoOszlop[lehetsegesDb] = 0;
                    lehetoIrany[lehetsegesDb] = 0;
                    lehetsegesDb++;
                }

                if (robotSor < sorok - 1 && palya[robotSor + 1, robotOszlop] != 'b')
                {
                    lehetoSor[lehetsegesDb] = 1;
                    lehetoOszlop[lehetsegesDb] = 0;
                    lehetoIrany[lehetsegesDb] = 1;
                    lehetsegesDb++;
                }

                if (robotOszlop > 0 && palya[robotSor, robotOszlop - 1] != 'b')
                {
                    lehetoSor[lehetsegesDb] = 0;
                    lehetoOszlop[lehetsegesDb] = -1;
                    lehetoIrany[lehetsegesDb] = 2;
                    lehetsegesDb++;
                }

                if (robotOszlop < oszlopok - 1 && palya[robotSor, robotOszlop + 1] != 'b')
                {
                    lehetoSor[lehetsegesDb] = 0;
                    lehetoOszlop[lehetsegesDb] = 1;
                    lehetoIrany[lehetsegesDb] = 3;
                    lehetsegesDb++;
                }

                // Ha nem tud lépni
                if (lehetsegesDb == 0)
                {
                    Console.Clear();
                    PalyaMegjelenitese(palya);
                    Console.WriteLine();
                    Console.WriteLine("A robot elakadt! Nincs tobb lehetseges lepes.");
                    folytatodik = false;
                }
                // Ha tud lépni
                else
                {
                    // Mozgás és takarítás
                    int valasztott = veletlen.Next(lehetsegesDb);

                    palya[robotSor, robotOszlop] = '-';

                    robotSor += lehetoSor[valasztott];
                    robotOszlop += lehetoOszlop[valasztott];
                    int irany = lehetoIrany[valasztott];

                    string iranySzoveg = "";
                    switch (irany)
                    {
                        case 0:
                            iranySzoveg = "FEL";
                            break;
                        case 1:
                            iranySzoveg = "LE";
                            break;
                        case 2:
                            iranySzoveg = "BALRA";
                            break;
                        case 3:
                            iranySzoveg = "JOBBRA";
                            break;
                    }

                    // Felszedett kosz kezelese
                    bool koszVolt = (palya[robotSor, robotOszlop] == 'k');

                    if (koszVolt)
                    {
                        felszedettKoszok++;
                    }

                    palya[robotSor, robotOszlop] = 'r';

                    // Konzol torlese es aktualis allapot megjelenitese
                    Console.Clear();
                    PalyaMegjelenitese(palya);
                    Console.WriteLine();

                    Console.WriteLine("Robot lepett: " + iranySzoveg);
                    Console.WriteLine("Osszes felszedett kosz: " + felszedettKoszok);
                }
                Thread.Sleep(5);

            } while (true);
        }

        
    }
}