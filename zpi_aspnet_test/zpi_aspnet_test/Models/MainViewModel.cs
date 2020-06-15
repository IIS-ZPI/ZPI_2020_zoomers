using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zpi_aspnet_test.Models
{
    public class MainViewModel
    {
        public SelectList ProductSelectList { get; set; }

        public SelectList CategorySelectList { get; set; }

        public SelectList StateSelectList { get; set; }

        public ProductModel ChosenProduct { get; set; }

        public StateOfAmericaModel ChosenState { get; set; }

        public List<string> StateNameList { get; set; }

        public List<ProductModel> ProductList { get; set; }

        public double PurchasePrice { get; set; }

        public double PreferredPrice { get; set; }

        public int NumberOfProducts { get; set; }

        public List<double> Tax { get; set; }

        public List<double> FinalPrice { get; set; }

        public List<double> Margin { get; set; }

        public string Category { get; set; }
    }
}