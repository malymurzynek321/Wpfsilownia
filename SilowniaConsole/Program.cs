using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;

class Program
{
    private static Silownia LoadState()
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stan_silowni.json");
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Silownia>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas wczytywania stanu: {ex.Message}");
            return new Silownia();
        }
    }

    private static void SaveState(Silownia silownia)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = System.Text.Json.JsonSerializer.Serialize(silownia, options);
        File.WriteAllText("stan_silowni.json", json);
    }
    static void Main()
    {
        var scanner = new Scanner();
        var silownia = LoadState();
        AppDomain.CurrentDomain.ProcessExit += (sender, e) => SaveState(silownia);
        while (true)
        {
            Console.WriteLine("Wybierz opcję:");
            Console.WriteLine("1. Dodaj trening");
            Console.WriteLine("2. Wyświetl treningi");
            Console.WriteLine("3. Oblicz BMI");
            Console.WriteLine("4. Wyświetl tablicę BMI");
            Console.WriteLine("5. Dodaj dietę");
            Console.WriteLine("6. Wyświetl dietę");
            Console.WriteLine("7. Usuń trening lub dietę");
            Console.WriteLine("8. Edytuj trening lub dietę");
            Console.WriteLine("9. Wyjście");

            int opcja;
            if (!int.TryParse(Console.ReadLine(), out opcja))
            {
                Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                continue;
            }

            switch (opcja)
            {
                case 1:
                    Console.WriteLine("Podaj trening:");
                    string nazwaTreningu = Console.ReadLine();

                    Console.WriteLine("Podaj ćwiczenie:");
                    string cwiczenie = Console.ReadLine();

                    int iloscSerii = 0;
                    bool poprawnaIloscSerii = false;
                    while (!poprawnaIloscSerii)
                    {
                        Console.WriteLine("Podaj ilość serii:");
                        if (int.TryParse(Console.ReadLine(), out iloscSerii))
                        {
                            poprawnaIloscSerii = true;
                        }
                        else
                        {
                            Console.WriteLine("Błędny format. Wprowadź liczbę.");
                        }
                    }

                    List<int> ilosciPowtorzen = new List<int>();
                    for (int i = 1; i <= iloscSerii; i++)
                    {
                        bool poprawnaIloscPowtorzen = false;
                        while (!poprawnaIloscPowtorzen)
                        {
                            Console.WriteLine($"Podaj ilość powtórzeń dla serii {i}:");
                            if (int.TryParse(Console.ReadLine(), out int iloscPowtorzen))
                            {
                                ilosciPowtorzen.Add(iloscPowtorzen);
                                poprawnaIloscPowtorzen = true;
                            }
                            else
                            {
                                Console.WriteLine("Błędny format. Wprowadź liczbę.");
                            }
                        }
                    }

                    silownia.DodajTrening(nazwaTreningu, cwiczenie, iloscSerii, ilosciPowtorzen);
                    break;
                case 2:
                    Console.WriteLine("Treningi:");
                    silownia.WyswietlTreningi();
                    break;
                case 3:
                    Console.WriteLine("Podaj wagę (w kilogramach):");
                    double waga = scanner.NextDouble();

                    Console.WriteLine("Podaj wzrost (w metrach):");
                    double wzrost = scanner.NextDouble();

                    double bmi = ObliczBMI(waga, wzrost);
                    Console.WriteLine($"Twoje BMI to: {bmi}");
                    break;
                case 4:
                    WyswietlInterpretacjeBMI();
                    break;
                case 5:
                    Console.WriteLine("Podaj opis diety:");
                    string opisDiety = scanner.NextLine();
                    silownia.DodajDiete(opisDiety);
                    break;
                case 6:
                    Console.WriteLine("Diety:");
                    silownia.WyswietlDiete();
                    break;
                case 7:
                    Console.WriteLine("Wybierz opcję:");
                    Console.WriteLine("1. Usuń trening");
                    Console.WriteLine("2. Usuń dietę");

                    string opcjaUsunString = scanner.NextLine();
                    if (int.TryParse(opcjaUsunString, out int opcjaUsun))
                    {
                        switch (opcjaUsun)
                        {
                            case 1:
                                Console.WriteLine("Podaj numer treningu do usunięcia:");
                                int numerTreningu = scanner.NextInt();
                                silownia.UsunTrening(numerTreningu);
                                break;
                            case 2:
                                Console.WriteLine("Podaj numer diety do usunięcia:");
                                int numerDiety = scanner.NextInt();
                                silownia.UsunDiete(numerDiety);
                                break;
                            default:
                                Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    }
                    break;
                case 8:
                    Console.WriteLine("Wybierz opcję:");
                    Console.WriteLine("1. Edytuj trening");
                    Console.WriteLine("2. Edytuj dietę");

                    string opcjaEdytujString = scanner.NextLine();
                    if (int.TryParse(opcjaEdytujString, out int opcjaEdytuj))
                    {
                        switch (opcjaEdytuj)
                        {
                            case 1:
                                Console.WriteLine("Podaj numer treningu do edycji:");
                                int numerEdytowanegoTreningu = scanner.NextInt();
                                scanner.NextLine();
                                silownia.EdytujTrening(numerEdytowanegoTreningu, scanner);
                                break;
                            case 2:
                                Console.WriteLine("Podaj numer diety do edycji:");
                                int numerEdytowanejDiety = scanner.NextInt();
                                scanner.NextLine();
                                silownia.EdytujDiete(numerEdytowanejDiety, scanner);
                                break;
                            default:
                                Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    }
                    break;
                case 9:
                    Console.WriteLine("Dziękujemy za skorzystanie z aplikacji. Do widzenia!");
                    SaveState(silownia);
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    break;
            }
        }
    }

    private static double ObliczBMI(double waga, double wzrost)
    {
        return waga / (wzrost * wzrost) * 10000.0;
    }

    private static void WyswietlInterpretacjeBMI()
    {
        Console.WriteLine("Interpretacja wyników BMI:");
        Console.WriteLine("Poniżej 16: Wygłodzenie");
        Console.WriteLine("16 - 16.9: Wychudzenie");
        Console.WriteLine("17 - 18.4: Niedowaga");
        Console.WriteLine("18.5 - 24.9: Waga prawidłowa");
        Console.WriteLine("25 - 29.9: Nadwaga");
        Console.WriteLine("30 - 34.9: I stopień otyłości");
        Console.WriteLine("35 - 39.9: II stopień otyłości (otyłość kliniczna)");
        Console.WriteLine("Powyżej 40: III stopień otyłości (otyłość skrajna)");
    }
}

class Scanner
{
    public int NextInt()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Błędny format. Wprowadź liczbę.");
            }
        }
    }

    public double NextDouble()
    {
        return double.Parse(Console.ReadLine());
    }

    public string NextLine()
    {
        return Console.ReadLine();
    }
}

class Silownia
{
    private List<Trening> treningi = new List<Trening>();
    private List<string> diety = new List<string>();

    public void DodajTrening(string nazwaTreningu, string cwiczenie, int iloscSerii, List<int> ilosciPowtorzen)
    {
        var trening = new Trening(nazwaTreningu, cwiczenie, iloscSerii, ilosciPowtorzen);
        treningi.Add(trening);
        Console.WriteLine("Trening dodany!");
    }

    public void WyswietlTreningi()
    {
        if (treningi.Count == 0)
        {
            Console.WriteLine("Brak treningów.");
        }
        else
        {
            for (int i = 0; i < treningi.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {treningi[i]}");
            }
        }
    }

    public void DodajDiete(string opisDiety)
    {
        diety.Add(opisDiety);
        Console.WriteLine("Dieta dodana!");
    }

    public void WyswietlDiete()
    {
        if (diety.Count == 0)
        {
            Console.WriteLine("Brak diet.");
        }
        else
        {
            for (int i = 0; i < diety.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {diety[i]}");
            }
        }
    }

    public void UsunTrening(int numerTreningu)
    {
        int indeksTreningu = numerTreningu - 1;
        if (indeksTreningu >= 0 && indeksTreningu < treningi.Count)
        {
            treningi.RemoveAt(indeksTreningu);
            Console.WriteLine("Trening usunięty!");
        }
        else
        {
            Console.WriteLine("Nieprawidłowy numer treningu.");
        }
    }

    public void UsunDiete(int numerDiety)
    {
        int indeksDiety = numerDiety - 1;
        if (indeksDiety >= 0 && indeksDiety < diety.Count)
        {
            diety.RemoveAt(indeksDiety);
            Console.WriteLine("Dieta usunięta!");
        }
        else
        {
            Console.WriteLine("Nieprawidłowy numer diety.");
        }
    }

    public void EdytujTrening(int numerTreningu, Scanner scanner)
    {
        int indeksTreningu = numerTreningu - 1;
        if (indeksTreningu >= 0 && indeksTreningu < treningi.Count)
        {
            var edytowanyTrening = treningi[indeksTreningu];

            Console.WriteLine("Podaj nową nazwę treningu:");
            string nowaNazwaTreningu = scanner.NextLine();
            edytowanyTrening.UstawNazweTreningu(nowaNazwaTreningu);

            Console.WriteLine("Podaj nowe ćwiczenie:");
            string noweCwiczenie = scanner.NextLine();
            edytowanyTrening.UstawCwiczenie(noweCwiczenie);

            int nowaIloscSerii = 0;
            bool poprawnaIloscSerii = false;
            while (!poprawnaIloscSerii)
            {
                Console.WriteLine("Podaj nową ilość serii:");
                if (int.TryParse(Console.ReadLine(), out nowaIloscSerii) && nowaIloscSerii > 0)
                {
                    poprawnaIloscSerii = true;
                }
                else
                {
                    Console.WriteLine("Błędny format lub wartość. Wprowadź liczbę większą niż 0.");
                }
            }
            edytowanyTrening.UstawIloscSerii(nowaIloscSerii);

            List<int> noweIlosciPowtorzen = new List<int>();
            for (int i = 1; i <= nowaIloscSerii; i++)
            {
                bool poprawnaIloscPowtorzen = false;
                while (!poprawnaIloscPowtorzen)
                {
                    Console.WriteLine($"Podaj ilość powtórzeń dla serii {i}:");
                    if (int.TryParse(Console.ReadLine(), out int iloscPowtorzen) && iloscPowtorzen > 0)
                    {
                        noweIlosciPowtorzen.Add(iloscPowtorzen);
                        poprawnaIloscPowtorzen = true;
                    }
                    else
                    {
                        Console.WriteLine("Błędny format lub wartość. Wprowadź liczbę większą niż 0.");
                    }
                }
            }
            edytowanyTrening.UstawIlosciPowtorzen(noweIlosciPowtorzen);

            Console.WriteLine("Trening został zaktualizowany!");
        }
        else
        {
            Console.WriteLine("Nieprawidłowy numer treningu.");
        }
    }

    public void EdytujDiete(int numerDiety, Scanner scanner)
    {
        int indeksDiety = numerDiety - 1;
        if (indeksDiety >= 0 && indeksDiety < diety.Count)
        {
            Console.WriteLine("Podaj nowy opis diety (wpisz 0, jeśli nie chcesz zmieniać):");
            string nowyOpisDiety = scanner.NextLine();
            if (nowyOpisDiety != "0")
            {
                diety[indeksDiety] = nowyOpisDiety;
                Console.WriteLine("Dieta została zaktualizowana!");
            }
            else
            {
                Console.WriteLine("Dieta nie została zmieniona.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowy numer diety.");
        }
    }
}

class Trening
{
    private string nazwaTreningu;
    private string cwiczenie;
    private int iloscSerii;
    private List<int> ilosciPowtorzen;

    public Trening(string nazwaTreningu, string cwiczenie, int iloscSerii, List<int> ilosciPowtorzen)
    {
        this.nazwaTreningu = nazwaTreningu;
        this.cwiczenie = cwiczenie;
        this.iloscSerii = iloscSerii;
        this.ilosciPowtorzen = ilosciPowtorzen;
    }

    public void UstawNazweTreningu(string nowaNazwaTreningu)
    {
        nazwaTreningu = nowaNazwaTreningu;
    }

    public void UstawCwiczenie(string noweCwiczenie)
    {
        cwiczenie = noweCwiczenie;
    }

    public void UstawIloscSerii(int nowaIloscSerii)
    {
        iloscSerii = nowaIloscSerii;
    }

    public void UstawIlosciPowtorzen(List<int> noweIlosciPowtorzen)
    {
        ilosciPowtorzen = noweIlosciPowtorzen;
    }

    public override string ToString()
    {
        var result = new System.Text.StringBuilder($"Trening: {nazwaTreningu}, Ćwiczenie: {cwiczenie}, Ilość serii: {iloscSerii}\n");
        for (int i = 0; i < iloscSerii; i++)
        {
            result.Append($"    Seria {i + 1}: {ilosciPowtorzen[i]} powtórzeń\n");
        }
        return result.ToString();
    }
}