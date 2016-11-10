using PostExpressGaleb.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace PostExpressGaleb.Common
{
    class PostExpressDal
    {
        public List<Primalac> VratiPrimaoce()
        {
            SQLiteConnection cnn = Konekcija.VratiKonekciju();
            SQLiteCommand cmd = new SQLiteCommand("select * from Primalac", cnn);
            var listaPrimalaca = new List<Primalac>();
            try
            {
                cnn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var p = new Primalac();
                        p.PrimalacId = Convert.ToInt32(dr["PrimalacId"]);
                        p.Naziv = dr["Naziv"].ToString();
                        p.Adresa = dr["Adresa"].ToString();
                        p.PostanskiBroj = dr["PostanskiBroj"].ToString();
                        p.Mesto = dr["Mesto"].ToString();
                        p.KontaktOsoba = dr["KontaktOsoba"].ToString();
                        p.Telefon = dr["Telefon"].ToString();
                        listaPrimalaca.Add(p);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                cnn.Close();
            }
            return listaPrimalaca;
        }

        public int DodajPrimaoica(Primalac p)
        {
            SQLiteConnection cnn = Konekcija.VratiKonekciju();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Primalac (Naziv, Adresa, PostanskiBroj,Mesto,KontaktOsoba,Telefon ) VALUES (@Naziv, @Adresa, @PostanskiBroj, @Mesto, @KontaktOsoba, @Telefon)", cnn);
            cmd.Parameters.AddWithValue("@Naziv", p.Naziv);
            cmd.Parameters.AddWithValue("@Adresa", p.Adresa);
            cmd.Parameters.AddWithValue("@PostanskiBroj", p.PostanskiBroj);
            cmd.Parameters.AddWithValue("@Mesto", p.Mesto);
            cmd.Parameters.AddWithValue("@KontaktOsoba", p.KontaktOsoba);
            cmd.Parameters.AddWithValue("@Telefon", p.Telefon);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }

        public int PromeniPrimaoica(Primalac p)
        {
            SQLiteConnection cnn = Konekcija.VratiKonekciju();
            SQLiteCommand cmd = new SQLiteCommand("UPDATE Primalac SET Naziv = @Naziv, Adresa = @Adresa, PostanskiBroj = @PostanskiBroj, Mesto=@Mesto, KontaktOsoba=@KontaktOsoba,Telefon=@Telefon WHERE PrimalacId = @PrimalacId", cnn);
            cmd.Parameters.AddWithValue("@Naziv", p.Naziv);
            cmd.Parameters.AddWithValue("@Adresa", p.Adresa);
            cmd.Parameters.AddWithValue("@PostanskiBroj", p.PostanskiBroj);
            cmd.Parameters.AddWithValue("@Mesto", p.Mesto);
            cmd.Parameters.AddWithValue("@KontaktOsoba", p.KontaktOsoba);
            cmd.Parameters.AddWithValue("@Telefon", p.Telefon);
            cmd.Parameters.AddWithValue("@PrimalacId", p.PrimalacId);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }

        public int DodajPosiljku(Posiljka pos)
        {
            SQLiteConnection cnn = Konekcija.VratiKonekciju();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Posiljka (PrimalacId, Vrednost, Otkupnina, Masa, Sadrzaj, PAK, DatumVreme) VALUES (@PrimalacId, @Vrednost, @Otkupnina, @Masa, @Sadrzaj, @PAK, @DatumVreme)", cnn);
            cmd.Parameters.AddWithValue("@PrimalacId", pos.PrimalacId);
            cmd.Parameters.AddWithValue("@Vrednost", pos.Vrednost);
            cmd.Parameters.AddWithValue("@Otkupnina", pos.Otkupnina);
            cmd.Parameters.AddWithValue("@Masa", pos.Masa);
            cmd.Parameters.AddWithValue("@Sadrzaj", pos.Sadrzaj);
            cmd.Parameters.AddWithValue("@PAK", pos.PAK);
            cmd.Parameters.AddWithValue("@DatumVreme", pos.DatumVreme);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }

        public int IzbrisiPosiljku(int posiljkaId)
        {
            SQLiteConnection cnn = Konekcija.VratiKonekciju();
            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Posiljka WHERE PosiljkaId = @PosiljkaId", cnn);
            cmd.Parameters.AddWithValue("@PosiljkaId", posiljkaId);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }

        public List<PosiljkaPrikaz> VratiPosiljke(DateTime startDate)
        {
            var sDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 00, 00, 00);
            var eDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 23, 59, 59);

            SQLiteConnection cnn = Konekcija.VratiKonekciju();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Select pr.Naziv, pr.Adresa, pr.Mesto, pr.PostanskiBroj, pr.Telefon, pr.KontaktOsoba, pos.Vrednost, pos.Masa, pos.Otkupnina,");
            sb.AppendLine(" pos.Sadrzaj, pos.PAK,pr.PrimalacId, pos.PosiljkaId, pos.DatumVreme");
            sb.AppendLine(" from Posiljka as pos");
            sb.AppendLine(" INNER JOIN Primalac as pr");
            sb.AppendLine(" ON pos.PrimalacId = pr.PrimalacId");
            sb.AppendLine(" WHERE pos.DatumVreme BETWEEN @sDate AND @eDate");
            sb.AppendLine(" order by pos.DatumVreme desc");

            SQLiteCommand cmd = new SQLiteCommand(sb.ToString(), cnn);
            cmd.Parameters.AddWithValue("sDate", sDate);
            cmd.Parameters.AddWithValue("eDate", eDate);

            var listaPosiljki = new List<PosiljkaPrikaz>();
            try
            {
                cnn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var pos = new PosiljkaPrikaz();
                        pos.PosiljkaId = Convert.ToInt32(dr["PosiljkaId"]);
                        pos.Naziv = dr["Naziv"].ToString();
                        pos.Adresa = dr["Adresa"].ToString();
                        pos.Mesto = dr["Mesto"].ToString();
                        pos.PostanskiBroj = dr["PostanskiBroj"].ToString();
                        pos.Telefon = dr["Telefon"].ToString();
                        pos.KontaktOsoba = dr["KontaktOsoba"].ToString();
                        pos.DatumVreme = Convert.ToDateTime(dr["DatumVreme"]);
                        pos.PrimalacId = Convert.ToInt32(dr["PrimalacId"]);
                        pos.Vrednost = Convert.ToDecimal(dr["Vrednost"]);
                        pos.Masa = Convert.ToDecimal(dr["Masa"]);
                        pos.Otkupnina = Convert.ToDecimal(dr["Otkupnina"]);
                        pos.Sadrzaj = dr["Sadrzaj"].ToString();
                        pos.PAK = dr["PAK"].ToString();
                        listaPosiljki.Add(pos);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                cnn.Close();
            }
            return listaPosiljki;
        }
    }
}
