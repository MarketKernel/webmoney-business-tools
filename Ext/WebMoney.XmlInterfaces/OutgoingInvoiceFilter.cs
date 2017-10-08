﻿using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OutgoingInvoiceFilter : WmRequest<OutgoingInvoiceRegister>
    {
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLOutInvoices.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLOutInvoicesCert.asp";

        public Purse Purse { get; set; }
        public uint InvoiceId { get; set; }
        public uint OrderId { get; set; }
        public WmDateTime StartTime { get; set; }
        public WmDateTime FinishTime { get; set; }

        protected internal OutgoingInvoiceFilter()
        {
        }

        public OutgoingInvoiceFilter(Purse purse, WmDateTime startTime, WmDateTime finishTime)
        {
            Purse = purse;
            StartTime = startTime;
            FinishTime = finishTime;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", Purse, requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("getoutinvoices"); // <getoutinvoices>

            xmlRequestBuilder.WriteElement("purse", Purse.ToString());
            xmlRequestBuilder.WriteElement("wminvid", InvoiceId);
            xmlRequestBuilder.WriteElement("orderid", OrderId);
            xmlRequestBuilder.WriteElement("datestart", StartTime.ToServerString());
            xmlRequestBuilder.WriteElement("datefinish", FinishTime.ToServerString());

            xmlRequestBuilder.WriteEndElement(); // </getoutinvoices>
        }
    }
}