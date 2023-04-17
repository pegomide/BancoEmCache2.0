using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Vale.GetFuseData.Service.Models
{
    /// <remarks/>
    [Serializable(), XmlRoot("o")]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class EmbarqueDadosQualidadeXML
    {
        /// <remarks/>
        [XmlElement(ElementName = "countMessages")]
        public byte CountMessages { get; set; }

        /// <remarks/>
        [XmlElement(ElementName = "message")]
        public Message MessageField { get; set; }

        /// <remarks/>
        [XmlElement(ElementName = "messageError")]
        public string MessageError { get; set; }

        /// <remarks/>
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }

        [Serializable()]
        public class Message
        {
            /// <remarks/>
            [XmlElement(ElementName = "pierList")]
            public Pier PierList { get; set; }

            /// <remarks/>
            [XmlElement(ElementName = "portId")]
            public string PortId { get; set; }

            [XmlRoot(ElementName = "pierList")]
            public class Pier
            {

                [XmlElement(ElementName = "e")]
                public PierData[] PierListElement { get; set; }


                /// <remarks/>
                [Serializable()]
                public class PierData
                {
                    /// <remarks/>
                    [XmlElement(ElementName = "pierId")]
                    public string PierId { get; set; }

                    /// <remarks/>
                    [XmlElement("shipmentList")]
                    public Shipment ShipmentList { get; set; }


                    [XmlRoot(ElementName = "shipmentList")]
                    public class Shipment
                    {

                        [XmlElement(ElementName = "e")]
                        public List<ShipmentData> ShipmentListElement { get; set; }


                        /// <remarks/>
                        [Serializable()]
                        public class ShipmentData
                        {

                            /// <remarks/>
                            [XmlElement(ElementName = "eventDt")]
                            public string EventDt { get; set; }

                            /// <remarks/>
                            [XmlElement("productList")]
                            public Product ProductList { get; set; }

                            /// <remarks/>
                            [XmlElement(ElementName = "shipID")]
                            public string ShipID { get; set; }

                            /// <remarks/>
                            [XmlElement(ElementName = "shipName")]
                            public string ShipName { get; set; }

                            /// <remarks/>
                            [XmlElement(ElementName = "shipSit")]
                            public string ShipSit { get; set; }


                            [XmlRoot(ElementName = "productList")]
                            public class Product
                            {

                                [XmlElement(ElementName = "e")]
                                public ProductData[] ProductListElement { get; set; }


                                /// <remarks/>
                                [Serializable()]
                                [XmlType(AnonymousType = true)]
                                public class ProductData
                                {

                                    /// <remarks/>
                                    [XmlElement("clientList")]
                                    public Client ClientList { get; set; }

                                    /// <remarks/>
                                    [XmlElement(ElementName = "prodId")]
                                    public string ProdId { get; set; }

                                    [XmlRoot(ElementName = "clientList")]
                                    public class Client
                                    {

                                        [XmlElement(ElementName = "e")]
                                        public ClientData[] ClienteListElement { get; set; }



                                        /// <remarks/>
                                        [Serializable()]
                                        [XmlType(AnonymousType = true)]
                                        public class ClientData
                                        {
                                            /// <remarks/>
                                            [XmlElement(ElementName = "actQty")]
                                            public uint ActQty { get; set; }

                                            /// <remarks/>
                                            [XmlElement(ElementName = "clientId")]
                                            public string ClientId { get; set; }

                                            /// <remarks/>
                                            [XmlElement("contractualSpecificationList")]
                                            public ContractualSpecification ContractualSpecificationList { get; set; }

                                            /// <remarks/>
                                            [XmlElement(ElementName = "fcstQty")]
                                            public uint FcstQty { get; set; }

                                            /// <remarks/>
                                            [XmlElement("hatchList")]
                                            public Hatch HatchList { get; set; }

                                            [XmlRoot(ElementName = "contractualSpecificationList")]
                                            public class ContractualSpecification
                                            {

                                                [XmlElement(ElementName = "e")]
                                                public ContractualSpecificationData[] ContractualSpecificationListElement { get; set; }


                                                /// <remarks/>
                                                [Serializable()]
                                                public class ContractualSpecificationData
                                                {

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "c")]
                                                    public string C { get; set; }

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "iv")]
                                                    public string IV { get; set; }

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "n")]
                                                    public string N { get; set; }

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "sv")]
                                                    public string SV { get; set; }

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "t")]
                                                    public string T { get; set; }

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "v")]
                                                    public decimal V { get; set; }
                                                }
                                            }

                                            [XmlRoot(ElementName = "hatchList")]
                                            public class Hatch
                                            {

                                                [XmlElement(ElementName = "e")]
                                                public HatchData[] HatchListElement { get; set; }

                                                /// <remarks/>
                                                [Serializable()]
                                                public class HatchData
                                                {
                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "actQty")]
                                                    public ushort ActQty { get; set; }

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "fcstQty")]
                                                    public ushort FcstQty { get; set; }

                                                    /// <remarks/>
                                                    [XmlElement(ElementName = "htchHldNum")]
                                                    public string HtchHldNum { get; set; }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}