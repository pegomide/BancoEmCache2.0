using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Vale.GetFuseData.Service.Models
{
    /// <remarks/>
    [Serializable(), XmlRoot("o")]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class LoteDadosQualidadeXML
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


        /// <remarks/>
        [Serializable()]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        public partial class Message
        {
            /// <remarks/>
            [XmlElement(ElementName = "lotList")]

            public Lot LotList { get; set; }

            /// <remarks/>
            [XmlElement(ElementName = "portId")]
            public string PortId { get; set; }

            [XmlRoot(ElementName = "lotList")]
            public partial class Lot
            {

                [XmlElement(ElementName = "e")]
                public LotData[] LotListElement { get; set; }


                /// <remarks/>
                [Serializable()]
                [System.ComponentModel.DesignerCategory("code")]
                [XmlType(AnonymousType = true)]
                public partial class LotData
                {
                    /// <remarks/>
                    [XmlElement(ElementName = "fstLstWag")]
                    public string FstLstWag { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "lotArrvlDt")]
                    public string LotArrvlDt { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "lotId")]
                    public string LotId { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "lotSit")]
                    public string LotSit { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "numVehicTrans")]
                    public byte NumVehicTrans { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "prefixID")]
                    public string PrefixID { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "prodID")]
                    public string ProdID { get; set; }

                    /// <remarks/>
                    [XmlArrayItem("qltyList", IsNullable = false)]
                    public Chemical[] QltyList { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "qtyTrans")]
                    public string QtyTrans { get; set; }

                    /// <remarks/>
                    [XmlElement(ElementName = "srcMine")]
                    public string SrcMine { get; set; }


                    /// <remarks/>
                    [Serializable()]
                    [System.ComponentModel.DesignerCategory("code")]
                    [XmlType(AnonymousType = true)]
                    public partial class Chemical
                    {
                        /// <remarks/>
                        [XmlElement(ElementName = "c")]
                        public string C { get; set; }

                        /// <remarks/>
                        [XmlElement(ElementName = "n")]
                        public string N { get; set; }

                        /// <remarks/>
                        [XmlElement(ElementName = "t")]
                        public string T { get; set; }

                        /// <remarks/>
                        [XmlElement(ElementName = "v")]
                        public decimal V { get; set; }
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