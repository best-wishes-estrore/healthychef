using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.Common
{
    [Serializable]
    public class CardInfo
    {
        public CardInfo()
        {

        }

        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public Enums.CreditCardType CardType { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string SecurityCode { get; set; }

        public bool HasValues
        {
            get
            {
                return !String.IsNullOrWhiteSpace(CardNumber)
                    && CardType != Enums.CreditCardType.Unknown
                    && ExpMonth > 0
                    && ExpYear > 0
                    && !string.IsNullOrWhiteSpace(SecurityCode);
            }
        }
    }

}
