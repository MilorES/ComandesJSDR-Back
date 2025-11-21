using ComandesAPI.Models;
using System.Text;
using System.Xml;

namespace ComandesAPI.Services
{
    public class XmlUblService
    {
        /// <summary>
        /// Generates an XML-UBL Order document from a Comanda entity.
        /// Fields marked with <!-- CAMP NO DISPONIBLE --> are mandatory in UBL but not available in current data model.
        /// </summary>
        public string GenerateOrderXml(Comanda comanda, Usuari usuari)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using var stringWriter = new StringWriter();
            using var writer = XmlWriter.Create(stringWriter, settings);

            // UBL 2.1 Order namespace
            writer.WriteStartDocument();
            writer.WriteStartElement("Order", "urn:oasis:names:specification:ubl:schema:xsd:Order-2");
            writer.WriteAttributeString("xmlns", "cac", null, "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            writer.WriteAttributeString("xmlns", "cbc", null, "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            // UBL Version
            writer.WriteElementString("cbc", "UBLVersionID", null, "2.1");
            writer.WriteElementString("cbc", "CustomizationID", null, "urn:cen.eu:en16931:2017");

            // Order ID and dates
            writer.WriteElementString("cbc", "ID", null, comanda.NumeroComanda);
            writer.WriteElementString("cbc", "IssueDate", null, comanda.DataCreacio.ToString("yyyy-MM-dd"));
            writer.WriteElementString("cbc", "IssueTime", null, comanda.DataCreacio.ToString("HH:mm:ss"));

            // Order status
            writer.WriteElementString("cbc", "OrderTypeCode", null, MapEstatToUblCode(comanda.Estat));

            // Notes/Observations
            if (!string.IsNullOrWhiteSpace(comanda.Observacions))
            {
                writer.WriteElementString("cbc", "Note", null, comanda.Observacions);
            }

            // Document Currency - <!-- CAMP NO DISPONIBLE: Moneda no disponible al model, assumint EUR -->
            writer.WriteElementString("cbc", "DocumentCurrencyCode", null, "EUR");

            // Buyer Customer Party
            writer.WriteStartElement("cac", "BuyerCustomerParty", null);
            writer.WriteStartElement("cac", "Party", null);

            // <!-- CAMP NO DISPONIBLE: PartyIdentification (NIF/CIF) no disponible al model d'usuari -->
            writer.WriteComment(" CAMP NO DISPONIBLE: PartyIdentification (NIF/CIF) - afegir camp a Usuari si es necessita ");

            writer.WriteStartElement("cac", "PartyName", null);
            writer.WriteElementString("cbc", "Name", null, usuari.FullName);
            writer.WriteEndElement(); // PartyName

            // Postal Address - <!-- CAMP NO DISPONIBLE -->
            writer.WriteComment(" CAMP NO DISPONIBLE: PostalAddress - afegir camps d'adreça a Usuari si es necessita ");
            writer.WriteStartElement("cac", "PostalAddress", null);
            writer.WriteElementString("cbc", "StreetName", null, "");
            writer.WriteElementString("cbc", "CityName", null, "");
            writer.WriteElementString("cbc", "PostalZone", null, "");
            writer.WriteStartElement("cac", "Country", null);
            writer.WriteElementString("cbc", "IdentificationCode", null, "ES");
            writer.WriteEndElement(); // Country
            writer.WriteEndElement(); // PostalAddress

            // Contact
            writer.WriteStartElement("cac", "Contact", null);
            writer.WriteElementString("cbc", "Name", null, usuari.FullName);
            writer.WriteElementString("cbc", "ElectronicMail", null, usuari.Email);
            // <!-- CAMP NO DISPONIBLE: Telephone no disponible al model d'usuari -->
            writer.WriteComment(" CAMP NO DISPONIBLE: Telephone - afegir camp de telèfon a Usuari si es necessita ");
            writer.WriteEndElement(); // Contact

            writer.WriteEndElement(); // Party
            writer.WriteEndElement(); // BuyerCustomerParty

            // Seller Supplier Party - <!-- CAMP NO DISPONIBLE: Informació del venedor -->
            writer.WriteComment(" CAMP NO DISPONIBLE: SellerSupplierParty - afegir configuració d'empresa si es necessita ");
            writer.WriteStartElement("cac", "SellerSupplierParty", null);
            writer.WriteStartElement("cac", "Party", null);
            writer.WriteStartElement("cac", "PartyName", null);
            writer.WriteElementString("cbc", "Name", null, "");
            writer.WriteEndElement(); // PartyName
            writer.WriteEndElement(); // Party
            writer.WriteEndElement(); // SellerSupplierParty

            // Delivery - <!-- CAMP NO DISPONIBLE: Informació d'entrega -->
            if (comanda.DataFinalitzacio.HasValue)
            {
                writer.WriteStartElement("cac", "Delivery", null);
                writer.WriteStartElement("cac", "RequestedDeliveryPeriod", null);
                writer.WriteElementString("cbc", "EndDate", null, comanda.DataFinalitzacio.Value.ToString("yyyy-MM-dd"));
                writer.WriteEndElement(); // RequestedDeliveryPeriod
                writer.WriteComment(" CAMP NO DISPONIBLE: DeliveryLocation - afegir adreça d'entrega si es necessita ");
                writer.WriteEndElement(); // Delivery
            }

            // Allowance/Charge for order-level discount
            if (comanda.DescomptePercentatge > 0)
            {
                writer.WriteStartElement("cac", "AllowanceCharge", null);
                writer.WriteElementString("cbc", "ChargeIndicator", null, "false");
                writer.WriteElementString("cbc", "AllowanceChargeReasonCode", null, "95"); // Discount
                writer.WriteElementString("cbc", "AllowanceChargeReason", null, "Descompte de comanda");
                writer.WriteElementString("cbc", "MultiplierFactorNumeric", null, (comanda.DescomptePercentatge / 100).ToString("F4"));
                writer.WriteStartElement("cbc", "Amount", null);
                writer.WriteAttributeString("currencyID", "EUR");
                writer.WriteString(comanda.ImportDescompte.ToString("F2"));
                writer.WriteEndElement(); // Amount
                writer.WriteStartElement("cbc", "BaseAmount", null);
                writer.WriteAttributeString("currencyID", "EUR");
                writer.WriteString(comanda.Total.ToString("F2"));
                writer.WriteEndElement(); // BaseAmount
                writer.WriteEndElement(); // AllowanceCharge
            }

            // Tax Total - <!-- CAMP NO DISPONIBLE: Impostos no disponibles al model -->
            writer.WriteComment(" CAMP NO DISPONIBLE: TaxTotal - afegir camps d'IVA/impostos si es necessita ");
            writer.WriteStartElement("cac", "TaxTotal", null);
            writer.WriteStartElement("cbc", "TaxAmount", null);
            writer.WriteAttributeString("currencyID", "EUR");
            writer.WriteString("0.00");
            writer.WriteEndElement(); // TaxAmount
            writer.WriteEndElement(); // TaxTotal

            // Anticipated Monetary Total
            writer.WriteStartElement("cac", "AnticipatedMonetaryTotal", null);

            writer.WriteStartElement("cbc", "LineExtensionAmount", null);
            writer.WriteAttributeString("currencyID", "EUR");
            writer.WriteString(comanda.Total.ToString("F2"));
            writer.WriteEndElement(); // LineExtensionAmount

            writer.WriteStartElement("cbc", "AllowanceTotalAmount", null);
            writer.WriteAttributeString("currencyID", "EUR");
            writer.WriteString(comanda.ImportDescompte.ToString("F2"));
            writer.WriteEndElement(); // AllowanceTotalAmount

            // <!-- CAMP NO DISPONIBLE: TaxExclusiveAmount i TaxInclusiveAmount -->
            writer.WriteStartElement("cbc", "TaxExclusiveAmount", null);
            writer.WriteAttributeString("currencyID", "EUR");
            writer.WriteString(comanda.TotalAmbDescompte.ToString("F2"));
            writer.WriteEndElement(); // TaxExclusiveAmount

            writer.WriteStartElement("cbc", "PayableAmount", null);
            writer.WriteAttributeString("currencyID", "EUR");
            writer.WriteString(comanda.TotalAmbDescompte.ToString("F2"));
            writer.WriteEndElement(); // PayableAmount

            writer.WriteEndElement(); // AnticipatedMonetaryTotal

            // Order Lines
            int lineNumber = 1;
            foreach (var linia in comanda.Linies.OrderBy(l => l.Ordre))
            {
                writer.WriteStartElement("cac", "OrderLine", null);

                // Line Extension Amount
                writer.WriteStartElement("cac", "LineItem", null);
                writer.WriteElementString("cbc", "ID", null, lineNumber.ToString());

                writer.WriteStartElement("cbc", "Quantity", null);
                // <!-- CAMP NO DISPONIBLE: unitCode (unitat de mesura) no disponible -->
                writer.WriteAttributeString("unitCode", "EA"); // Each - unitat per defecte
                writer.WriteString(linia.Quantitat.ToString("F2"));
                writer.WriteEndElement(); // Quantity

                writer.WriteStartElement("cbc", "LineExtensionAmount", null);
                writer.WriteAttributeString("currencyID", "EUR");
                writer.WriteString(linia.Total.ToString("F2"));
                writer.WriteEndElement(); // LineExtensionAmount

                // Line level discount
                if (linia.DescomptePercentatge > 0)
                {
                    writer.WriteStartElement("cac", "AllowanceCharge", null);
                    writer.WriteElementString("cbc", "ChargeIndicator", null, "false");
                    writer.WriteElementString("cbc", "AllowanceChargeReasonCode", null, "95");
                    writer.WriteElementString("cbc", "AllowanceChargeReason", null, "Descompte de línia");
                    writer.WriteElementString("cbc", "MultiplierFactorNumeric", null, (linia.DescomptePercentatge / 100).ToString("F4"));
                    writer.WriteStartElement("cbc", "Amount", null);
                    writer.WriteAttributeString("currencyID", "EUR");
                    writer.WriteString(linia.ImportDescompte.ToString("F2"));
                    writer.WriteEndElement(); // Amount
                    writer.WriteStartElement("cbc", "BaseAmount", null);
                    writer.WriteAttributeString("currencyID", "EUR");
                    writer.WriteString(linia.Subtotal.ToString("F2"));
                    writer.WriteEndElement(); // BaseAmount
                    writer.WriteEndElement(); // AllowanceCharge
                }

                // Price
                writer.WriteStartElement("cac", "Price", null);
                writer.WriteStartElement("cbc", "PriceAmount", null);
                writer.WriteAttributeString("currencyID", "EUR");
                writer.WriteString(linia.PreuUnitari.ToString("F2"));
                writer.WriteEndElement(); // PriceAmount
                writer.WriteEndElement(); // Price

                // Item
                writer.WriteStartElement("cac", "Item", null);

                if (!string.IsNullOrWhiteSpace(linia.Descripcio))
                {
                    writer.WriteElementString("cbc", "Description", null, linia.Descripcio);
                }

                writer.WriteElementString("cbc", "Name", null, linia.NomProducte);

                // Sellers Item Identification
                if (linia.ArticleId.HasValue)
                {
                    writer.WriteStartElement("cac", "SellersItemIdentification", null);
                    writer.WriteElementString("cbc", "ID", null, linia.ArticleId.Value.ToString());
                    writer.WriteEndElement(); // SellersItemIdentification
                }

                // <!-- CAMP NO DISPONIBLE: StandardItemIdentification (codi de barres, EAN, etc.) -->
                writer.WriteComment(" CAMP NO DISPONIBLE: StandardItemIdentification - afegir codi de barres/EAN a Article si es necessita ");

                // <!-- CAMP NO DISPONIBLE: ClassifiedTaxCategory (categoria d'IVA) -->
                writer.WriteComment(" CAMP NO DISPONIBLE: ClassifiedTaxCategory - afegir tipus d'IVA a Article si es necessita ");

                writer.WriteEndElement(); // Item
                writer.WriteEndElement(); // LineItem
                writer.WriteEndElement(); // OrderLine

                lineNumber++;
            }

            writer.WriteEndElement(); // Order
            writer.WriteEndDocument();

            writer.Flush();
            return stringWriter.ToString();
        }

        private string MapEstatToUblCode(EstatComanda estat)
        {
            return estat switch
            {
                EstatComanda.Esborrany => "1", // Not yet released
                EstatComanda.PendentAprovacio => "5", // Pending
                EstatComanda.Aprovada => "29", // Accepted
                EstatComanda.EnProces => "12", // In process
                EstatComanda.Enviada => "21", // Delivered
                EstatComanda.Finalitzada => "9", // Completed
                EstatComanda.Cancellada => "4", // Cancelled
                _ => "1"
            };
        }
    }
}
