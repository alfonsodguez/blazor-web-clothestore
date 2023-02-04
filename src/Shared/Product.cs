using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zalandu.Shared
{
    public class Product
    {
        public String  ProductId       { get; set; } 
        public int     CategoryId      { get; set; }
        public String  CategoryParent  { get; set; }
        public String  CategoryChild   { get; set; }
        public String  CategoryProduct { get; set; } //<---subcategory name (grandchild)
        public String  ProductName     { get; set; }
        public String  Brand           { get; set; }
        public Decimal Price           { get; set; }
        public Decimal Discount        { get; set; }
        public String  Color           { get; set; }
        public String  ImageId         { get; set; }
        public String  Materials       { get; set; }
        public String  Feature         { get; set; } 
        public String  SizeCut         { get; set; }
        public String  Sustainability  { get; set; }
        public Decimal Rating          { get; set; }
        [NotMapped]
        public List<StockProduct> Stock { get; set; }  

        //-------constructor--------
        public Product(){}
    }
}

