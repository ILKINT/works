using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExample1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSource ds = new DataSource();
            List<Musteri> musteriListe = ds.musteriListesi();
            //  Console.WriteLine(musteriListe.Count());

            #region A ile başlayan müşteriler ve adedi
            #region Linq sorgusu olmadan 
            int count = 0;
            for (int i = 0; i < musteriListe.Count; i++)
            {
                // Console.WriteLine(musteriListe[i].isim[0]);
                if (musteriListe[i].isim[0] == 'A')
                {
                    //Console.WriteLine(musteriListe[i].isim);
                    count++;
                }
            }
           // Console.WriteLine("Müşterilerin toplam adedi {0}", count);
            #endregion
            #region Linq Sorgusu ile

            int toplamMusteriAdet = 0;
            toplamMusteriAdet = musteriListe.Where(m => m.isim.StartsWith("A")).Count();

           // Console.WriteLine("Müşterilerin toplam adedi(Linq Sorgusu ile) {0}", toplamMusteriAdet);
            #endregion

            #endregion

            #region Linq sorgulama çeşitleri
            //1. Yol  Linq Metot
            int toplamMusteriAdet1 = musteriListe.Where(m => m.isim.StartsWith("A")).Count();

            //2. Yol
            int toplamMusteriAdt2 = (from m in musteriListe
                                     where m.isim.StartsWith("A")
                                     select m).Count();
            #endregion

            #region ALIŞTIRMALAR

            //1- Müşteriler içerisinde ülke ismi a ile başlayan müşterileri Linq metot kullanarak bulalım.
            IEnumerable<Musteri> musteriListeAlistirma1 = musteriListe.Where(m => m.ulke.StartsWith("a"));
          //  Console.WriteLine(musteriListeAlistirma1.Count());

            //2- Müşteriler içerisinde isminin içinde b harfi geçen ve ülke değeri içinde a harfi geçen müşterileri getirin 
            List<Musteri> musteriListeAlistirma2 = musteriListe.Where(m => m.isim.Contains("b") && m.ulke.Contains("a")).ToList();

           // Console.WriteLine(musteriListeAlistirma2.Count());

            //3- Müşteri listesi içindeki kayıtlardan doğum yılı 1990 dan büyük olan ve isminin içerisinde a harfi geçen isimleri Linq To Query ile bulalım

            var musteriListeAlistirma3 = from m in musteriListe
                                         where m.dTarih.Year > 1990 && m.isim.Contains("a")
                                         select m;

           // Console.WriteLine(musteriListeAlistirma2.Count());

            //4- Müşteri listesi içindeki kayıtlardan doğum yılı 1990 dan büyük olan veya isminin içerisinde a harfi geçen isimleri Linq To Query ile bulalım

            var musteriListeAlistirma4 = from m in musteriListe
                                         where m.dTarih.Year > 1990 || m.isim.Contains("a")
                                         select m;


            //Console.WriteLine(musteriListeAlistirma4.Count());


            #endregion

            #region Linq sorgularında delegate kullanımı =>

            var delegateKullanimi1 = musteriListe.Where(m => m.isim.StartsWith("A"));
            Func<Musteri, bool> funcDelegate1 = new Func<Musteri, bool>(FuncDelegateKullanimi);

            var delegateKullanimi2 = musteriListe.Where(funcDelegate1);
            delegateKullanimi2 = musteriListe.Where(FuncDelegateKullanimi);

            var delegateKullanimi3 = musteriListe.Where(new Func<Musteri, bool>(FuncDelegateKullanimi));

            var delegateKullanimi4 = musteriListe.Where(delegate (Musteri m) { return m.isim[0] == 'A' ? true : false; });

            var delegateKullanimi5 = musteriListe.Where((Musteri m) => { return m.isim[0] == 'A' ? true : false; });

            var delegateKullanimi6 = musteriListe.Where(m => { return m.isim[0] == 'A' ? true : false; });

            var delegateKullanini7 = musteriListe.Where(m => m.isim[0] == 'A');


            #endregion

            #region Linq sorgularında predicate delegate kullanımı

            Predicate<Musteri> predicate = new Predicate<Musteri>(FuncPredicateDelegate);
            var delegateKullanimiPredicateDelegate1 = musteriListe.FindAll(predicate);

            var delegateKullanimiPredicateDelegate2 = musteriListe.FindAll(new Predicate<Musteri>(FuncPredicateDelegate));

            var delegateKullanimiPredicateDelegate3 = musteriListe.FindAll(delegate (Musteri m) { return m.dTarih.Year > 1990 ? true : false; });

            var delegateKullanimiPredicateDelegate4 = musteriListe.FindAll((Musteri m) => { return m.dTarih.Year > 1990 ? true : false; });

            var delegateKullanimiPredicateDelegate5 = musteriListe.FindAll((m) => { return m.dTarih.Year > 1990 ? true : false; });

            var delegateKullanimiPredicateDelegate6 = musteriListe.FindAll(m => m.dTarih.Year > 1990);

            #endregion

            #region Action Delegate Kullanımı

            Action<Musteri> actionMusteri1 = new Action<Musteri>(FuncActionDelegate);
            musteriListe.ForEach(actionMusteri1);

            musteriListe.ForEach(new Action<Musteri>(FuncActionDelegate));

            musteriListe.ForEach(delegate (Musteri m) { Console.WriteLine(m.isim + " " + m.soyisim); });

            musteriListe.ForEach((Musteri m) => { Console.WriteLine(m.isim + " " + m.soyisim); });

            musteriListe.ForEach((m) => { Console.WriteLine(m.isim + " " + m.soyisim); });

            #endregion

            #region Linq inceleme ve ara ödevler

            //Müşteri Listesi içinde bulunan kayıtlardan ismi a ile başlayan, soyisim değerinin içinde e olan ve doğum yılı 1985 den büyük olan kayıtları getirsin

            List<Musteri> musteriler = musteriListe.Where(m => m.isim.StartsWith("A") && m.soyisim.Contains("e") && m.dTarih.Year > 1985).ToList();
            Console.WriteLine(musteriler.Count);

            #endregion

            Console.ReadLine();
        }

        static bool FuncDelegateKullanimi(Musteri m)
        {
            if (m.isim[0] == 'A')
                return true;
            else
                return false;
        }

        static bool FuncPredicateDelegate(Musteri m)
        {
            if (m.dTarih.Year > 1990)
                return true;
            else return false;
        }

        static void FuncActionDelegate(Musteri m)
        {
          //  Console.WriteLine(m.isim + " " + m.soyisim);
        }
    }
}
