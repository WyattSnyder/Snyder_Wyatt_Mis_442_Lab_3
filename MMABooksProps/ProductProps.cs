using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MMABooksProps
{
    [Serializable()]
    public class ProductProps : IBaseProps
    {
        public int ProductId { get; set; } = 0;
        public string ProductCode { get; set; } = "";
        public string Description { get; set; } = "";
        public int OnHandsQuantity { get; set; } = 0;
        public string UnitPrice { get; set; } = "";
        public int ConcurrencyID { get; set; } = 0;

        public object Clone()
        {
            ProductProps p = new ProductProps();
            p.ProductId = this.ProductId;
            p.ProductCode = this.ProductCode;
            p.OnHandsQuantity = this.OnHandsQuantity;
            p.UnitPrice = this.UnitPrice;
            p.UnitPrice = this.UnitPrice;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }

        public string GetState()
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        public void SetState(string jsonString)
        {
            ProductProps p = JsonSerializer.Deserialize<ProductProps>(jsonString);
            this.ProductId = p.ProductId;
            this.ProductCode = p.ProductCode;
            this.OnHandsQuantity = p.OnHandsQuantity;
            this.Description = p.Description;
            this.UnitPrice = p.UnitPrice;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            this.ProductId = (int)dr["ProductID"];
            this.ProductCode = (string)dr["ProductCode"];
            this.UnitPrice = (string)dr["UnitPrice"];
            this.OnHandsQuantity = (int)dr["OnHandsQuantity"];
            this.Description = (string)dr["Description"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }
    }
}
