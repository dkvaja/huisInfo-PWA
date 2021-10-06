using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.Models
{
    public class AddressModel
    {
        public AddressModel() { }
        public AddressModel(Adres address)
        {
            Street = address.Straat;
            HouseNo = address.Nummer;
            HouseNoAddition = address.NummerToevoeging;
            Postcode = address.Postcode;
            Place = address.Plaats;
        }

        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string HouseNoAddition { get; set; }
        public string Postcode { get; set; }
        public string Place { get; set; }
    }
}
