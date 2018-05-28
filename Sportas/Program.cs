﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sportas
{
    class Sportininkas
    {
        public string Sportas { get; private set; }
        public string Komanda { get; private set; }
        public string Pavardė { get; private set; }
        public string Vardas { get; private set; }
        public int Rungtynės { get; private set; }

        public Sportininkas(string sport, string kom, string pvrd, string vrd, int rung)
        {
            Sportas = sport;
            Komanda = kom;
            Pavardė = pvrd;
            Vardas = vrd;
            Rungtynės = rung;
        }

        public override string ToString()
        {
            return string.Format(" {0, -15} {1, -15} {2}         {3, -15} {4, 4:d} ", Pavardė, Vardas, Sportas, Komanda, Rungtynės);
        }

        public virtual int SkaičiuotiVidurkį()
        {
            return 0;
        }

        public virtual int SkaičiuotiPapildomąVidurkį()
        {
            return 0;
        }

        public virtual int Rikiuoti()
        {
            return 0;
        }
    }

    class Krepšininkas : Sportininkas
    {
        public int Taškai { get; set; }
        public int AtkovotiKamuoliai { get; set; }
        public int RezultatyvūsPerdavimai { get; set; }

        public Krepšininkas (string sport, string kom, string pvrd, string vrd, int rung, int tsk, int atk, int rezp) : base(sport, kom, pvrd, vrd, rung)
        {
            Taškai = tsk;
            AtkovotiKamuoliai = atk;
            RezultatyvūsPerdavimai = rezp;
        }

        public override string ToString()
        {
            return string.Format(" {0}          {1, 4:d}                       {2, 4:d}                           {3, 4:d} ", base.ToString(), Taškai, AtkovotiKamuoliai, RezultatyvūsPerdavimai);
        }

        public override int SkaičiuotiVidurkį()
        {
            return this.Taškai;
        }

        public override int SkaičiuotiPapildomąVidurkį()
        {
            return this.RezultatyvūsPerdavimai;
        }

        public override int Rikiuoti()
        {
            return this.RezultatyvūsPerdavimai;
        }
    }

    class Futbolininkas : Sportininkas
    {
        public int Ivarciai { get; set; }
        public int GeltonųKortelių { get; set; }

        public Futbolininkas(string sport, string kom, string pvrd, string vrd, int rung, int iv, int kort) : base(sport, kom, pvrd, vrd, rung)
        {
            Ivarciai = iv;
            GeltonųKortelių = kort;
        }

        public override string ToString()
        {
            return string.Format(" {0}          {1, 4:d}                       {2, 4:d} ", base.ToString(), Ivarciai, GeltonųKortelių);
        }

        public override int SkaičiuotiVidurkį()
        {
            return this.Ivarciai;
        }

        public override int SkaičiuotiPapildomąVidurkį()
        {
            return this.GeltonųKortelių;
        }

        public override int Rikiuoti()
        {
            return this.GeltonųKortelių;
        }
    }
    
    class Komanda
    {
        public string Sportas { get; set; }
        public string Pavadinimas { get; set; }
        public string Miestas { get; set; }
        public string Treneris { get; set; }
        public int Rungtynės { get; set; }

        public Komanda(string sport, string pav, string mst, string tr, int rungt)
        {
            Sportas = sport;
            Pavadinimas = pav;
            Miestas = mst;
            Treneris = tr;
            Rungtynės = rungt;
        }

        public override string ToString()
        {
            return string.Format("  {0, -15} {1}     {2, -15} {3, -30} {4, 4:d}", Pavadinimas, Sportas, Miestas, Treneris, Rungtynės);
        }
    }

    class Program
    {
        const string komandosDuom = "..\\..\\Komandos2.txt";
        const string sportininkaiDuom = "..\\..\\Sportininkai2.txt";
        const string rez = "..\\..\\Rezultatai.txt";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            List<Sportininkas> sportininkai = new List<Sportininkas>();
            List<Komanda> komandos = new List<Komanda>();
            SkaitytiSportininkus(sportininkaiDuom, sportininkai);
            SkaitytiKomandas(komandosDuom, komandos);

            if (File.Exists(rez))
                File.Delete(rez);
            SpausdintiSportininkus(rez, sportininkai, "Pradiniai sportininkų duomenys:");
            SpausdintiKomandas(rez, komandos, "Pradiniai komandų duomenys:");

            List<Sportininkas> naujas = new List<Sportininkas>();
            Formuoti(sportininkai, naujas, komandos);
            SpausdintiSportininkus(rez, naujas, "Krepšininkai/futbolininkai, kurie žaidė visose varžybose ir atitiko vidurkio kriterijus:");

            naujas = naujas.OrderBy(x => x.Rikiuoti()).ToList();
            SpausdintiSportininkus(rez, naujas, "Surikiuotas masyvas:");

            Console.WriteLine("Programa baigė darbą!");
        }

        static void Formuoti(List<Sportininkas> A, List<Sportininkas> naujas, List<Komanda> komandos)
        {
            for (int i = 0; i < A.Count; i++)
            {
                Komanda tempKomanda = komandos.Find(x => (x.Pavadinimas == A[i].Komanda && x.Sportas == A[i].Sportas));
                double vidurkis = SkaiciuotiVidurki(A, tempKomanda);
                double papildomasVidurkis = SkaiciuotiPapildomaVidurki(A, tempKomanda);
                var tempSportininkas = A[i];
                Console.WriteLine($"Vidurkis: {vidurkis}");
                Console.WriteLine($"Papildomas vidurkis: {papildomasVidurkis}");

                if (tempSportininkas.Rungtynės == tempKomanda.Rungtynės)
                {
                    Console.WriteLine(A[i].Pavardė + " " + tempSportininkas.Rungtynės + " " + tempKomanda.Pavadinimas + " " + tempKomanda.Rungtynės);
                    if (tempSportininkas.SkaičiuotiVidurkį() >= vidurkis && tempSportininkas.SkaičiuotiPapildomąVidurkį() > papildomasVidurkis)
                    {
                        naujas.Add(A[i]);
                    }
                }
            }
        }

        static double SkaiciuotiPapildomaVidurki(List<Sportininkas> A, Komanda komanda)
        {
            double suma = 0;
            int kiekis = 0;
            for (int i = 0; i < A.Count; i++)
            {
                var temp = A[i];
                if (temp.Sportas == komanda.Sportas && temp.Komanda == komanda.Pavadinimas)
                {
                    suma += temp.SkaičiuotiPapildomąVidurkį();
                    kiekis++;
                }
            }

            return suma / kiekis;
        }

        static double SkaiciuotiVidurki(List<Sportininkas> A, Komanda komanda)
        {
            double suma = 0;
            int kiekis = 0;
            for (int i = 0; i < A.Count; i++)
            {
                var temp = A[i];
                if (temp.Sportas == komanda.Sportas && temp.Komanda == komanda.Pavadinimas)
                {
                    suma += temp.SkaičiuotiVidurkį();
                    kiekis++;
                }
            }

            return suma / kiekis;
        }

        static void SkaitytiSportininkus(string duom, List<Sportininkas> A)
        {
            using (StreamReader reader = new StreamReader(duom))
            {
                string line;
                string[] parts;
                string sportas;
                string komanda;
                string pavardė;
                string vardas;
                int rungtynės;

                while ((line = reader.ReadLine()) != null)
                {
                    parts = line.Split(';');
                    sportas = parts[0].Trim();
                    komanda = parts[1].Trim();
                    pavardė = parts[2].Trim();
                    vardas = parts[3].Trim();
                    rungtynės = int.Parse(parts[4].Trim());

                    if (sportas == "k")
                    {
                        int taškai = int.Parse(parts[5].Trim());
                        int atkovotiKamuoliai = int.Parse(parts[6].Trim());
                        int rezultatyvūsPerdavimai = int.Parse(parts[7].Trim());
                        Krepšininkas naujas = new Krepšininkas(sportas, komanda, pavardė, vardas, rungtynės, taškai, atkovotiKamuoliai, rezultatyvūsPerdavimai);
                        A.Add(naujas);
                    }
                    else if (sportas == "f")
                    {
                        int ivarciai = int.Parse(parts[5].Trim());
                        int geltonųKortelių = int.Parse(parts[6].Trim());
                        Futbolininkas naujas = new Futbolininkas(sportas, komanda, pavardė, vardas, rungtynės, ivarciai, geltonųKortelių);
                        A.Add(naujas);
                    }
                }
            }
        }

        static void SkaitytiKomandas(string duom, List<Komanda> A)
        {
            using (StreamReader reader = new StreamReader(duom))
            {
                string line;
                string[] parts;
                string sportas;
                string pavadinimas;
                string miestas;
                string treneris;
                int rungtynės;

                while ((line = reader.ReadLine()) != null)
                {
                    parts = line.Split(';');
                    sportas = parts[0].Trim();
                    pavadinimas = parts[1].Trim();
                    miestas = parts[2].Trim();
                    treneris = parts[3].Trim();
                    rungtynės = int.Parse(parts[4].Trim());
                    Komanda nauja = new Komanda(sportas, pavadinimas, miestas, treneris, rungtynės);
                    A.Add(nauja);
                }
            }
        }

        static void SpausdintiSportininkus(string rez, List<Sportininkas> A, string antraste)
        {
            const string virsus = "--------------------------------------------------------------------------------------------------------------------------------------------------------\r\n" +
                                  " Nr.  Pavardė         Vardas       Sportas      Komanda        Rungtynės  Taškai/Įvarčiai  Atkovoti kamuoliai/Geltonų kortelių  Rezultatyvūs perdavimai \r\n" +
                                  "--------------------------------------------------------------------------------------------------------------------------------------------------------";

            using (var fr = File.AppendText(rez))
            {
                fr.WriteLine(antraste);
                if (A.Count > 0)
                {
                    fr.WriteLine(virsus);
                    for (int i = 0; i < A.Count; i++)
                    {
                        Sportininkas sp = A[i];
                        fr.WriteLine(" {0, 2:d} {1}", i + 1, sp.ToString());
                    }
                    fr.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------\r\n");
                }
                else
                {
                    fr.WriteLine("Sąrašas tuščias!\n");
                }
            }
        }

        static void SpausdintiKomandas(string rez, List<Komanda> A, string antraste)
        {
            const string virsus = "-----------------------------------------------------------------------------------\r\n" +
                                  " Nr.  Pavadinimas  Sportas  Miestas         Treneris                     Rungtynės \r\n" +
                                  "-----------------------------------------------------------------------------------";

            using (var fr = File.AppendText(rez))
            {
                fr.WriteLine(antraste);
                if (A.Count > 0)
                {
                    fr.WriteLine(virsus);
                    for (int i = 0; i < A.Count; i++)
                    {
                        Komanda kom = A[i];
                        fr.WriteLine(" {0, 2:d} {1}", i + 1, kom.ToString());
                    }
                    fr.WriteLine("-----------------------------------------------------------------------------------\r\n");
                }
                else
                {
                    fr.WriteLine("Sąrašas tuščias!\n");
                }
            }
        }
    }
}
