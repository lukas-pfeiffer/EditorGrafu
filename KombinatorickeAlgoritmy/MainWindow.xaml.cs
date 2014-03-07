using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;

namespace KombinatorickeAlgoritmy
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int pocetVrcholu = 0;
        int pocetHran = 0;
        Ellipse[] znakVrchol = new Ellipse[102];
        Label[] nazevVrcholu = new Label[102];
        Line[] znakHrana = new Line[102];
        Label[] nazevHrany = new Label[102];
        int[,] maticeSousednosti = new int[102, 102];
        int[,] maticeIncidence = new int[102, 102];
        int[] ohodnoceniVrcholu = new int[102];
        int[] ohodnoceniHran = new int[102];

        public MainWindow()
        {
            InitializeComponent();
            platnoMenu.Children.Add(vyklesleniMenu(Brushes.LightGray, Brushes.WhiteSmoke, 19, 370));
            tlacitkaMenu();
            kontextMenu();
        }
        #region vrcholy
        private void PridaniVrcholu(MouseButtonEventArgs e)
        {
            if (pocetVrcholu == 100)
            {
                MessageBox.Show("Maximální počet vzrcholů je omezen na " + pocetVrcholu + ".", "Omezení", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Point bod = new Point();
                bod.X = e.GetPosition(platno).X;
                bod.Y = e.GetPosition(platno).Y;
                int x = (int)bod.X;
                int y = (int)bod.Y;

                Brush barva = Brushes.Black;
                pocetVrcholu++;

                vytvoreniVrcholu(pocetVrcholu, x, y, barva,0);

                /*if (vrcholyZnak[pocetVrcholu] != null)
                {
                    MessageBox.Show("Na těchto souřadnicích již existuje vrchol.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {*/
                /* pocetVrcholu++;
                 vrcholyZnak[pocetVrcholu] = new Ellipse();
                 vrcholyZnak[pocetVrcholu].Fill = Brushes.Black;
                 vrcholyZnak[pocetVrcholu].Height = vrcholyZnak[pocetVrcholu].Width = 10;
                 //vrcholyZnak[pocetVrcholu].Name = x + "x" + y;
                 //poleVrcholu[x, y].MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;

                 Canvas.SetLeft(vrcholyZnak[pocetVrcholu], bod.X - 5);
                 Canvas.SetTop(vrcholyZnak[pocetVrcholu], bod.Y - 5);

                 platno.Children.Add(vrcholyZnak[pocetVrcholu]);
                 lbVrcholy.Items.Add(string.Format("v{0}, {1}, {2}", pocetVrcholu, x, y));


                 nazevVrcholu[pocetVrcholu] = new Label();
                 nazevVrcholu[pocetVrcholu].Content = "v" + pocetVrcholu;

                 Canvas.SetLeft(nazevVrcholu[pocetVrcholu], bod.X);
                 Canvas.SetTop(nazevVrcholu[pocetVrcholu], bod.Y);
                 platno.Children.Add(nazevVrcholu[pocetVrcholu]);*/

                //lbVrcholy.Items.Add(string.Format("Vrchol v{0}, x = {1}, y = {2}", pocetVrcholu, x, y));
                // }
            }
        }

        private void vytvoreniVrcholu(int cislo, int x, int y, Brush barva, int ohodnoceni)
        {
            znakVrchol[cislo] = new Ellipse();
            znakVrchol[cislo].Fill = barva;
            znakVrchol[cislo].Height = znakVrchol[cislo].Width = 10;
            znakVrchol[cislo].Name = "vrchol";
            //vrcholyZnak[pocetVrcholu].Name = x + "x" + y;
            //poleVrcholu[x, y].MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;

            Canvas.SetLeft(znakVrchol[cislo], x - 5);
            Canvas.SetTop(znakVrchol[cislo], y - 5);

            platno.Children.Add(znakVrchol[cislo]);
            lbVrcholy.Items.Add(string.Format("v{0}, {1}, {2}", cislo, x, y));

            nazevVrcholu[cislo] = new Label();
            nazevVrcholu[cislo].Content = "v" + cislo;

            ohodnoceniVrcholu[cislo] = new int();
            ohodnoceniVrcholu[cislo] = ohodnoceni;

            Canvas.SetLeft(nazevVrcholu[cislo], x);
            Canvas.SetTop(nazevVrcholu[cislo], y);
            platno.Children.Add(nazevVrcholu[cislo]);
        }

        private void platno_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PridaniVrcholu(e);
        }

        #endregion

        #region hrany
        private void nakresleniHrany(int vrchol1, int vrchol2)
        {
            int x1 = (int)Canvas.GetLeft(znakVrchol[vrchol1]) + 5;
            int y1 = (int)Canvas.GetTop(znakVrchol[vrchol1]) + 5;
            int x2 = (int)Canvas.GetLeft(znakVrchol[vrchol2]) + 5;
            int y2 = (int)Canvas.GetTop(znakVrchol[vrchol2]) + 5;

            bool pravda = true;
                if (x1 == x2 && y1 == y2)
                {
                    MessageBox.Show("Není možné aby hrana vedla z vrcholu v" + vrchol1 + " do v" + vrchol2, "Omezení", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    pravda = false;
                }
                for (int i = 1; i <= pocetHran; i++)
                {
                    if (znakHrana[i].X1 == x1 &&
                    znakHrana[i].Y1 == y1 &&
                    znakHrana[i].X2 == x2 &&
                    znakHrana[i].Y2 == y2)
                    {
                        MessageBox.Show("Hrana už existuje mezi vrcholi v" + vrchol1 + " do v" + vrchol2, "Hrana existuje", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        pravda = false;
                    }
                    if (znakHrana[i].X1 == x2 &&
                     znakHrana[i].Y1 == y2 &&
                     znakHrana[i].X2 == x1 &&
                     znakHrana[i].Y2 == y1)
                    {
                        MessageBox.Show("Hrana už existuje mezi vrcholi v" + vrchol1 + " do v" + vrchol2, "Hrana existuje", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        pravda = false;
                    }
                }
            if (pravda)
            {
                pocetHran++;
                vytvoreniHrany(pocetHran, x1, y1, x2, y2, Brushes.Black,0);
            }
        }

        private void vytvoreniHrany(int cislo, int x1, int y1, int x2, int y2, Brush barva, int ohodnoceni)
        {
            znakHrana[cislo] = new Line();
            znakHrana[cislo].Stroke = barva;
            znakHrana[cislo].StrokeThickness = 1;
            znakHrana[cislo].Name = "hrana";//"e" + cislo;

            znakHrana[cislo].X1 = x1;
            znakHrana[cislo].Y1 = y1;
            znakHrana[cislo].X2 = x2;
            znakHrana[cislo].Y2 = y2;

            platno.Children.Add(znakHrana[cislo]);

            lbVrcholy.Items.Add(string.Format("e{0}, {1}, {2}, {3}, {4}", cislo, x1, y1, x2, y2));

            nazevHrany[cislo] = new Label();
            nazevHrany[cislo].Content = "e" + cislo;//znakHrana[cislo].Name;

            ohodnoceniHran[cislo] = new int();
            ohodnoceniHran[cislo] = ohodnoceni;

            Canvas.SetLeft(nazevHrany[cislo], (x1 + x2) / 2);
            Canvas.SetTop(nazevHrany[cislo], (y1 + y2) / 2);

            platno.Children.Add(nazevHrany[cislo]);
        }
        #endregion

        #region
        private void odstraneni()
        {
            string[] vrchol = new string[2];
            vrchol = vyber();
            if (vrchol[0] == "v")
            {
                smazaniVrcholu(int.Parse(vrchol[1]));
            }
            else if (vrchol[0] == "e")
            {
                smazaniHrany(int.Parse(vrchol[1]),false);
            }
            else
            {
                MessageBox.Show("Nejedná se o vrchol ani o hranu.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            return;
        }

        private void smazaniVrcholu(int cisloVrcholu)
        {
            platno.Children.Remove(znakVrchol[cisloVrcholu]);
            znakVrchol[cisloVrcholu] = new Ellipse();
            platno.Children.Remove(nazevVrcholu[cisloVrcholu]);
            nazevVrcholu[cisloVrcholu] = new Label();
            ohodnoceniVrcholu[cisloVrcholu] = new int();

            int[] sousedi = new int[102];
            sousedi = najdiSousedy(cisloVrcholu);
            int pomV = zjisteniPoctuVrcholu();
            
            for (int i = 1; i < pomV; i++)
            {
                maticeSousednosti[cisloVrcholu, i] = 0;
                maticeSousednosti[i, cisloVrcholu] = 0;
            }

            int pomH = zjisteniPoctuHran();

            for (int i = 1; i < pomH; i++)
            {
                maticeIncidence[cisloVrcholu, i] = 0;
            }


            lbVrcholy.Items.RemoveAt(lbVrcholy.Items.IndexOf(lbVrcholy.SelectedItem));
        }

        private void smazaniHrany(int cisloHrany, bool mKostra)
        {
            string[] souradnice = new string[10];
            souradnice = infoHrana(cisloHrany);

            platno.Children.Remove(znakHrana[cisloHrany]);
            znakHrana[cisloHrany] = new Line();
            platno.Children.Remove(nazevHrany[cisloHrany]);
            nazevHrany[cisloHrany] = new Label();
            ohodnoceniHran[cisloHrany] = new int();


            int v1 = najdiVrcholyHrany1(cisloHrany, int.Parse(souradnice[1]),int.Parse(souradnice[2]));
            int v2 = najdiVrcholyHrany2(cisloHrany, int.Parse(souradnice[3]), int.Parse(souradnice[4]));
            
            maticeSousednosti[v1, v2] = 0;
            maticeSousednosti[v2, v1] = 0;
            

            int pomV = zjisteniPoctuVrcholu();

            for (int i = 0; i < pomV; i++)
            {
                maticeIncidence[i, cisloHrany] = 0;
            }

            try
            {
                if (mKostra==false)
                {
                    lbVrcholy.Items.RemoveAt(lbVrcholy.Items.IndexOf(lbVrcholy.SelectedItem));                    
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                //MessageBox.Show(e.Message);
            }
        }

        private int najdiVrcholyHrany1(int cisloHrany, int x1, int y1)
        {
           // string[] souradnice = new string[10];
           // souradnice = infoHrana(cisloHrany);
            /*informace[1] = znakHrana[cisloHrany].X1.ToString();
            informace[2] = znakHrana[cisloHrany].Y1.ToString();
            informace[3] = znakHrana[cisloHrany].X2.ToString();
            informace[4] = znakHrana[cisloHrany].Y2.ToString();*/

           // int x1 = int.Parse(souradnice[1]);
            
            //int x1 = 
           // int y1 = ;
            //int x2 = (int)znakHrana[cisloHrany].X2;
            //int y2 = (int)znakHrana[cisloHrany].Y2;
            
            int pomV = zjisteniPoctuVrcholu();

            for (int i = 1; i < pomV; i++)
            {
                if (Canvas.GetLeft(znakVrchol[i]) == x1-5)
                {
                    if (Canvas.GetTop(znakVrchol[i]) == y1-5)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        private int najdiVrcholyHrany2(int cisloHrany,int x2,int y2)
        {
           // int x1 = (int)znakHrana[cisloHrany].X1;
           // int y1 = (int)znakHrana[cisloHrany].Y1;
           // int x2 = (int)znakHrana[cisloHrany].X2;
           // int y2 = (int)znakHrana[cisloHrany].Y2;

            int pomV = zjisteniPoctuVrcholu();

            for (int i = 1; i < pomV; i++)
            {
                if (Canvas.GetLeft(znakVrchol[i]) == x2-5)
                {
                    if (Canvas.GetTop(znakVrchol[i]) == y2-5)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
      
        private void lbVrcholy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] pom = new string[2];
            pom = vyber();
            if (pom[0] == "v")
            {
                infoVrcholu(int.Parse(pom[1]));
            }
            else if (pom[0] == "e")
            {
                infoHrana(int.Parse(pom[1]));
            }
            return;
        }

        private string[] vyber()
        {
            string vyber;
            try
            {
                vyber = lbVrcholy.SelectedItem.ToString();

            }
            catch (NullReferenceException)
            {
                vyber = "chyba";
            }


            string[] vstup = new string[4];
            string[] vystup = new string[4];
            char deliciZnak = ',';

            vstup = vyber.Split(deliciZnak);

            vystup[0] = vstup[0].Substring(0, 1);

            try
            {
                vystup[1] = vstup[0].Substring(1, 2);
            }
            catch (ArgumentOutOfRangeException)
            {
                vystup[1] = vstup[0].Substring(1, 1);
            }

            return vystup;
        }

        bool vrcholyPravda = false;
        int vrchol1;
        int vrchol2;

        private void pridaniHrany()
        {
            string[] vrcholy = new string[2];
            vrcholy = vyber();

            if (vrcholy[0] == "v" && vrcholyPravda == false)
            {
                vrchol1 = int.Parse(vrcholy[1]);
                vrcholyPravda = true;
            }
            else if (vrcholy[0] == "v" && vrcholyPravda == true)
            {
                vrchol2 = int.Parse(vrcholy[1]);
                nakresleniHrany(vrchol1, vrchol2);
                vytvoreniMaticeSousednice(vrchol1, vrchol2);
                vytvoreniMaticeIncidence(vrchol1, vrchol2, pocetHran);
                vrcholyPravda = false;
            }
        }

        private void vytvoreniMaticeSousednice(int vrchol1, int vrchol2)
        {
            if (vrchol1!=vrchol2)
            {
            maticeSousednosti[vrchol1, vrchol2] = 1;
            maticeSousednosti[vrchol2, vrchol1] = 1;   
            }
        }

        private void vytvoreniMaticeIncidence(int vrchol1, int vrchol2, int cisloHrany)
        {
            maticeIncidence[vrchol1, cisloHrany] = 1;
            maticeIncidence[vrchol2, cisloHrany] = 1;
        }

        private string[] infoVrcholu(int cisloVrcholu)
        {
            string[] informace = new string[10];

            informace[0] = "v" + cisloVrcholu;
            informace[1] = (Canvas.GetLeft(znakVrchol[cisloVrcholu]) + 5).ToString();
            informace[2] = (Canvas.GetTop(znakVrchol[cisloVrcholu]) + 5).ToString();
            try
            {
                informace[3] = znakVrchol[cisloVrcholu].Fill.ToString();
            }
            catch (NullReferenceException)
            {
                informace[3] = "Není Barva";
            }

            informace[4] = ohodnoceniVrcholu[cisloVrcholu].ToString();

            lNazev.Content = informace[0];
            txbBarva.Text = informace[3];
            lSouradnice.Content = "x: " + informace[1] + " y: " + informace[2];  //vrcholyZnak[cisloVrcholu].Name;
            txbOhodnoceni.Text = informace[4];
            txbPocVrcholKostra.Text = cisloVrcholu.ToString();
            txbPocVrcholProhledavani.Text = cisloVrcholu.ToString();

            return informace;
        }

        private string[] infoHrana(int cisloHrany)
        {
            string[] informace = new string[10];

            informace[0] = "e" + cisloHrany;
            //informace[1] = Canvas.GetLeft(hrana[cisloHrany]).ToString();
            //informace[2] = Canvas.GetTop(hrana[cisloHrany]).ToString();

            informace[1] = znakHrana[cisloHrany].X1.ToString();
            informace[2] = znakHrana[cisloHrany].Y1.ToString();
            informace[3] = znakHrana[cisloHrany].X2.ToString();
            informace[4] = znakHrana[cisloHrany].Y2.ToString();
            try
            {
                informace[5] = znakHrana[cisloHrany].Stroke.ToString();
            }
            catch (NullReferenceException)
            {
                informace[5] = "Neni barva";
            }
            informace[6] = ohodnoceniHran[cisloHrany].ToString();

            lNazev.Content = informace[0];
            txbBarva.Text = informace[5];
            lSouradnice.Content = "x1: " + informace[1] + " y1: " + informace[2] + "| x2: " + informace[3] + " y2: " + informace[4];  //vrcholyZnak[cisloVrcholu].Name;
            txbOhodnoceni.Text = informace[6];

            return informace;
        }

        private int zjisteniPoctuVrcholu()
        {
            int pocet = 0;
            for (int i = 1; i < 103; i++)
            {
                try
                {
                    if (znakVrchol[i].Name == "")
                    {

                    }

                }
                catch (NullReferenceException)
                {
                    return i;
                }
                catch (IndexOutOfRangeException)
                {
                    return 100;
                }
            }

            return pocet;
        }

        private int zjisteniPoctuHran()
        {
            int pocet = 0;
            for (int i = 1; i < 102; i++)
            {
                try
                {
                    if (znakHrana[i].Name == "")
                    {

                    }

                }
                catch (NullReferenceException)
                {
                    return i;
                }
            }

            return pocet;
        }

        private void ulozeni()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Soubor s grafem (*.grf)|*.grf";
            sfd.FileName = "Graf";
            sfd.Title = "Uložení grafu";


            if (sfd.ShowDialog() == true)
            {
                int pomV = zjisteniPoctuVrcholu();
                int pomH = zjisteniPoctuHran();

                using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                {
                    string[] pole = new string[10];

                    for (int i = 1; i < pomV; i++)
                    {
                        pole = infoVrcholu(i);
                        sw.WriteLine(pole[0] + "," + pole[1] + "," + pole[2] + "," + pole[3] + "," + pole[4]);
                    }

                    string[] pole2 = new string[10];

                    for (int i = 1; i < pomH; i++)
                    {
                        pole2 = infoHrana(i);
                        sw.WriteLine(pole2[0] + "," + pole2[1] + "," + pole2[2] + "," + pole2[3] + "," + pole2[4] + "," + pole2[5] + "," + pole2[6]);
                    }

                    for (int i = 1; i < pomV; i++)
                    {
                    sw.Write("s");
                        for (int j = 1; j < pomV; j++)
                        {
                            sw.Write("," + maticeSousednosti[i, j]);
                        }
                        sw.WriteLine();
                    }

                    for (int i = 1; i < pomV; i++)
                    {
                    sw.Write("i");
                        for (int j = 1; j < pomH; j++)
                        {
                            sw.Write("," + maticeIncidence[i, j]);
                        }
                        sw.WriteLine();
                    }
                }
            }
        }

        private void nacteni()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Soubor s grafem (*.grf)|*.grf";
            ofd.FileName = "Graf";
            ofd.Title = "Načtení grafu";

            if (ofd.ShowDialog() == true)
            {
                using (StreamReader sr = new StreamReader(ofd.OpenFile()))
                {
                    bool bVcholy = true;
                    //bool bHrany = true;
                    int cSousednosti = 1;
                    int cIncidentni = 1;

                    while (bVcholy)
                    {
                        string vstup;

                        vstup = sr.ReadLine();

                        string[] vystup = new string[10];
                        char deliciZnak = ',';

                        try
                        {
                            vystup = vstup.Split(deliciZnak);
                        }

                        catch (NullReferenceException)
                        {
                            break;
                        }
                        if (vystup[0].Substring(0, 1) == "v")
                        {
                            int pom = -1;
                            try
                            {
                                pom = int.Parse(vystup[0].Substring(1, 2));
                                vytvoreniVrcholu(int.Parse(vystup[0].Substring(1, 2)), int.Parse(vystup[1]), int.Parse(vystup[2]), ziskejBarvu(vystup[3]), int.Parse(vystup[4]));
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                pom = int.Parse(vystup[0].Substring(1, 1));
                                vytvoreniVrcholu(int.Parse(vystup[0].Substring(1, 1)), int.Parse(vystup[1]), int.Parse(vystup[2]), ziskejBarvu(vystup[3]), int.Parse(vystup[4]));
                            }
                            catch (FormatException)
                            {
                                lbVrcholy.Items.Add("v" + pom + " nebyl načten");
                            }
                            pocetVrcholu++;
                        }
                        else if (vystup[0].Substring(0, 1) == "e")
                        {
                            int pom = -1;
                            try
                            {
                                pom = int.Parse(vystup[0].Substring(1, 2));
                                vytvoreniHrany(int.Parse(vystup[0].Substring(1, 2)), int.Parse(vystup[1]), int.Parse(vystup[2]), int.Parse(vystup[3]), int.Parse(vystup[4]), ziskejBarvu(vystup[5]), int.Parse(vystup[6]));
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                pom = int.Parse(vystup[0].Substring(1, 1));
                                vytvoreniHrany(int.Parse(vystup[0].Substring(1, 1)), int.Parse(vystup[1]), int.Parse(vystup[2]), int.Parse(vystup[3]), int.Parse(vystup[4]), ziskejBarvu(vystup[5]), int.Parse(vystup[6]));
                            }
                            catch (FormatException)
                            {
                                lbVrcholy.Items.Add("e" + pom + " nebyl načten");
                            }
                            pocetHran++;
                        }
                        else if (vystup[0].Substring(0, 1) == "s")
                        {
                            int pom = zjisteniPoctuVrcholu();
                            for (int i = 1; i < pom; i++)
                            {                                                           
                                maticeSousednosti[i, cSousednosti] = int.Parse(vystup[i]);
                            }
                            cSousednosti++;
                        }
                        else if (vystup[0].Substring(0, 1) == "i")
                        {
                            int pom = zjisteniPoctuHran();
                            for (int i = 1; i < pom; i++)
                            {
                                maticeIncidence[cIncidentni, i] = int.Parse(vystup[i]);
                            }
                            cIncidentni++;
                        }
                    }
                    /*
                    int pomV = zjisteniPoctuVrcholu();
                    int pomH = zjisteniPoctuHran();

                    while (true)
                    {
                        sw.WriteLine("ms");
                        for (int i = 1; i < pomV; i++)
                        {
                            for (int j = 1; j < pomV; j++)
                            {
                                sw.Write(maticeSousednosti[i, j] + ",");
                            }
                            sw.WriteLine();
                            sw.WriteLine();
                        }                        
                    }

                    while (true)
                    {
                        sw.WriteLine("mi");
                        for (int i = 1; i < pomV; i++)
                        {
                            for (int j = 1; j < pomH; j++)
                            {
                                sw.Write(maticeIncidence[i, j] + ",");
                            }
                            sw.WriteLine();
                            sw.WriteLine();
                        }
                    }*/
                }
            }
        }

        public void vycisteni()
        {
            platno.Children.Clear();
            lbVrcholy.Items.Clear();
            pocetHran = 0;
            pocetVrcholu = 0;
            znakVrchol = new Ellipse[101];
            nazevVrcholu = new Label[101];
            znakHrana = new Line[101];
            nazevHrany = new Label[101];
            maticeSousednosti = new int[100, 100];
            maticeIncidence = new int[100, 100];
            ulozeniMatice();
        }

        private Brush ziskejBarvu(string hexBarva)
        {
            var converter = new System.Windows.Media.BrushConverter();
            Brush brush;
            try
            {
                brush = (Brush)converter.ConvertFromString(hexBarva);
            }
            catch (FormatException)
            {
                brush = Brushes.Black;
            }
            catch (NotSupportedException)
            {
                brush = Brushes.Black;
            }
            return brush;
        }
        #endregion

        #region Hlavní menu

        Menu menu;

        public Menu vyklesleniMenu(Brush barvaMenu, Brush barvaOkraje, int vyska, int sirka)
        {
            menu = new Menu();
            menu.Background = barvaMenu;
            menu.BorderBrush = barvaOkraje;
            menu.Height = vyska;
            menu.Width = sirka;

            return menu;
        }

        public void tlacitkaMenu()
        {
            Separator oddelovac = new Separator();

            MenuItem soubor = new MenuItem();
            soubor.Header = "Soubor";
            //soubor.Height = 20;

            MenuItem novyGraf = new MenuItem();
            novyGraf.Header = "Nový graf";
            novyGraf.Click += novyGraf_Click;

            MenuItem ulozit = new MenuItem();
            ulozit.Header = "Uložit graf";
            ulozit.Click += ulozit_Click;

            MenuItem nacist = new MenuItem();
            nacist.Header = "Načíst graf";
            nacist.Click += nacist_Click;

            MenuItem konec = new MenuItem();
            konec.Header = "Konec";
            konec.Click += konec_Click;
            //-----------------------------------

            MenuItem algoritmy = new MenuItem();
            algoritmy.Header = "Algoritmy";

            MenuItem vBarveni = new MenuItem();
            vBarveni.Header = "Vrcholové barvení";
            vBarveni.Click += vBarveni_Click;

            MenuItem doHloubky = new MenuItem();
            doHloubky.Header = "Prohledavání do hloubky";
            doHloubky.Click += doHloubky_Click;

            MenuItem mKostra = new MenuItem();
            mKostra.Header = "Minimální kostra";
            mKostra.Click += mKostra_Click;

            //--------------------------------------
            MenuItem matice = new MenuItem();
            matice.Header = "Matice sousednosti a incidenční";
            matice.Click += matice_Click;

            MenuItem napoveda = new MenuItem();
            napoveda.Header = "Nápověda";
            napoveda.Click += napoveda_Click;

            MenuItem oHre = new MenuItem();
            oHre.Header = "O hře";


            menu.Items.Add(soubor);

            soubor.Items.Add(novyGraf);
            soubor.Items.Add(oddelovac);
            soubor.Items.Add(ulozit);
            soubor.Items.Add(nacist);
            //soubor.Items.Add(oddelovac);
            soubor.Items.Add(konec);
            //-----------------------------------

            menu.Items.Add(algoritmy);
            algoritmy.Items.Add(vBarveni);
            algoritmy.Items.Add(doHloubky);
            algoritmy.Items.Add(mKostra);
            //--------------------------------
            
            menu.Items.Add(matice);
            menu.Items.Add(napoveda);
            //menu.Items.Add(oHre);

        }

        void mKostra_Click(object sender, RoutedEventArgs e)
        {
            vynulovani();
            try
            {
                minimalniKostra(int.Parse(txbPocVrcholKostra.Text));
            }
            catch (FormatException)
            {
                MessageBox.Show("Byl zvolen chybný počáteční vrchol.", "Chyba - minimální kostra", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void napoveda_Click(object sender, RoutedEventArgs e)
        {
            OknoNapoveda oknoNapoveda = new OknoNapoveda();
            oknoNapoveda.Show();
        }

        void matice_Click(object sender, RoutedEventArgs e)
        {
            ulozeniMatice();
            OknoMatice oknoMatice = new OknoMatice();
            oknoMatice.Show();
        }

        void doHloubky_Click(object sender, RoutedEventArgs e)
        {
            vynulovani();
            try
            {
                prohledavaniDoHloubky(int.Parse(txbPocVrcholProhledavani.Text));

            }
            catch (FormatException)
            {
                MessageBox.Show("Byl zvolen chybný počáteční vrchol.", "Chyba - prohledávání do hloubky", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void vBarveni_Click(object sender, RoutedEventArgs e)
        {
            vynulovani();
            barveniVrcholu();
        }

        void nacist_Click(object sender, RoutedEventArgs e)
        {
            vycisteni();
            nacteni();
        }

        void ulozit_Click(object sender, RoutedEventArgs e)
        {
            ulozeni();
        }

        void novyGraf_Click(object sender, RoutedEventArgs e)
        {
            vycisteni();
        }

        void konec_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult odpoved = MessageBox.Show("Opravdu chcete ukončit aplikaci?", "Ukončení", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (odpoved == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region kontextove menu v seznamu vrcholu a hran
        private void kontextMenu()
        {
            MenuItem nHrana = new MenuItem();
            nHrana.Header = "Nová hrana";
            nHrana.Click += nHrana_Click;

            MenuItem smazat = new MenuItem();
            smazat.Header = "Smazat";
            smazat.Click += delete_Click;

            lbVrcholy.ContextMenu.Items.Add(nHrana);
            lbVrcholy.ContextMenu.Items.Add(smazat);
        }

        void nHrana_Click(object sender, RoutedEventArgs e)
        {
            pridaniHrany();
        }

        void delete_Click(object sender, RoutedEventArgs e)
        {
            odstraneni();
        }
        #endregion

        #region
        private void zmenaBarvyVrcholu(int cisloVrcholu, Brush barvaVrcholu)
        {
            znakVrchol[cisloVrcholu].Fill = barvaVrcholu;
            znakVrchol[cisloVrcholu].Name = barvaVrcholu.ToString().Substring(1,8);
        }

        private void zmenaBarvyHrany(int cisloHrany, Brush barvaHrany)
        {
            znakHrana[cisloHrany].Stroke = barvaHrany;
        }

        private void ulozeniMatice()
        {
            int pomVrcholy = zjisteniPoctuVrcholu();
            int pomHrany = zjisteniPoctuHran();

            using (StreamWriter sw = new StreamWriter(@"matice.txt"))
            {
                sw.WriteLine("Matice sousednosti");
                for (int i = 1; i < pomVrcholy; i++)
                {
                    for (int j = 1; j < pomVrcholy; j++)
                    {
                        sw.Write(maticeSousednosti[i, j] + " ");
                    }
                    sw.WriteLine();
                    //sw.WriteLine();
                }
            }

            using (StreamWriter sw = new StreamWriter(@"matice.txt", true))
            {
                sw.WriteLine("Matice incidencni");
                for (int i = 1; i < pomVrcholy; i++)
                {
                    for (int j = 1; j < pomHrany; j++)
                    {
                        sw.Write(maticeIncidence[i, j] + " ");
                    }
                    sw.WriteLine();
                    //sw.WriteLine();
                }
            }
            /*
            string s;

            using (StreamReader sr = new StreamReader(@"matice.txt", Encoding.Default))
            {
                s = sr.ReadToEnd();
                textBlock1.Text = s;
            }*/
        }

        private void txbBarva_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] info = new string[2];
            info = vyber();
            if (info[0] == "v")
            {
                zmenaBarvyVrcholu(int.Parse(info[1]), ziskejBarvu(txbBarva.Text));
            }
            else if (info[0] == "e")
            {
                zmenaBarvyHrany(int.Parse(info[1]), ziskejBarvu(txbBarva.Text));
            }
        }

        private void txbOhodnoceni_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] info = new string[2];
            info = vyber();
            int cislo;
            
            try
            {
                cislo = int.Parse(txbOhodnoceni.Text);
            }
            catch (FormatException)
            {
                return;
            }  
          
            if (info[0] == "v")
            {
                ohodnoceniVrcholu[int.Parse(info[1])] = cislo;
            }
            else if (info[0] == "e")
            {
                ohodnoceniHran[int.Parse(info[1])] = cislo;
            }
        }

        private int pocetSousedu(int cisloVrcholu)
        {
            int pom = zjisteniPoctuVrcholu();
            int pocet = 0;

            for (int i = 1; i < pom; i++)
            {
                if (maticeSousednosti[cisloVrcholu, i] == 1)
                {
                    pocet++;
                }
            }
            return pocet;
        }

        private static Random nahoda = new Random();

        public string barva { get; set; }

        private void nastaveniBarvy(int pocet)
        {

            string[] barvy = { "black", "red", "blue", "green", "yellow", "white", "purple", "orange", "pink", "gray", "brown", "blueviolet", "dodgerblue", "hotpink", "khaki", "lime", "navy", "salmon", "yellowgreen" };

            //barva = barvy[nahoda.Next(pocet)];//barvy.Length)];
            barva = barvy[pocet];
        }

        private int[] najdiSousedy(int cisloVrcholu)
        {
            int[] sousedi = new int[102];
            int pom = zjisteniPoctuVrcholu();
            int pocet =0;

            for (int i = 1; i < pom; i++)
            {
                if (maticeSousednosti[cisloVrcholu, i] == 1)
                {
                    sousedi[pocet]=i;
                    pocet++;
                }
            }
            return sousedi;
        }
        #endregion

        #region Vrcholove barveni
        /// <summary>
        /// algoritmus prochází postupně všechny vrcholy, vybraný vrchol obarví vybranou barvou, zkontroluje, 
        /// zda sousedi mají odlišnou barvu, pokud sousedi mají stejnou barvu zvýší se chromatické číslo 
        /// a znovu obarví vrchol jinou barvou, poté se zvona zkontroluje barva sousedů, jinak přejde na další vrchol
        /// 
        /// chromatické číslo pro každý vrchol je 1 a postupně se zvyšuje
        /// 
        /// Algoritmus:
        /// 1. vybere vrchol
        /// 2. obarví vrchol podle chromatického čísla
        /// 3. kontrola barvy sousedů
        ///     pokud je barva stejná, zvýší se chromatické číslo, pokračuje krokem 2
        ///     pokud je barva jiná, přechází se na další vrchol, pokračuje krokem 2
        /// </summary>
        private void barveniVrcholu()
        {
            lChCislo.Content = 0;//vynulování labelu
            int pocetVrcholu = zjisteniPoctuVrcholu();//určení počtu vrcholu
            int chromatickeCislo;
            int[] chrom = new int[102];//pomocné pole, pro každý vrchol zvlášť
            int[] sousedi = new int[102];//pole sousedu daného vrcholu
            int pocSousedu;//pomocna pocet sousedů

            //naplnění pole -1
            for (int i = 0; i < 100; i++)
            {
                chrom[i] = -1;
            }

            //začátek algpritmu
            //  1.   
            for (int i = 1; i < pocetVrcholu; i++)
            {
                chrom[i]++;//zvýšení chromatického číslo
                chromatickeCislo = chrom[i];

                if (int.Parse(lChCislo.Content.ToString()) <= chromatickeCislo) 
                {
                    int cch = chromatickeCislo + 1;
                    lChCislo.Content = cch;
                }

                //  2. 
                nastaveniBarvy(chromatickeCislo);//nastavení barvy, určuje se podle chromatického čísla
                zmenaBarvyVrcholu(i, ziskejBarvu(barva));//obarvení vrcholu nastavenou barvou

                sousedi = najdiSousedy(i);//pole sousedů daného vrcholu
                pocSousedu = pocetSousedu(i);//počet sousedů daného vrcholu

                //  3.
                for (int j = 0; j < pocSousedu; j++)//cyklus, který prochází sousedy
                {
                    if (znakVrchol[i].Name == znakVrchol[sousedi[j]].Name)//kontrola zda sousedí mají stenou barvu
                    {
                        i--;//snížení i aby se změnila barva stavajícího vrcholu
                        break;//přesuší se cyklus
                    }
                }
            }
            MessageBox.Show("Vrcholové barvení dokončeno.", "Hotovo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Prohledavání do hloubky
        private void prohledavaniDoHloubky(int cisloVrcholu)
        {
            try
            {
                znakVrchol[cisloVrcholu].Fill = Brushes.Green;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Byl zvolen chybný počáteční vrchol.", "Chyba - prohledávání do hloubky", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            znakVrchol[cisloVrcholu].Name = "navstiven";

            if (chbProDoHloubky.IsChecked==true)
            {
                MessageBox.Show("Přechod do vrcholu v" + cisloVrcholu + ".", "Prohedávání do hloubky");
            }

            int[] sousedi = new int[102];
            int pocSousedu;

            pocSousedu = pocetSousedu(cisloVrcholu);
            sousedi = najdiSousedy(cisloVrcholu);

            for (int j = 0; j < pocSousedu; j++)
            {
                //if (znakVrchol[sousedi[j]].Fill == ziskejBarvu("Black"))
                if(znakVrchol[sousedi[j]].Name == "vrchol")
                {
                    prohledavaniDoHloubky(sousedi[j]);
                }
            }
            znakVrchol[cisloVrcholu].Fill = Brushes.Red;
            znakVrchol[cisloVrcholu].Name = "opusten";
            
            if (chbProDoHloubky.IsChecked == true)
            {
                MessageBox.Show("Opuštění a označení vrcholu v" + cisloVrcholu + ".", "Prohedávání do hloubky");
            }
        }
        #endregion

        #region Minimální kostra
        private void minimalniKostra(int cisloVrcholu)
        {
            try
            {
                znakVrchol[cisloVrcholu].Fill = Brushes.White;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Byl zvolen chybný počáteční vrchol.", "Chyba - minimální kostra", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            znakVrchol[cisloVrcholu].Name = "navstiven";

            if (chbMinKostra.IsChecked == true)
            {
              //  MessageBox.Show("Přechod do vrcholu v" + cisloVrcholu + ".", "Minimální kostra");
            }

            int[] sousedi = new int[102];
            int pocSousedu;

            pocSousedu = pocetSousedu(cisloVrcholu);
            sousedi = najdiSousedy(cisloVrcholu);

            for (int j = 0; j < pocSousedu; j++)
            {
                //if (znakVrchol[sousedi[j]].Fill == Brushes.Black)
                if (znakVrchol[sousedi[j]].Name == "vrchol")
                {
                    minimalniKostra(sousedi[j]);
                }
                //else if (znakVrchol[sousedi[j]].Fill == Brushes.Blue)
                else if (znakVrchol[sousedi[j]].Name == "opusten")
                {
                    vymazaniHranyPodleVrcholu(cisloVrcholu, sousedi[j]);
                }
            }
            znakVrchol[cisloVrcholu].Fill = Brushes.Blue;
            znakVrchol[cisloVrcholu].Name = "opusten";

            if (chbMinKostra.IsChecked == true)
            {
              //  MessageBox.Show("Opuštění a označení vrcholu v" + cisloVrcholu + ".", "Minimální kostra");
            }
        }
        #endregion

        #region
        private void vymazaniHranyPodleVrcholu(int v1, int v2)
        {
            int h = najdiHranuPodleVrcholu(v1, v2);

            if (chbMinKostra.IsChecked == true)
            {
                MessageBox.Show("Smazana hrana h" + h + ".", "Minimální kostra");
            }
            smazaniHrany(h,true);

            string x1 = (Canvas.GetLeft(znakVrchol[v1]) + 5).ToString();
            string y1 = (Canvas.GetTop(znakVrchol[v1]) + 5).ToString();
            string x2 = (Canvas.GetLeft(znakVrchol[v2]) + 5).ToString();
            string y2 = (Canvas.GetTop(znakVrchol[v2]) + 5).ToString();

            //MessageBox.Show(string.Format("e{0}, {1}, {2}, {3}, {4}", h.ToString(), x1, y1, x2, y2));

            lbVrcholy.Items.Remove(string.Format("e{0}, {1}, {2}, {3}, {4}", h.ToString(), x1, y1, x2, y2));
            lbVrcholy.Items.Remove(string.Format("e{0}, {1}, {2}, {3}, {4}", h.ToString(), x2, y2, x1, y1));
        }

        private int najdiHranuPodleVrcholu(int vrchol1, int vrchol2)
        {
            int pomV = zjisteniPoctuVrcholu();
            int pomH = zjisteniPoctuHran();

            bool h1 = false;
            bool h2 = false;
            int h = 0;
            
                for (int j = 0; j < pomH; j++)
                {
                    if (maticeIncidence[vrchol1, j] == 1)
                    {
                        h1 = true;
                        h = j;
                    }

                    if (maticeIncidence[vrchol2, j] == 1)
                    {
                        h2 = true;
                        if (h1)
                        {
                        break;                           
                        }
                    }
                    else
                    {
                        h1 = false;
                        h2 = false;
                        h = 0;
                    }
                }

                if (h1 && h2)
                {
                    return h;
                }
                else
                {
                    return 0;
                }
        }

        private void vynulovani()
        {
            int pom = zjisteniPoctuVrcholu();
            for (int i = 1; i < pom; i++)
            {
                znakVrchol[i].Name = "vrchol";
            }
        }
        #endregion

        #region tlačítka
        private void btnVrBarveni_Click(object sender, RoutedEventArgs e)
        {
             
            vynulovani();
            /*Task taskA = new Task(() => MessageBox.Show("Probíhá barvení vrcholu.", "Prosím počtejte..."));
            taskA.Start();*/
            barveniVrcholu();
            //barveniProhledavaniDoHloubky(1, 0);
        }

        private void btnProDoHloubky_Click(object sender, RoutedEventArgs e)
        {
            vynulovani();
            try
            {
                prohledavaniDoHloubky(int.Parse(txbPocVrcholProhledavani.Text));
            }
            catch (FormatException)
            {
                MessageBox.Show("Byl zvolen chybný počáteční vrchol.", "Chyba - prohledávání do hloubky", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMinKostra_Click(object sender, RoutedEventArgs e)
        {
            vynulovani();
            try
            {
                minimalniKostra(int.Parse(txbPocVrcholKostra.Text));
            }
            catch (FormatException)
            {
                MessageBox.Show("Byl zvolen chybný počáteční vrchol.", "Chyba - minimální kostra", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}