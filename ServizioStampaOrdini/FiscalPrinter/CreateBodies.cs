
using System;
using static ServizioStampaAdmin.FiscalPrinter.Barcode;

namespace ServizioStampaAdmin.FiscalPrinter
{
    internal class CreateBodies

    {
        PrintConfig config;
        public string reponse = "";
        string items = "";
        string other = "";
        string piva = "";
        string pay = "";
        const string item = @"vend REP={0}, PRE={1}, DES='{2}', qty={3}" + "\n";
        const string scontoSingolo = @"Sconto val={0}" + "\n";
        const string p_iva = @"INP TERM=61" + "\n" + @"INP ALFA='{0}',TERM=49" + "\n";
        const string c_f = @"INP TERM=61" + "\n" + @"INP ALFA='[[c_f]]',TERM=49" + "\n";
        const string Sconto = @"SCONTO VAL=[[sconto]], SUBT" + "\n";

        const string alleg_on = @"alleg on" + "\n";
        const string bar_code = "alleg riga='{0}', barcode=158, scala=1, allineamento=1" + "\n" + "alleg fine" + "\n";
        const string courtesy1 = @"CORT R1='Dati Gestionale: [[MSG1]]';" + "\n";
        const string courtesy2 = @"CORT R2='[[MSG2]]'	;messaggio di cortesia" + "\n";

        const string body =
            @"CLEAR 	  ;Preme il tasto C" + "\n" +
            @"RESPRN	  ;Annulla eventuali transazioni aperte" + "\n" +
            @"CHIAVE REG  ;Conferma che la cassa si trovi in assetto REGistrazione" + "\n" +
            @"{0}" + "\n" +
            @"{1}" + "\n" +
            @"{2}" + "\n" +
            @"{3}" +"\n"+
            @"wecfine" + "\n" +
            @"";


        internal CreateBodies(PrintConfig conf)
        {

            config = conf;
        }

        public string createBody()
        {


            reponse = body;

            foreach (var i in config.items)
            {
                AddItem(i);
            }
          
            if (config.sellerInfo.p_iva != null || !string.IsNullOrEmpty(config.sellerInfo.p_iva))
            {
                Addpiva(config.sellerInfo);
            }
            if (config.client.fiscal_code != null || !string.IsNullOrEmpty(config.client.fiscal_code))
            {
                AddCf(config.client);
            }
            
            if (config.Courtesy_message != null || !string.IsNullOrEmpty(config.Courtesy_message))
            {
                AddSecondCourtesy(config.Courtesy_message);
            }
            if (config.sellerInfo.sconto != null && !string.IsNullOrEmpty(config.sellerInfo.sconto) && Convert.ToDouble(config.sellerInfo.sconto) != 0)
            {
                AddSconto(config.sellerInfo.sconto);
            }
            if (config.Barcode != null)
            {
                AddBarCode(config.Barcode);
            }
            if (config.IdGestione != null && config.IdTavolo != null)
            {
                AddFirstCourtesy(config.IdTavolo + " - " + config.IdGestione);
            }
            if (config.ValoreBarCode != null)
            {
                AddBarCodeMixed(config.ValoreBarCode, config.CodeBarCode);
            }
            Payment_Type(config.payment);
            
            return CreateSender();
        }



        private void AddItem(PosItem PosItem)
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
            if(PosItem.Sconto != null && PosItem.Sconto != 0)
            {
                items += scontoSingolo.Replace("{0}", PosItem.Sconto.ToString().Replace(",", "."));
            }
        }
        private void Addpiva(SellerInfo info)
        {
            piva += p_iva.Replace("{0}", info.p_iva);
        }
        private void AddCf(Client info)
        {
            piva += c_f.Replace("[[c_f]]", info.fiscal_code);
        }
        private void AddBarCodeMixed(string valore, string codice)
        {
            other += bar_code.Replace("{0}", valore);
        }
        private void AddBarCode(Barcode barcode)
        {
            //"alleg riga='{0}',barcode={1}	; QRCODE" + "\n"

            switch (barcode.type)
            {
                case BarcodeType.code128:
                    other += bar_code.Replace("{0}", barcode.code).Replace("{1}", "120");  //.Append((bar_code, barcode.code, "120"));

                    break;
                case BarcodeType.code39:
                    other += bar_code.Replace("{0}", barcode.code).Replace("{1}", "3");

                    break;
                case BarcodeType.code39_chk_dgt:
                    other += bar_code.Replace("{0}", barcode.code).Replace("{1}", "2");

                    break;
                case BarcodeType.ean13:
                    other += bar_code.Replace("{0}", barcode.code).Replace("{1}", "1");

                    break;
                case BarcodeType.qrcode:
                    other += bar_code.Replace("{0}", barcode.code).Replace("{1}", "158");

                    break;


            }
        }

        private void AddSconto(string sconto)
        {
            other += Sconto.Replace("[[sconto]]", sconto);


        }
        private void AddFirstCourtesy(string curt1)
        {
            other += courtesy1.Replace("[[MSG1]]", curt1);


        }
        private void AddSecondCourtesy(string curt1)
        {
            other += courtesy2.Replace("[[MSG2]]", curt1);


        }

        private string CreateSender()
        {
            return reponse
                .Replace("{0}", piva)
                .Replace("{1}", items.ToString())
                .Replace("{2}", other.ToString())
                .Replace("{3}", pay.ToString());
        }

        private void Payment_Type(PrintConfig.payment_type payment)

        {
            /*
             * ; Tipo=1  corrisponde a CONTANTI
               ; Tipo=2  corrisponde a CREDITO
               ; Tipo=3  corrisponde a ASSEGNI
               ; Tipo=4  corrisponde a BUONI
               ; Tipo=5  corrisponde a CARTA DI CREDITO
             * 
             */

            switch (payment)
            {
                case PrintConfig.payment_type.CASH:
                    pay = "chius T=1";
                    break;
                case PrintConfig.payment_type.ASSIGN:
                    pay = "chius T=3";
                    break;

                case PrintConfig.payment_type.CARD:
                    pay = "chius T=5";
                    break;

                default:
                    pay = "chius T=1";
                    break;

            }


        }


    }
}