using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using PostExpressGaleb.Common;
using PostExpressGaleb.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PostExpressGaleb
{
    public partial class MainWindow : MetroWindow
    {
        #region Inicijalizacija
        PostExpressDal pDal = new PostExpressDal();
        HSSFWorkbook wb;
        HSSFSheet sh;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = pDal.VratiPrimaoce();
            listView1.ItemsSource = pDal.VratiPosiljke(DateTime.Now);

        }

        private void listBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Primalac p = listBox.SelectedItem as Primalac;
            if (p != null)
            {
                txtNaziv.Text = p.Naziv;
                txtAdresa.Text = p.Adresa;
                txtKontaktOsoba.Text = p.KontaktOsoba;
                txtTelefon.Text = p.Telefon;
                txtPostBroj.Text = p.PostanskiBroj.ToString();
                txtMesto.Text = p.Mesto;
            }
        }

        private void btnNovUnos_Click(object sender, RoutedEventArgs e)
        {
            OcistiTextBoxove();
        }

        private void myMessageBox(string title, string message)
        {
            var mySettings = new MetroDialogSettings()
            {
                AnimateShow = true,
                AnimateHide = false,
                ColorScheme = MetroDialogColorScheme.Theme
            };
            this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, mySettings);
        }

        private void OcistiTextBoxove()
        {
            txtNaziv.Text = "";
            txtAdresa.Text = "";
            txtKontaktOsoba.Text = "";
            txtTelefon.Text = "";
            txtPostBroj.Text = "";
            txtMesto.Text = "";
            listBox.SelectedIndex = -1;
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (txtNaziv.Text.Trim() == string.Empty)
            {
                myMessageBox("Obaveštenje", "Morate uneti polje naziv!");
                return;
            }

            if (listBox.SelectedIndex == -1)
            {
                // nov unos
                Primalac p = new Primalac();
                p.Naziv = txtNaziv.Text.Trim();
                p.Adresa = txtAdresa.Text.Trim();
                p.KontaktOsoba = txtKontaktOsoba.Text.Trim();
                p.Mesto = txtMesto.Text.Trim();
                p.PostanskiBroj = txtPostBroj.Text.Trim();
                p.Telefon = txtTelefon.Text.Trim();
                int rez = pDal.DodajPrimaoica(p);
                if (rez == 1)
                {
                    myMessageBox("Obaveštenje", "Uspešno sačuvano!");
                    listBox.ItemsSource = pDal.VratiPrimaoce();
                    listBox.Items.Refresh();
                    OcistiTextBoxove();
                }
                else
                {
                    myMessageBox("Obaveštenje", "Došlo je do greške!");
                }
            }
            else
            {
                // update unosa
                var p = listBox.SelectedItem as Primalac;
                p.Naziv = txtNaziv.Text;
                p.Adresa = txtAdresa.Text;
                p.KontaktOsoba = txtKontaktOsoba.Text;
                p.Telefon = txtTelefon.Text;
                int pBroj;
                int.TryParse(txtPostBroj.Text.Trim(), out pBroj);
                p.PostanskiBroj = txtPostBroj.Text.Trim();
                p.Mesto = txtMesto.Text;
                int rez = pDal.PromeniPrimaoica(p);
                if (rez == 1)
                {
                    myMessageBox("Obaveštenje", "Uspešno promenjeno!");
                    listBox.ItemsSource = pDal.VratiPrimaoce();
                    listBox.Items.Refresh();
                }
                else
                {
                    myMessageBox("Obaveštenje", "Došlo je do greške!");
                }
            }
        }

        private void txtPretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var lista = pDal.VratiPrimaoce().FindAll(p => p.Naziv.ToUpper().Contains(txtPretraga.Text.ToUpper()));
            listBox.ItemsSource = lista;
        }

        private void btnSacuvajPosiljku_Click(object sender, RoutedEventArgs e)
        {
            var p = listBox.SelectedItem as Primalac;
            if (p != null)
            {
                var pos = new Posiljka();
                pos.DatumVreme = DateTime.Now;
                pos.PrimalacId = p.PrimalacId;
                pos.Vrednost = Convert.ToDecimal(txtVrednost.Text.Trim());
                pos.Masa = Convert.ToDecimal(txtMasa.Text.Trim());
                pos.Otkupnina = Convert.ToDecimal(txtOtkupnina.Text.Trim());
                pos.Sadrzaj = txtSadrzaj.Text;
                pos.PAK = txtSadrzaj.Text.Trim();
                int rez = pDal.DodajPosiljku(pos);
                if (rez == 1)
                {
                    myMessageBox("Obaveštenje", "Uspešno sačuvana pošiljka!");
                    listView1.ItemsSource = pDal.VratiPosiljke(DateTime.Now);
                    listView1.Items.Refresh();
                    txtSadrzaj.Text = "";
                    txtOtkupnina.Text = "";
                    txtVrednost.Text = "";
                    txtMasa.Text = "";
                    txtPak.Text = "";
                }
                else
                {
                    myMessageBox("Obaveštenje", "Došlo je do greške!");
                }
            }
            else
            {
                myMessageBox("Obaveštenje", "Morate odabrati primaoca!");
            }
        }
             
        private void btnIzvozPdf_Click(object sender, RoutedEventArgs e)
        {
            var posiljka = listView1.SelectedItem as PosiljkaPrikaz;

            if (posiljka != null)
            {
                wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                sh = (HSSFSheet)wb.CreateSheet("Sheet1");              
                sh.CreateRow(0).CreateCell(5).SetCellValue(posiljka.Naziv);

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = "..xls"; // Default file extension
                dlg.Filter = "Excel files (..xls)|*..xls"; // Filter files by extension

                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                    using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    {
                        wb.Write(fs);
                    }

                }
            }
            else
            {
                myMessageBox("Obaveštenje", "Morate odabrati pošiljku!");
            }
        }

        private void dtPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;

            DateTime? date = picker.SelectedDate;

            if (date != null)
            {
                listView1.ItemsSource = pDal.VratiPosiljke(date.Value);
            }           
        }

        private void btnIzbrisi_Click(object sender, RoutedEventArgs e)
        {
            var posiljka = listView1.SelectedItem as PosiljkaPrikaz;
            if (posiljka == null)
            {
                myMessageBox("Obaveštenje", "Odaberite zapis u listi!");
                return;
            }

            int rez = pDal.IzbrisiPosiljku(posiljka.PosiljkaId);

            if (rez == 1)
            {
                myMessageBox("Obaveštenje", "Pošiljka je uspešno obrisana!");
                if (dtPicker.SelectedDate == null)
                {
                    listView1.ItemsSource = pDal.VratiPosiljke(DateTime.Now);
                }
                else
                {
                    listView1.ItemsSource = pDal.VratiPosiljke(dtPicker.SelectedDate.Value);
                }
                
            }
            else
            {
                myMessageBox("Obaveštenje", "Došlo je do greške!");
            }
        }
    }
}
