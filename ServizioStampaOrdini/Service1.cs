//using DAL.Model;
//using DAL.Repository;
//using PrinterUtility;
//using ServizioStampaAdmin.FiscalPrinter;
//using ServizioStampaAdmin.Model;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Configuration;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.Drawing.Printing;
//using System.IO;
//using System.Linq;
//using System.ServiceProcess;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Timers;
//using System.Windows.Forms;
//using Vip.Printer;
//using Vip.Printer.Enums;
//using Timer = System.Timers.Timer;


//namespace ServizioStampaOrdini
//{
//    public partial class Service1 : ServiceBase
//    {
//        List<DettagliOrdine> lista = null;
//        List<DettaglioGestioneTavoli> listaPreOrder = null;
//        List<DettaglioGestioneTavoli> listaFiscale = null;
//        List<DettaglioGestioneTavoli> listaNonFiscale = null;
//        private System.Timers.Timer timer;
//        bool onlylocal = false;
//        public static string codscontrino = null;
//        public static string codazzeramento = null;
//        public static string pathDati = @"c:\DitronDati\Dati.txt";

//        public Service1()
//        {
//            InitializeComponent();
//            onlylocal = ConfigurationManager.AppSettings["OnlyLocal"] == "1";
//        }

//        protected override void OnStart(string[] args)
//        {
//            Thread t = new Thread(new ThreadStart(this.InitTimer));
//            t.Start();
//        }

//        private void InitTimer()
//        {
//            timer = new System.Timers.Timer();
//            //wire up the timer event 
//            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
//            //set timer interval   
//            //var timeInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalInSeconds"]); 
//            double timeInSeconds = 2.5;
//            timer.Interval = (timeInSeconds * 1000);
//            // timer.Interval is in milliseconds, so times above by 1000 
//            timer.Enabled = true;
//        }

//        private void OnTimer(object sender, ElapsedEventArgs e)
//        {
//            Stampa();
//        }

//        public void OnDebug()
//        {
//            Stampa();
//        }

//        protected override void OnStop()
//        {
//        }

//        //public void AvviaStampa()
//        //{
//        //    lista = new List<DettagliOrdine>();
//        //    listaPreOrder = new List<DettaglioGestioneTavoli>();
//        //    listaFiscale = new List<DettaglioGestioneTavoli>();

//        //    #region GetCodScontrino
//        //        GetScontrinoAzzeramento();
//        //    #endregion
//        //}


//        public  void Stampa()
//        {
//            if (onlylocal)
//            {
//                LocalProxy proxylocal = new LocalProxy();
//                #region Sezione Stampa Cucina Bar
//                lista = proxylocal.GetOrdiniDaStampare();
//                if (lista != null && lista.Count > 0)
//                {
//                    foreach (DettagliOrdine item in lista)
//                    {
//                        PrintReceipt(item);
//                        proxylocal.UpdateStatoOrdine(item.id_ordine);
//                    }
//                }
//                #endregion

//                #region Preconto
//                listaPreOrder = proxylocal.GetPreOrderDaStampare();
//                if (listaPreOrder != null && listaPreOrder.Count > 0)
//                {
//                    foreach (DettaglioGestioneTavoli item in listaPreOrder)
//                    {
//                        PrintPreOrder(item);
//                        proxylocal.UpdatePreOrder(item.id_gestione, item.id_tavolo);
//                    }
//                }
//                #endregion

//                #region Scontrino Non Fiscale
//                listaNonFiscale = proxylocal.GetCheckDaStampareNonFiscale();
//                if (listaNonFiscale != null && listaNonFiscale.Count > 0)
//                {
//                    foreach (DettaglioGestioneTavoli item in listaNonFiscale)
//                    {
//                        PrintCheckNonFiscale(item);
//                        proxylocal.UpdateCheckNonFiscale(item.id_gestione, item.id_tavolo);
//                    }
//                }
//                #endregion

//                #region Scontrino Fiscale
//                listaFiscale = proxylocal.GetCheckDaStampare();
//                if (listaFiscale != null && listaFiscale.Count > 0)
//                {
//                    foreach (DettaglioGestioneTavoli item in listaFiscale)
//                    {
//                        PrintCheck(item);
//                        proxylocal.UpdateCheck(item.id_gestione, item.id_tavolo);
//                    }
//                }
//                #endregion

//                #region Chiusura Giornaliera Fiscale
//                bool chiusura = proxylocal.GetChiusura();
//                if (chiusura)
//                {
//                    PrintChiusura();
//                    proxylocal.UpdateChiusura();
//                }
//                #endregion

//                #region Lettura Fiscale
//                bool Lettura = proxylocal.GetLettura();
//                if (Lettura)
//                {
//                    PrintLettura();
//                    proxylocal.UpdateLettura();
//                }
//                #endregion

//                #region Scontrino Regalo
//                bool regalo = proxylocal.GetRegalo();
//                if (regalo)
//                {
//                    PrintRegalo();
//                    proxylocal.UpdateRegalo();
//                }
//                #endregion

//                #region Annulla Scontrino
//                AnnulloScontrino Annulla = proxylocal.GetAnnullaScontrino();
//                if (Annulla != null && Annulla.DaAnnullare)
//                {
//                    PrintAnnullaScontrino(Annulla.codiceScontrino, Annulla.codiceAzzeramento, Annulla.DataScontrino.ToString("ddMMyy"));
//                    proxylocal.UpdateAnnullaScontrino(Annulla.id_annullo);
//                }
//                #endregion


//            }
//            else
//            {
//                Proxy proxy = new Proxy();
//                #region Sezione Stampa Cucina Bar
//                lista = proxy.GetOrdiniDaStampare();
//                if (lista != null && lista.Count > 0)
//                {
//                    foreach (DettagliOrdine item in lista)
//                    {
//                        PrintReceipt(item);
//                        proxy.UpdateStatoOrdine(item.id_ordine);
//                    }
//                }
//                #endregion

//                #region Preconto
//                listaPreOrder = proxy.GetPreOrderDaStampare();
//                if (listaPreOrder.Count > 0)
//                {
//                    foreach (DettaglioGestioneTavoli item in listaPreOrder)
//                    {
//                        PrintPreOrder(item);
//                        proxy.UpdatePreOrder(item.id_gestione, item.id_tavolo);
//                    }
//                }
//                #endregion

//                #region Scontrino Non Fiscale
//                listaNonFiscale = proxy.GetCheckDaStampareNonFiscale();
//                if (listaNonFiscale != null && listaNonFiscale.Count > 0)
//                {
//                    foreach (DettaglioGestioneTavoli item in listaNonFiscale)
//                    {
//                        PrintCheckNonFiscale(item);
//                        proxy.UpdateCheck(item.id_gestione, item.id_tavolo);
//                    }
//                }
//                #endregion

//                #region Scontrino Fiscale
//                listaFiscale = proxy.GetCheckDaStampare();
//                if (listaFiscale.Count > 0)
//                {
//                    foreach (DettaglioGestioneTavoli item in listaFiscale)
//                    {
//                        PrintCheck(item);
//                        proxy.UpdateCheck(item.id_gestione, item.id_tavolo);
//                    }
//                }
//                #endregion

//                #region Chiusura Giornaliera Fiscale
//                bool chiusura = proxy.GetChiusura();
//                if (chiusura)
//                {
//                    PrintChiusura();
//                    proxy.UpdateChiusura();
//                }
//                #endregion

//                #region Lettura Fiscale
//                bool Lettura = proxy.GetLettura();
//                if (Lettura)
//                {
//                    PrintLettura();
//                    proxy.UpdateLettura();
//                }
//                #endregion

//                #region Scontrino Regalo
//                bool regalo = proxy.GetRegalo();
//                if (regalo)
//                {
//                    PrintRegalo();
//                    proxy.UpdateRegalo();
//                }
//                #endregion

//                #region Annulla Scontrino
//                AnnulloScontrino Annulla = proxy.GetAnnullaScontrino();
//                if (Annulla != null &&  Annulla.DaAnnullare)
//                {
//                    PrintAnnullaScontrino(Annulla.codiceScontrino,Annulla.codiceAzzeramento, Annulla.DataScontrino.ToString("ddMMYY"));
//                    proxy.UpdateAnnullaScontrino(Annulla.id_annullo);
//                }
//                #endregion

//            }

//        }

//        private void PrintCheckNonFiscale(DettaglioGestioneTavoli item)
//        {
//            List<TabellaOrdiniTavolo> listaProdotti = new List<TabellaOrdiniTavolo>();
//            List<TabellaOrdiniTavolo> listaPerAliquote = new List<TabellaOrdiniTavolo>();
//            List<DettaglioStampanti> listaStampanti = new List<DettaglioStampanti>();

//            if (onlylocal)
//            {
//                LocalProxy OrderProxyLocal = new LocalProxy();
//                listaProdotti = OrderProxyLocal.GetDettaglioOrdineTavoloStampare(item.id_gestione.ToString(), item.id_tavolo.ToString());
//                listaStampanti = OrderProxyLocal.GetStampanti();
//            }
//            else
//            {
//                Proxy OrderProxy = new Proxy();
//                listaProdotti = OrderProxy.GetDettaglioOrdineTavoloStampare(item.id_gestione.ToString(), item.id_tavolo.ToString());
//                listaStampanti = OrderProxy.GetStampanti();
//            }

//            var printer = new Printer(listaStampanti.Where(x => x.tipo == 3).Select(x => x.nome).FirstOrDefault(), PrinterType.Epson);
//            Util u = new Util();
//            int numSpazi = Convert.ToInt32(ConfigurationManager.AppSettings["SpazioFronteSpizio"]);
//            for(int i = 0; i < numSpazi; i++)
//            {
//                printer.NewLine();
//            }
//            printer.AlignCenter();
//            printer.BoldMode(PrinterModeState.On);
//            printer.WriteLine(ConfigurationManager.AppSettings["NomeSocieta"]);
//            printer.WriteLine(ConfigurationManager.AppSettings["IndirizzoSocieta"]);
//            printer.WriteLine(ConfigurationManager.AppSettings["CittaSocieta"]);
//            printer.WriteLine(ConfigurationManager.AppSettings["PIvaSocieta"]);
//            printer.NewLine();
//            printer.WriteLine("DOCUMENTO NON FISCALE");
//            printer.BoldMode(PrinterModeState.Off);
//            printer.DoubleWidth2();
//            printer.NormalWidth();
//            printer.AlignLeft();
//            printer.NewLine();
//            printer.WriteLine(String.Format("{0,-28}{1,-10}{2}", "DESCRIZIONE", "IVA", "PREZZO"));
//            if(ConfigurationManager.AppSettings["modNegozio"] == "0")
//            {
//                printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", item.numero_persone + "X 0,00", "", ""));
//                printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", "COPERTO", "10%", "0,00"));
//            }
//            printer.ItalicMode(PrinterModeState.On);
//            foreach (TabellaOrdiniTavolo item1 in listaProdotti)
//            {
//                if (item1.QuantitaTotale > 1)
//                {
//                    printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", item1.QuantitaTotale + "X " + Convert.ToDouble(item1.prezzoUnitario).ToString("N2"), "", ""));
//                }
//                if (!String.IsNullOrEmpty(item1.NomeProdotto) && item1.NomeProdotto.Length > 30)
//                {
//                    List<string> lista = new List<string>();
//                    lista = u.SplitByLength(item1.NomeProdotto, 25);
//                    int i = 0;
//                    foreach (string item2 in lista)
//                    {
//                        if (i == 0)
//                        {
//                            printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", item2, item1.iva + "%", Convert.ToDouble(item1.QuantitaTotale * item1.prezzoUnitario).ToString("N2")));
//                        }
//                        else
//                        {
//                            printer.WriteLine(item2);
//                        }
//                        i++;

//                    }
//                }
//                else
//                {
//                    printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", item1.NomeProdotto, item1.iva + "%", Convert.ToDouble(item1.QuantitaTotale * item1.prezzoUnitario).ToString("N2")));

//                }
//            }
//            if (ConfigurationManager.AppSettings["modNegozio"] == "0")
//            {
//                printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", "Servizio", "", Convert.ToDouble(item.percentuale_servizio).ToString("N2")));
//            }
//            if (item.sconto != null && item.sconto != 0)
//            {
//                printer.NewLine();
//                printer.AlignRight();
//                printer.BoldMode(PrinterModeState.On);
//                printer.WriteLine(String.Format("{0,-40}{1}{2,-50}", "SCONTO", "", Convert.ToDouble(item.sconto).ToString("N2")));
//                printer.NewLine();
//                printer.AlignRight();
//                printer.BoldMode(PrinterModeState.On);
//                printer.WriteLine(String.Format("{0,-40}{1}{2,-50}", "TOTALE COMPLESSIVO ", "", Convert.ToDouble(item.totale - item.sconto).ToString("N2")));
//            }
//            else
//            {
//                printer.NewLine();
//                printer.AlignRight();
//                printer.BoldMode(PrinterModeState.On);
//                printer.WriteLine(String.Format("{0,-40}{1}{2,-50}", "TOTALE COMPLESSIVO ", "", Convert.ToDouble(item.totale).ToString("N2")));
//            }
//            printer.BoldMode(PrinterModeState.Off);
//            printer.NewLines(2);
//            printer.AlignCenter();
//            printer.WriteLine(DateTime.Now.ToString());
//            printer.NewLines(2);
//            if (ConfigurationManager.AppSettings["modNegozio"] == "0")
//            {
//                printer.AlignLeft();
//                printer.WriteLine("Tavolo n. " + item.numeroTavolo);
//                printer.NewLines(2);
//                printer.WriteLine("Importo a persona: " + Convert.ToDouble(item.totale / item.numero_persone).ToString("N2"));
//            }
//            printer.PartialPaperCut();
//            printer.PrintDocument();
//        }

//        private void PrintAnnullaScontrino(string codScontrino,string CodAzzeramento,string DataScontrino)
//        {
//            string command = String.Format(@"docannullo numsco={0},znumber={1},datasco={2},matr='{3}',automatico", codScontrino, CodAzzeramento, DataScontrino, ConfigurationManager.AppSettings["matricola"]);
//            string path = ConfigurationManager.AppSettings["pathScontrino"] + "/scontrino.txt";
//            string pathCopia = ConfigurationManager.AppSettings["pathCopiaScontrino"] + "/scontrino_Annullamento " + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".txt";
//            File.WriteAllText(path, command);
//            try
//            {
//                File.WriteAllText(pathCopia, command);
//            }
//            catch
//            {

//            }
//        }

//        public static void OnChanged(object source, FileSystemEventArgs e)
//        {
//            try
//            {
//                if (File.Exists(pathDati))
//                {
//                    string text = File.ReadAllText(pathDati);
//                    codscontrino = (Convert.ToInt32(text.Substring(0, 4)) + 1).ToString().PadLeft(4, '0'); ;
//                    codazzeramento = (Convert.ToInt32(text.Substring(4, 4)) + 1).ToString().PadLeft(4, '0');
//                    Service1 ser = new Service1();
//                    ser.Stampa();
//                }
//            }
//            catch (Exception ex)
//            {
//                string test = ex.Message;
//            }
//        }

//        private void GetScontrinoAzzeramento()
//        {
//            string command = @"clear" + "\n" +
//              @"info codice=10,file='c:\DitronDati\Dati.txt'" + "\n" +
//              @"wecfine" + "\n";
//            string path = ConfigurationManager.AppSettings["pathScontrino"] + "/scontrino.txt";
//            try
//            {
//                if (File.Exists(pathDati)) File.Delete(pathDati);
//                File.WriteAllText(path, command);
//            }
//            catch (Exception ex)
//            {
//                string test = ex.Message;
//            }
//        }

//        private void PrintChiusura()
//        {
//            string command = @"clear"  +"\n" +
//                @"resprn" +  "\n"  +
//                @"azzgio tipo=2 " + "\n" +
//                @"RTEJ INVIOAE" + "\n" +
//                @"wecfine" + "\n";
//            string path = ConfigurationManager.AppSettings["pathScontrino"] + "/scontrino.txt";
//            string pathCopia = ConfigurationManager.AppSettings["pathCopiaScontrino"] + "/scontrino_Chiusura_Cassa "  + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".txt";
//            File.WriteAllText(path, command);
//            try
//            {
//                File.WriteAllText(pathCopia, command);
//            }
//            catch
//            {

//            }
//        }

//        private void PrintLettura()
//        {
//            string command = @"clear" + "\n" +
//                @"report num=1" + "\n" +
//                @"wecfine" + "\n";
//            string path = ConfigurationManager.AppSettings["pathScontrino"] + "/scontrino.txt";
//            string pathCopia = ConfigurationManager.AppSettings["pathCopiaScontrino"] + "/scontrino_Lettura_Cassa " + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".txt";
//            File.WriteAllText(path, command);
//            try
//            {
//                File.WriteAllText(pathCopia, command);
//            }
//            catch
//            {

//            }
//        }

//        private void PrintRegalo()
//        {
//            string command = @"clear" + "\n" +
//                @"inp term=184" + "\n" +
//                @"wecfine" + "\n";
//            string path = ConfigurationManager.AppSettings["pathScontrino"] + "/scontrino.txt";
//            string pathCopia = ConfigurationManager.AppSettings["pathCopiaScontrino"] + "/scontrino_Regalo " + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".txt";
//            File.WriteAllText(path, command);
//            try
//            {
//                File.WriteAllText(pathCopia, command);
//            }
//            catch
//            {

//            }
//        }

//        private void PrintPreOrder(DettaglioGestioneTavoli itemPreOrder)
//        {
//            List<TabellaOrdiniTavolo> listaProdotti = new List<TabellaOrdiniTavolo>();
//            List<DettaglioStampanti> listaStampanti = new List<DettaglioStampanti>();

//            if (onlylocal)
//            {
//                LocalProxy OrderProxyLocal = new LocalProxy();
//                listaProdotti = OrderProxyLocal.GetDettaglioOrdineTavoloStampare(itemPreOrder.id_gestione.ToString(), itemPreOrder.id_tavolo.ToString());
//                listaStampanti = OrderProxyLocal.GetStampanti();
//            }
//            else
//            {
//                Proxy OrderProxy = new Proxy();
//                listaProdotti = OrderProxy.GetDettaglioOrdineTavoloStampare(itemPreOrder.id_gestione.ToString(), itemPreOrder.id_tavolo.ToString());
//                listaStampanti = OrderProxy.GetStampanti();
//            }

//            var printer = new Printer(listaStampanti.Where(x => x.tipo == 3 ).Select(x => x.nome).FirstOrDefault(), PrinterType.Epson);
//            Util u = new Util();
//            int numSpazi = Convert.ToInt32(ConfigurationManager.AppSettings["SpazioFronteSpizio"]);
//            for (int i = 0; i < numSpazi; i++)
//            {
//                printer.NewLine();
//            }
//            printer.AlignCenter();
//            printer.BoldMode(PrinterModeState.On);
//            printer.WriteLine("BARBEQOOL  SRL");
//            printer.WriteLine("VIA MERLIANI N.110" );
//            printer.WriteLine("NAPOLI");
//            printer.WriteLine("P.IVA 09200961218"); 
//            printer.NewLine();
//            printer.WriteLine("DOCUMENTO NON FISCALE");
//            printer.BoldMode(PrinterModeState.Off);
//            printer.DoubleWidth2();
//            printer.NormalWidth();
//            printer.AlignLeft();
//            printer.NewLine();
//            printer.WriteLine(String.Format("{0,-28}{1,-10}{2}", "DESCRIZIONE", "IVA","PREZZO"));
//            printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", itemPreOrder.numero_persone + "X 0,00", "", ""));
//            printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", "COPERTO", "10%", "0,00"));
//            printer.ItalicMode(PrinterModeState.On);
//            foreach (TabellaOrdiniTavolo item1 in listaProdotti)
//            {
//                    if(item1.QuantitaTotale > 1)
//                    {
//                        printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", item1.QuantitaTotale + "X " + Convert.ToDouble(item1.prezzoUnitario).ToString("N2"), "", ""));
//                    }
//                    if (!String.IsNullOrEmpty(item1.NomeProdotto) && item1.NomeProdotto.Length > 30)
//                    {
//                        List<string> lista = new List<string>();
//                        lista = u.SplitByLength(item1.NomeProdotto, 25);
//                        int i = 0;
//                        foreach (string item2 in lista)
//                        {
//                            if (i == 0)
//                            {
//                                printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", item2, item1.iva + "%", Convert.ToDouble(item1.QuantitaTotale* item1.prezzoUnitario).ToString("N2")));
//                            }
//                            else
//                            {
//                                printer.WriteLine(item2);
//                            }
//                            i++;

//                        }
//                    }
//                    else
//                    {
//                        printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", item1.NomeProdotto, item1.iva + "%", Convert.ToDouble(item1.QuantitaTotale* item1.prezzoUnitario).ToString("N2")));
                        
//                    }
//            }
//            printer.WriteLine(String.Format("{0,-30}{1,-8}{2,-5}", "Servizio","", Convert.ToDouble(itemPreOrder.percentuale_servizio).ToString("N2")));
//            if(itemPreOrder.sconto != null && itemPreOrder.sconto != 0)
//            {
//                printer.NewLine();
//                printer.AlignRight();
//                printer.BoldMode(PrinterModeState.On);
//                printer.WriteLine(String.Format("{0,-40}{1}{2,-50}", "SCONTO", "", Convert.ToDouble(itemPreOrder.sconto).ToString("N2")));
//                printer.NewLine();
//                printer.AlignRight();
//                printer.BoldMode(PrinterModeState.On);
//                printer.WriteLine(String.Format("{0,-40}{1}{2,-50}", "TOTALE COMPLESSIVO ", "", Convert.ToDouble(itemPreOrder.totale-itemPreOrder.sconto).ToString("N2")));
//            }
//            else
//            {
//                printer.NewLine();
//                printer.AlignRight();
//                printer.BoldMode(PrinterModeState.On);
//                printer.WriteLine(String.Format("{0,-40}{1}{2,-50}", "TOTALE COMPLESSIVO ", "", Convert.ToDouble(itemPreOrder.totale).ToString("N2")));
//            }
//            printer.BoldMode(PrinterModeState.Off);
//            printer.NewLines(2);
//            printer.AlignCenter();
//            printer.WriteLine(DateTime.Now.ToString());
//            printer.NewLines(2);
//            printer.AlignLeft();
//            printer.WriteLine("Tavolo n. " + itemPreOrder.numeroTavolo);
//            printer.NewLines(2);
//            printer.WriteLine("Importo a persona: " + Convert.ToDouble(itemPreOrder.totale/itemPreOrder.numero_persone).ToString("N2"));
//            printer.PartialPaperCut();
//            printer.PrintDocument();
//        }
        
//        public void PrintReceipt(DettagliOrdine itemOrdine)
//        {
//            List<DettaglioProdottiOrdine> listaprodotti = new List<DettaglioProdottiOrdine>();
//            List<DettaglioStampanti> listaStampanti = new List<DettaglioStampanti>();
//            List<DettaglioProdottiOrdine> listaBar = new List<DettaglioProdottiOrdine>();
//            List<DettaglioProdottiOrdine> listaRisto = new List<DettaglioProdottiOrdine>();
//            List<DettaglioAssociazioneStampantiCategorie> listaCatStampantiBar = new List<DettaglioAssociazioneStampantiCategorie>();
//            List<DettaglioAssociazioneStampantiCategorie> listaCatStampantiRisto = new List<DettaglioAssociazioneStampantiCategorie>();

//            if (onlylocal)
//            {
//                LocalProxy OrderProxyLocal = new LocalProxy();
//                listaStampanti = OrderProxyLocal.GetStampanti();
//                listaprodotti = OrderProxyLocal.GetprodottiOrdine(itemOrdine.id_ordine.ToString());
//                listaCatStampantiBar = OrderProxyLocal.GetCatStampanti(listaStampanti.Where(x => x.tipo == 1).Select(x => x.id_stampante).FirstOrDefault().ToString());
//                listaCatStampantiRisto = OrderProxyLocal.GetCatStampanti(listaStampanti.Where(x => x.tipo == 2).Select(x => x.id_stampante).FirstOrDefault().ToString());
                
//            }
//            else
//            {
//                Proxy OrderProxy = new Proxy();
//                listaStampanti = OrderProxy.GetStampanti();
//                listaprodotti = OrderProxy.GetprodottiOrdine(itemOrdine.id_ordine.ToString());
//                listaCatStampantiBar = OrderProxy.GetCatStampanti(listaStampanti.Where(x => x.tipo == 1).Select(x => x.id_stampante).FirstOrDefault().ToString());
//                listaCatStampantiRisto = OrderProxy.GetCatStampanti(listaStampanti.Where(x => x.tipo == 2).Select(x => x.id_stampante).FirstOrDefault().ToString());
//            }

//            var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
           
//            bool mostraTotale = System.Configuration.ConfigurationManager.AppSettings["mostraTotale"] == "1";
//            foreach (DettaglioProdottiOrdine item in listaprodotti)
//            {
//                if (!String.IsNullOrEmpty(listaCatStampantiBar.Where(x => x.categoria == item.id_categoria).Select(x => x.descrizione_categorie).FirstOrDefault())) listaBar.Add(item);
//                if (!String.IsNullOrEmpty(listaCatStampantiRisto.Where(x => x.categoria == item.id_categoria).Select(x => x.descrizione_categorie).FirstOrDefault())) listaRisto.Add(item);

//            }

//            ///////////////Stampa Bar///////////////////
//            #region Bar
//            if (listaBar != null && listaBar.Count() > 0)
//            {
//                var printer = new Printer(listaStampanti.Where(x => x.tipo == 1).Select(x => x.nome).FirstOrDefault(), PrinterType.Epson);
//                Util u = new Util();

//                printer.AlignCenter();
//                printer.DoubleWidth2();
//                switch (itemOrdine.id_tipoVendita)
//                {
//                    case 1:
//                    case 2:
//                        if (!string.IsNullOrEmpty(itemOrdine.NumeroTavolo))
//                        {
//                            printer.BoldMode(PrinterModeState.On);
//                            printer.WriteLine("COMANDA");
//                            printer.BoldMode(PrinterModeState.Off);
//                            printer.NewLine();
//                            printer.BoldMode(PrinterModeState.On);
//                            printer.WriteLine("Tavolo: " + itemOrdine.NumeroTavolo);
//                            printer.BoldMode(PrinterModeState.Off);
//                        }
//                        break;
//                    case 3:
//                        printer.BoldMode(PrinterModeState.On);
//                        printer.WriteLine("RITIRO");
//                        printer.BoldMode(PrinterModeState.Off);
//                        printer.NewLine();
//                        if (!string.IsNullOrEmpty(itemOrdine.Nominativo))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Nominativo: " + itemOrdine.Nominativo);
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.NumTelefono))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Telefono: " + itemOrdine.NumTelefono);
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.DataRitiro.ToString()))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Data Ritiro: " + ((DateTime)itemOrdine.DataRitiro).ToString("dd/MM/yyyy"));
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.FasciaOraria))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Orario: " + itemOrdine.FasciaOraria);
//                        }

//                        break;
//                    case 4:
//                        printer.BoldMode(PrinterModeState.On);
//                        printer.WriteLine("CONSEGNA");
//                        printer.BoldMode(PrinterModeState.Off);
//                        printer.NewLine();
//                        if (!string.IsNullOrEmpty(itemOrdine.Nominativo))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Nominativo: " + itemOrdine.Nominativo);
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.NumTelefono))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Telefono: " + itemOrdine.NumTelefono);
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.DataRitiro.ToString()))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Data Ritiro: " + ((DateTime)itemOrdine.DataRitiro).ToString("dd/MM/yyyy"));
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.FasciaOraria))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Orario: " + itemOrdine.FasciaOraria);
//                        }
//                        break;
//                    case 5:
//                        printer.BoldMode(PrinterModeState.On);
//                        printer.WriteLine("TAVOLO");
//                        printer.BoldMode(PrinterModeState.Off);
//                        printer.NewLine();
//                        if (!string.IsNullOrEmpty(itemOrdine.NumeroTavolo))
//                        {
//                            printer.BoldMode(PrinterModeState.On);
//                            printer.WriteLine("Tavolo: " + itemOrdine.NumeroTavolo);
//                            printer.BoldMode(PrinterModeState.Off);
//                        }
//                        break;
//                    case 6:
//                        printer.BoldMode(PrinterModeState.On);
//                        printer.WriteLine("OMBRELLONE");
//                        printer.BoldMode(PrinterModeState.Off);
//                        printer.NewLine();
//                        if (!string.IsNullOrEmpty(itemOrdine.Nominativo))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Nominativo: " + itemOrdine.Nominativo);
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.NumTelefono))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Telefono: " + itemOrdine.NumTelefono);
//                        }
//                        if (itemOrdine.Numero != 0)
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Ombrellone: " + itemOrdine.Numero);
//                        }
//                        if (itemOrdine.id_fila != 0)
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Fila: " + itemOrdine.id_fila);
//                        }
//                        if (!string.IsNullOrEmpty(itemOrdine.Lato))
//                        {
//                            printer.NewLine();
//                            printer.WriteLine("Lato: " + itemOrdine.Lato);
//                        }
//                        break;
//                }

//                printer.WriteLine("Data: " + itemOrdine.DataOra.ToString("dd/MM/yyyy HH:mm"));
//                printer.NormalWidth();
//                printer.AlignLeft();
//                printer.NewLine();
//                printer.WriteLine(String.Format("{0,-35}{1}", "Prodotto", "Quantità"));
//                printer.NewLine();
//                printer.ItalicMode(PrinterModeState.On);
//                foreach (DettaglioProdottiOrdine item1 in listaprodotti)
//                {
//                    if (!String.IsNullOrEmpty(listaCatStampantiBar.Where(x => x.categoria == item1.id_categoria).Select(x => x.descrizione_categorie).FirstOrDefault()))
//                    {
//                        if (!String.IsNullOrEmpty(item1.NomeProdotto) && item1.NomeProdotto.Length > 30)
//                        {
//                            List<string> lista = new List<string>();
//                            lista = u.SplitByLength(item1.NomeProdotto, 30);
//                            int i = 0;
//                            foreach (string item2 in lista)
//                            {
//                                if (i == 0)
//                                {
//                                    printer.WriteLine(String.Format("{0,-38}{1}", item2, item1.Quantita));
//                                }
//                                else
//                                {
//                                    printer.WriteLine(item2);
//                                }
//                                i++;

//                            }
//                            if (!String.IsNullOrEmpty(item1.Note)) printer.WriteLine(String.Format("{0}", item1.Note));
//                        }
//                        else
//                        {
//                            printer.WriteLine(String.Format("{0,-38}{1}", item1.NomeProdotto, item1.Quantita));
//                            if (!String.IsNullOrEmpty(item1.Note)) printer.WriteLine(String.Format("{0}", item1.Note));
//                        }
//                        printer.NewLine();

//                    }
                   
                    

//                }
//                printer.NewLine();
//                printer.WriteLine("Metodo Pagamento: " + itemOrdine.Tipo_Pagamento);
//                printer.NewLine();
//                if (mostraTotale)
//                {
//                    printer.AlignRight();
//                    printer.WriteLine("Totale: " + itemOrdine.Totale + " euros");
//                    if (itemOrdine.Note != null && !String.IsNullOrEmpty(itemOrdine.Note)) printer.WriteLine("Note: " + itemOrdine.Note.Replace("€", " euros"));
//                    printer.NewLines(5);
//                }
              
//                printer.PartialPaperCut();
//                printer.PrintDocument();

//            }
//            #endregion

//            ///////Stampa Ristorante///////////////////
//            #region Cucina
//            if (listaRisto != null && listaRisto.Count() > 0)
//            {
//                var printer = new Printer(listaStampanti.Where(x => x.tipo == 2).Select(x => x.nome).FirstOrDefault(), PrinterType.Epson);
//                Util u = new Util();
//                printer.AlignCenter();
//                printer.DoubleWidth2();
//                printer.BoldMode(PrinterModeState.On);
//                printer.WriteLine("Tavolo: " + itemOrdine.NumeroTavolo);
//                printer.NormalWidth();
//                printer.NewLine();
//                printer.NewLine();
//                printer.WriteLine("Coperti: " + itemOrdine.NumeroPersone);
//                printer.WriteLine("Data: " + itemOrdine.DataOra);
//                printer.AlignLeft();
//                printer.NewLine();
//                printer.DoubleWidth2();
//                printer.WriteLine(String.Format("{0}      {1}", "Prodotto", "Quantità"));
//                printer.NewLine();
//                //printer.ItalicMode(PrinterModeState.On);
//                printer.WriteLine(String.Format("******{0}******", "1° Uscita"));
//                printer.NewLine();
//                bool print = true;
//                int ordineattuale = 1;            
//                foreach (DettaglioProdottiOrdine item1 in listaprodotti.OrderBy(x => x.OrdineUscita))
//                {
//                    if (ordineattuale == item1.OrdineUscita)
//                    {
//                        print = false;
//                    }
//                    else
//                    {
//                        print = true;
//                    }
//                    if (item1.OrdineUscita > 1 && print)
//                    {
//                        printer.WriteLine(String.Format("******{0}******", item1.OrdineUscita + "° Uscita"));
//                        printer.NewLine();
//                        print = false;
//                        ordineattuale = (int)item1.OrdineUscita;
//                    }

//                    if (!String.IsNullOrEmpty(listaCatStampantiRisto.Where(x => x.categoria == item1.id_categoria).Select(x => x.descrizione_categorie).FirstOrDefault()))
//                    {
//                        if (!String.IsNullOrEmpty(item1.NomeProdotto) && item1.NomeProdotto.Length > 25)
//                        {
//                            List<string> lista = new List<string>();
//                            lista = u.SplitByLength(item1.NomeProdotto, 25);
//                            int i = 0;
//                            foreach (string item2 in lista)
//                            {
//                                if (i == 0)
//                                {
//                                    printer.WriteLine(String.Format("{0,-23}{1}", item2, item1.Quantita));
                                    
//                                }
//                                else
//                                {
//                                    printer.WriteLine(item2);
//                                }
//                                i++;

//                            }
//                            #region Special
//                            if (item1.IsSpecial != null && (bool)item1.IsSpecial)
//                            {
//                                if (item1.Ingredienti.Length > 25)
//                                {
//                                    List<string> listaIngr = new List<string>();
//                                    listaIngr = u.SplitByLength(item1.Ingredienti, 25);
//                                    int ing = 0;
//                                    foreach (string item3 in listaIngr)
//                                    {
//                                        if (ing == 0)
//                                        {
//                                            printer.WriteLine(String.Format("{0,-23}", item3));
//                                        }
//                                        else
//                                        {
//                                            printer.WriteLine(item3);
//                                        }
//                                        ing++;
//                                    }
//                                }
//                                else
//                                {
//                                    printer.WriteLine(String.Format("{0}", item1.Ingredienti));
//                                }
//                            }
//                            #endregion
//                            if (!String.IsNullOrEmpty(item1.Note)) printer.WriteLine(String.Format("{0}", item1.Note));
//                            if (!String.IsNullOrEmpty(item1.LivelloCottura)) printer.WriteLine(String.Format("{0}", item1.LivelloCottura));
//                        }
//                        else
//                        {
//                            printer.WriteLine(String.Format("{0,-23}{1}", item1.NomeProdotto, item1.Quantita));
//                            #region Special
//                            if (item1.IsSpecial != null && (bool)item1.IsSpecial)
//                            {
//                                if (item1.Ingredienti.Length > 25)
//                                {
//                                    List<string> listaIngr2 = new List<string>();
//                                    listaIngr2 = u.SplitByLength(item1.Ingredienti, 25);
//                                    int ing2 = 0;
//                                    foreach (string item4 in listaIngr2)
//                                    {
//                                        if (ing2 == 0)
//                                        {
//                                            printer.WriteLine(String.Format("{0,-23}", item4));
//                                        }
//                                        else
//                                        {
//                                            printer.WriteLine(item4);
//                                        }
//                                        ing2++;
//                                    }
//                                }
//                                else
//                                {
//                                    printer.WriteLine(String.Format("{0}", item1.Ingredienti));
//                                }
//                            }
//                            #endregion
//                            if (!String.IsNullOrEmpty(item1.Note)) printer.WriteLine(String.Format("{0}", item1.Note));
//                            if (!String.IsNullOrEmpty(item1.LivelloCottura)) printer.WriteLine(String.Format("{0}", item1.LivelloCottura));
//                        }
                       
//                        printer.NewLine();

//                    }
                    
                    

//                }
//                printer.NewLine();
//                if (mostraTotale)
//                {
//                    printer.AlignRight();
//                    printer.WriteLine("Totale: " + itemOrdine.Totale + " euros");
//                    if (itemOrdine.Note != null && !String.IsNullOrEmpty(itemOrdine.Note)) printer.WriteLine("Note: " + itemOrdine.Note.Replace("€", " euros"));
//                    printer.NewLines(5);
//                }
//                printer.BoldMode(PrinterModeState.Off);
//                printer.PartialPaperCut();
//                printer.PrintDocument();
//            }
//            #endregion
//        }

//        private void PrintCheck(DettaglioGestioneTavoli itemPreOrder)
//        {

//            List<TabellaOrdiniTavolo> listaProdotti = new List<TabellaOrdiniTavolo>();
//            List<TabellaOrdiniTavolo> listaPerAliquote = new List<TabellaOrdiniTavolo>();

//            if (onlylocal)
//            {
//                LocalProxy OrderProxyLocal = new LocalProxy();
//                listaProdotti = OrderProxyLocal.GetDettaglioOrdineTavoloStampare(itemPreOrder.id_gestione.ToString(), itemPreOrder.id_tavolo.ToString());
//            }
//            else
//            {
//                Proxy OrderProxy = new Proxy();
//                listaProdotti = OrderProxy.GetDettaglioOrdineTavoloStampare(itemPreOrder.id_gestione.ToString(), itemPreOrder.id_tavolo.ToString());
//            }

//            if(listaProdotti.Where(x => x.id_prodotto == -1).Count()>0)
//            {
//                PrintConfig config = new PrintConfig();
//                List<PosItem> posItems = new List<PosItem>();
//                double totale = (double)listaProdotti.Where(x => x.id_prodotto != -1).Sum(x => (x.prezzoUnitario * x.QuantitaTotale));
//                double valueticket = listaProdotti.Where(x => x.id_prodotto == -1).Sum(x => Math.Abs((double)x.prezzoUnitario));
//                double valuePay = (double)(listaProdotti.Where(x => x.id_prodotto != -1).Sum(x => (x.prezzoUnitario * x.QuantitaTotale)) - valueticket);
//                config.IdGestione = itemPreOrder.id_gestione.ToString();
//                config.IdTavolo = itemPreOrder.id_tavolo.ToString();
//                //Controllo Generaione Credico con QrCode
//                if (valueticket > totale)
//                {
//                    //trasforto la lista dei prodotti nei due prodotti base per le due aliquote
//                    TabellaOrdiniTavolo ord = new TabellaOrdiniTavolo();
//                    ord.NomeProdotto = "Prodotto Generico 10%";
//                    ord.iva = 10;
//                    ord.prezzoUnitario = totale;
//                    ord.QuantitaTotale = 1;
//                    listaPerAliquote.Add(ord);
//                    TabellaOrdiniTavolo ord2 = new TabellaOrdiniTavolo();
//                    ord2.NomeProdotto = "Prodotto Generico 4%";
//                    ord2.iva = 4;
//                    ord2.prezzoUnitario = 0;
//                    ord2.QuantitaTotale = 1;
//                    listaPerAliquote.Add(ord2);

//                    //Generazione Codice avanzo Ticket
//                    config.ValoreBarCode = (valueticket - totale).ToString("N2").Replace(",", ".");
//                    config.CodeBarCode = Util.GeneraCodiceRandom();
//                    Codici cod = new   Codici();
//                    cod.Valore = (valueticket - totale) ;
//                    cod.Codice = config.CodeBarCode;
//                    cod.Utilizzato = false;
//                    LocalProxy.SaveCode(cod);
//                }
//                else
//                {
//                    //trasforto la lista dei prodotti nei due prodotti base per le due aliquote
//                    TabellaOrdiniTavolo ord = new TabellaOrdiniTavolo();
//                    ord.NomeProdotto = "Prodotto Generico 10%";
//                    ord.iva = 10;
//                    ord.prezzoUnitario = valueticket;
//                    ord.QuantitaTotale = 1;
//                    listaPerAliquote.Add(ord);
//                    TabellaOrdiniTavolo ord2 = new TabellaOrdiniTavolo();
//                    ord2.NomeProdotto = "Prodotto Generico 4%";
//                    ord2.iva = 4;
//                    ord2.prezzoUnitario = valuePay;
//                    ord2.QuantitaTotale = 1;
//                    listaPerAliquote.Add(ord2);
//                }

//                // aggiungo la lista dei prodotti base allo scontrino
//                foreach (TabellaOrdiniTavolo a in listaPerAliquote)
//                {
//                    if (a.Sconto == null) a.Sconto = 0;
//                    posItems.Add(new PosItem
//                    {
//                        tax = (int)a.iva,
//                        name = a.NomeProdotto,
//                        price = (double)a.prezzoUnitario,
//                        Sconto = (double)a.Sconto,
//                        qty = (int)a.QuantitaTotale,
//                    });
//                }

//                config.items = posItems;

//                //configurazione del venditore
//                SellerInfo seller = new SellerInfo();
//                seller.name = "";//azienda.intestazione;
//                seller.p_iva = "";
//                config.sellerInfo = seller;

//                switch (itemPreOrder.metodo_pagamento)
//                {
//                    case "Contanti":
//                        config.payment = PrintConfig.payment_type.CASH;
//                        break;
//                    case "Carte":
//                        config.payment = PrintConfig.payment_type.CARD;
//                        break;
//                    case "Bonifico":
//                        config.payment = PrintConfig.payment_type.ASSIGN;
//                        break;
//                    case "Assegno":
//                        config.payment = PrintConfig.payment_type.ASSIGN;
//                        break;
//                    default:
//                        config.payment = PrintConfig.payment_type.CASH;
//                        break;
//                }

//                //////////QRCODE ANNULLAMENTO///////////////////
//                if(!String.IsNullOrEmpty(codscontrino) && !String.IsNullOrEmpty(codazzeramento))
//                {
//                    config.ValoreBarCode = codscontrino + ";" + codazzeramento + ";" + itemPreOrder.id_gestione.ToString() + ";" + itemPreOrder.id_tavolo.ToString();
//                }
//                ////////////////////////////////////

//                CreateBodiesMixed body_req = new CreateBodiesMixed(config);
//                body_req.ListaProdotti = listaProdotti;
//                string command = body_req.createBodyMixed();
//                string path = ConfigurationManager.AppSettings["pathScontrino"] + "/scontrino.txt";
//                string pathCopia = ConfigurationManager.AppSettings["pathCopiaScontrino"] + "/scontrino_Tav " + itemPreOrder.numeroTavolo + "_Tot " + itemPreOrder.totale.Value.ToString("N2") + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".txt";
//                File.WriteAllText(path, command);
//                try
//                {
//                    File.WriteAllText(pathCopia, command);
//                }
//                catch
//                {

//                }
//            }
//            else
//            {
//                PrintConfig config = new PrintConfig();
//                List<PosItem> posItems = new List<PosItem>();
//                // aggiungo  a lista degli item su scontrino
//                config.IdGestione = itemPreOrder.id_gestione.ToString();
//                config.IdTavolo = itemPreOrder.id_tavolo.ToString();

//                foreach (TabellaOrdiniTavolo a in listaProdotti)
//                {
//                    if (a.Sconto == null) a.Sconto = 0;
//                    posItems.Add(new PosItem
//                    {
//                        tax = (int)a.iva,
//                        name = a.NomeProdotto,
//                        price = (double)a.prezzoUnitario,
//                        Sconto = (double)a.Sconto,
//                        qty = (int)a.QuantitaTotale,
//                    });
//                }

//                if(itemPreOrder.percentuale_servizio != null && itemPreOrder.percentuale_servizio > 0)
//                {
//                    posItems.Add(new PosItem
//                    {
//                        tax = 10,
//                        name = "Servizio",
//                        price = (double)itemPreOrder.percentuale_servizio,
//                        Sconto = 0,
//                        qty = 1,
//                    });
//                }

//                config.items = posItems;
//                config.client = new Client
//                {
//                    fiscal_code = "",
//                    name = ""
//                };
//                //configurazione del venditore
//                SellerInfo seller = new SellerInfo();
//                seller.name = "";//azienda.intestazione;
//                seller.p_iva = "";
//                if (itemPreOrder.sconto != null && itemPreOrder.sconto != 0) seller.sconto = ((double)itemPreOrder.sconto).ToString("N2").Replace(",", ".");
//                config.Courtesy_message = "";//azienda.intestazione;

//                switch (itemPreOrder.metodo_pagamento)
//                {
//                    case "Contanti":
//                        config.payment = PrintConfig.payment_type.CASH;
//                        break;
//                    case "Carte":
//                        config.payment = PrintConfig.payment_type.CARD;
//                        break;
//                    case "Bonifico":
//                        config.payment = PrintConfig.payment_type.ASSIGN;
//                        break;
//                    case "Assegno":
//                        config.payment = PrintConfig.payment_type.ASSIGN;
//                        break;
//                    default:
//                        config.payment = PrintConfig.payment_type.CASH;
//                        break;
//                }

//                //////////QRCODE ANNULLAMENTO///////////////////
//                if (!String.IsNullOrEmpty(codscontrino) && !String.IsNullOrEmpty(codazzeramento))
//                {
//                    config.ValoreBarCode = codscontrino + ";" + codazzeramento + ";" + itemPreOrder.id_gestione.ToString() + ";" + itemPreOrder.id_tavolo.ToString();
//                }
//                ////////////////////////////////////

//                config.sellerInfo = seller;
//                CreateBodies body_req = new CreateBodies(config);
//                string command = body_req.createBody();
//                string path = ConfigurationManager.AppSettings["pathScontrino"] + "/scontrino.txt";
//                string pathCopia = ConfigurationManager.AppSettings["pathCopiaScontrino"] + "/scontrino_Tav " + itemPreOrder.numeroTavolo + "_Tot " + itemPreOrder.totale.Value.ToString("N2") + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".txt";
//                File.WriteAllText(path, command);
//                try
//                {
//                    File.WriteAllText(pathCopia, command);
//                }
//                catch
//                {

//                }
//            }
            
//        }   

//    }
//}
