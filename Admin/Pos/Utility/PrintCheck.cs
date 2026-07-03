using DAL.Model;
using Admin.Models.CustomModels;
using Admin.Pos.Classes;
using Admin.Pos.Printers;
using Admin.Pos.Utility;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Admin.Pos.Utility
{
    public class PrintCheck
    {
        public StampantePOS StampaFatturaPOS(List<TabellaOrdiniTavolo> items, List<DettaglioProdottiOrdine> items2, string tipo_pagamento, double? servizio, double? sconto)
        {
            PrintConfig config = new PrintConfig();
            List<PosItem> posItems = new List<PosItem>();
            // aggiungo  a lista degli item su scontrino

            if (items != null)
            {
                foreach (var a in items)
                {
                    posItems.Add(new PosItem
                    {
                        tax = (int)a.iva,
                        name = a.NomeProdotto,
                        price = (double)a.prezzoUnitario,
                        qty = (int)a.QuantitaTotale,
                    });
                }

                posItems.Add(new PosItem
                {
                    tax = 10,
                    name = "Servizio",
                    price = (double)servizio,
                    qty = 1,
                });
            }
            else
            {
                foreach (var a in items2)
                {
                    posItems.Add(new PosItem
                    {
                        tax = (int)a.iva,
                        name = a.NomeProdotto,
                        price = (double)a.prezzoUnitario,
                        qty = (int)a.Quantita
                    });
                }
            }

            config.items = posItems;
            config.client = new Client
            {
                fiscal_code = "",
                name = ""

            };
            //configurazione del venditore
            SellerInfo seller = new SellerInfo();
            seller.name = "";//azienda.intestazione;
            seller.p_iva = "";
            if (sconto != null && sconto !=0) seller.sconto = ((double)sconto).ToString("N2").Replace(",", ".");
            config.Courtesy_message = "";//azienda.intestazione;

            POS pos = new POS();
            pos.name = "";//stampanteFiscale.nome;
            pos.ip = WebConfigurationManager.AppSettings["IpStampante"];
            pos.https = true;

            switch (tipo_pagamento)
            {
                case "Contanti":
                    config.payment = PrintConfig.payment_type.CASH;
                    break;
                case "Carte":
                    config.payment = PrintConfig.payment_type.CARD;
                    break;
                case "Bonifico":
                    config.payment = PrintConfig.payment_type.ASSIGN;
                    break;
                case "Assegno":
                    config.payment = PrintConfig.payment_type.ASSIGN;
                    break;
                default:
                    config.payment = PrintConfig.payment_type.CASH;
                    break;
            }

            config.sellerInfo = seller;
            config.pos = pos;
            PosPrinter conf = new PosPrinter(config);
            StampantePOS model = new StampantePOS();
            //invia alla stampante attraverso chiamata API  e prende la response
            model.Body = conf.GetBody();
            return model;

        }
    }
}