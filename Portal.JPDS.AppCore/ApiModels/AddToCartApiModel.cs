using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class AddToCartApiModel
    {
        public string BuildingId { get; set; }
        /// <summary>
        /// Can be individual option id or standard option id based on the type of option to be added
        /// </summary>
        public string OptionId { get; set; }
        public decimal? Quantity { get; set; }
        public string Comment { get; set; }
    }
}
