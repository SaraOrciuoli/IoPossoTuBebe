
using DAL.Model;
using System;
using System.Collections.Generic;
using static ServizioStampaAdmin.FiscalPrinter.Barcode;

namespace ServizioStampaAdmin.FiscalPrinter
{
    internal class CreateBodiesMixed

    {
        PrintConfig config;
        public string reponse = "";
        string items = "";
        string other = "";
        string piva = "";
        string pay = "";
        string BarCode = "";

        const string item = @"vend REP={0}, PRE={1}, DES='{2}', qty={3}" + "\n";
        const string p_iva = @"INP TERM=61" + "\n" + @"INP ALFA='{0}',TERM=49" + "\n";
        const string c_f = @"INP TERM=61" + "\n" + @"INP ALFA='[[c_f]]',TERM=49" + "\n";
        const string Sconto = @"SCONTO VAL=[[sconto]], SUBT" + "\n";

        const string alleg_on = @"alleg on" + "\n";
        const string bar_code = "alleg riga='{0}', barcode=158, scala=1, allineamento=1" + "\n" + "alleg fine" + "\n";

        const string courtesy1 = @"CORT R1='Dati Gestionale: [[MSG1]]';" + "\n";
        const string courtesy2 = @"CORT R2='[[MSG2]]'	;messaggio di cortesia" + "\n";
        const string MessageOrder = @"PRMSG LINE='[[MSG]]'" + "\n";

        const string body =
            @"CLEAR 	  ;Preme il tasto C" + "\n" +
            @"RESPRN	  ;Annulla eventuali transazioni aperte" + "\n" +
            @"CHIAVE REG  ;Conferma che la cassa si trovi in assetto REGistrazione" + "\n" +
            @"INP TERM=167" + "\n" +
            @"INP ALFA = '', TERM = 145" + "\n" +
            @"{0}" + "\n" +
            @"{1}" + "\n" +
            @"{2}" + "\n" +
            @"{3}" + "\n" +
            @"{4}" + "\n" +
            @"wecfine" + "\n" +
            @"";

        const string Code = @"alleg riga = '     Credito da utilizzare per il       '" + "\n" +
                @"alleg riga = '         prossimo  acquisto             '" + "\n" +
                @"alleg riga = '----------    Euro [[valore]]     ------------'" + "\n" +
                @"alleg riga = '                                              '" + "\n" +
                @"alleg riga = '[[codice]]', barcode = 158, scala = 1, allineamento = 1" + "\n" +
                @"alleg fine";





        public List<TabellaOrdiniTavolo> ListaProdotti { get; internal set; }

        internal CreateBodiesMixed(PrintConfig conf)
        {

            config = conf;
        }

        public string createBodyMixed()
        {


            reponse = body;

            foreach (var i in config.items)
            {
                AddItemMixed(i);
            }
          
            if (config.sellerInfo.p_iva != null || !string.IsNullOrEmpty(config.sellerInfo.p_iva))
            {
                AddpivaMixed(config.sellerInfo);
            }
            if (config.ValoreBarCode != null)
            {
                AddBarCodeMixed(config.ValoreBarCode, config.CodeBarCode);
            }
            if (!string.IsNullOrEmpty(config.ValoreBarCode)) pay += alleg_on;
            foreach (var i in config.items)
            {
                if(i.tax == 10)
                {
                    Payment_TypeMixed(PrintConfig.payment_type.ASSIGN,i.price.ToString("N2"));
                }
                else
                {
                    Payment_TypeMixed(config.payment,i.price.ToString("N2"));
                }
            }

            foreach (TabellaOrdiniTavolo item in ListaProdotti)
            {
                if(!string.IsNullOrEmpty(item.NomeProdotto))AddDetailProdotti((item.QuantitaTotale.ToString() + " " + item.NomeProdotto));
            }

            if (config.IdGestione != null && config.IdTavolo != null)
            {
                AddFirstCourtesyMixed(config.IdTavolo + " - " + config.IdGestione);
            }
            


            return CreateSenderMixed();
        }



        private void AddItemMixed(PosItem PosItem)
        {
            //@"vend rep={0}, pre={1}, des='{2}', qty={3}" + "\n";
            switch (PosItem.tax)
            {
                case 22:
                    items += item.Replace("{0}", "1").Replace("{1}", PosItem.price.ToString().Replace(",",".")).Replace("{2}", PosItem.name).Replace("{3}", PosItem.qty.ToString());

                    break;
                case 10:
                    items += item.Replace("{0}", "2").Replace("{1}", PosItem.price.ToString().Replace(",", ".")).Replace("{2}", PosItem.name).Replace("{3}", PosItem.qty.ToString());

                    break;
                case 8:
                    items += item.Replace("{0}", "3").Replace("{1}", PosItem.price.ToString().Replace(",", ".")).Replace("{2}", PosItem.name).Replace("{3}", PosItem.qty.ToString());

                    break;
                case 4:
                    items += item.Replace("{0}", "3").Replace("{1}", PosItem.price.ToString().Replace(",", ".")).Replace("{2}", PosItem.name).Replace("{3}", PosItem.qty.ToString());

                    break;
                case 0:
                    items += item.Replace("{0}", "5").Replace("{1}", PosItem.price.ToString().Replace(",", ".")).Replace("{2}", PosItem.name).Replace("{3}", PosItem.qty.ToString());

                    break;
                default:
                    items += item.Replace("{0}", "5").Replace("{1}", PosItem.price.ToString().Replace(",", ".")).Replace("{2}", PosItem.name).Replace("{3}", PosItem.qty.ToString());

                    break;
            }
        }
        private void AddpivaMixed(SellerInfo info)
        {
            piva += p_iva.Replace("{0}", info.p_iva);
        }
        private void AddCfMixed(Client info)
        {
            piva += c_f.Replace("[[c_f]]", info.fiscal_code);
        }
        private void AddBarCodeMixed(string valore,string codice)
        {
            BarCode += Code.Replace("[[valore]]", valore).Replace("[[codice]]", codice);
        }


        private void AddScontoMixed(string sconto)
        {
            other += Sconto.Replace("[[sconto]]", sconto);


        }
        private void AddFirstCourtesyMixed(string curt1)
        {
            other += courtesy1.Replace("[[MSG1]]", curt1);


        }
        private void AddSecondCourtesyMixed(string curt1)
        {
            other += courtesy2.Replace("[[MSG2]]", curt1);


        }
        private void AddDetailProdotti(string msg)
        {
            other += MessageOrder.Replace("[[MSG]]", msg);
        }

        private string CreateSenderMixed()
        {
            return reponse
                .Replace("{0}", piva)
                .Replace("{1}", items.ToString())
                .Replace("{2}", other.ToString())
                .Replace("{3}", pay.ToString())
                .Replace("{4}", BarCode.ToString());
        }

        private void Payment_TypeMixed(PrintConfig.payment_type payment, string imp)
        {
            
            switch (payment)
            {
                case PrintConfig.payment_type.CASH:
                    if(string.IsNullOrEmpty(pay))
                    {
                        pay += "CHIU T=1, imp=" + imp.Replace(",",".");
                    }
                    else
                    {
                        pay += "\n" + "CHIU T=1, imp=" + imp.Replace(",", ".");
                    }
                    break;
                case PrintConfig.payment_type.ASSIGN:
                    if (string.IsNullOrEmpty(pay))
                    {
                        pay += "CHIU T=2, imp=" + imp.Replace(",", ".");
                    }
                    else
                    {
                        pay += "\n" + "CHIU T=2, imp=" + imp.Replace(",", ".");
                    }
                    break;

                case PrintConfig.payment_type.CARD:
                    if (string.IsNullOrEmpty(pay))
                    {
                        pay += "CHIU T=6, imp=" + imp.Replace(",", ".");
                    }
                    else
                    {
                        pay += "\n" + "CHIU T=6, imp=" + imp.Replace(",", ".");
                    }
                    break;

                default:
                    if (string.IsNullOrEmpty(pay))
                    {
                        pay += "CHIU T=1, imp=" + imp.Replace(",", ".");
                    }
                    else
                    {
                        pay += "\n" + "CHIU T=1, imp=" + imp.Replace(",", ".");
                    }
                    break;

            }


        }


    }
}