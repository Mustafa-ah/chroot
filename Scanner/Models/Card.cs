using System;
using System.Collections.Generic;

namespace Scanner.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public string Carrier { get; set; }
        public string CardNumber { get; set; }
        public DateTime ChargingDate { get; set; }
        public string ProiderName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool IsSelected { get; set; }
        public int Position { get; set; }
        public List<Card> Services { get; set; }
        public string Description { get; set; }
        public Card()
        {
            Services = new List<Card>();
        }
    }

    public enum Proiders
    {
        Etisalat,
        Vodafone,
        Orange,
        We
    }
}